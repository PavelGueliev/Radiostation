ALTER TRIGGER trg_Playlist_UpdateOnTrackInsert
ON Playlist_composition
AFTER INSERT
AS
BEGIN
    SET NOCOUNT ON;

    /*
      1. Вложенный запрос (подзапрос) получает все ID плейлистов, 
         затронутых вставкой (могут быть несколько за раз, если вставка пакетная).
      2. Для каждого плейлиста из inserted пересчитываем суммарную длительность 
         всех треков, находящихся в таблице Playlist_composition.
    */

    UPDATE p
    SET p.Продолжительность = (
        SELECT CAST(DATEADD(SECOND, 
               SUM(DATEDIFF(SECOND, '00:00:00', t.duration)), 
               '00:00:00') AS time)
        FROM Playlist_composition pc
        JOIN Track t ON t.id = pc.КодТрека
        WHERE pc.КодПлейлиста = p.КодПлейлиста
          AND pc.IsDeleted = 0  -- Если используете IsDeleted
          AND t.IsDeleted = 0   -- Если трек может быть помечен как удалённый
    )
    FROM Playlist p
    JOIN inserted i ON p.КодПлейлиста = i.КодПлейлиста;
END
GO
ALTER TRIGGER trg_Playlist_UpdateOnTrackDelete
ON Playlist_composition
AFTER DELETE
AS
BEGIN
    SET NOCOUNT ON;

    ;WITH DurationCalculation AS (
        SELECT 
            pc.КодПлейлиста,
            Продолжительность = ISNULL(
                CAST(DATEADD(SECOND, SUM(DATEDIFF(SECOND, '00:00:00', t.duration)), '00:00:00') AS time),
                '00:00:00'
            )
        FROM Playlist_composition pc
        LEFT JOIN Track t ON t.id = pc.КодТрека AND t.IsDeleted = 0
        WHERE pc.IsDeleted = 0
        GROUP BY pc.КодПлейлиста
    )
    UPDATE p
    SET p.Продолжительность = dc.Продолжительность
    FROM Playlist p
    JOIN deleted d ON p.КодПлейлиста = d.КодПлейлиста
    LEFT JOIN DurationCalculation dc ON p.КодПлейлиста = dc.КодПлейлиста;
END
GO
ALTER TRIGGER trg_AdBlock_UpdateOnAdInsert
ON Block_composition
AFTER INSERT
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE ab
    SET ab.Продолжительность = (
        SELECT CAST(DATEADD(SECOND,
               SUM(DATEDIFF(SECOND, '00:00:00', r.Продолжительность)),
               '00:00:00') AS time)
        FROM Block_composition bc
        JOIN Ad_record r ON r.КодРолика = bc.КодРолика
        WHERE bc.КодБлока = ab.КодБлока
          AND bc.IsDeleted = 0  -- Если используете IsDeleted
          AND r.IsDeleted = 0
    )
    FROM Ad_block ab
    JOIN inserted i ON ab.КодБлока = i.КодБлока;
END
GO
ALTER TRIGGER trg_AdBlock_UpdateOnAdDelete
ON Block_composition
AFTER DELETE
AS
BEGIN
    SET NOCOUNT ON;

    ;WITH DurationCalculation AS (
        SELECT 
            bc.КодБлока,
            Продолжительность = ISNULL(
                CAST(DATEADD(SECOND, SUM(DATEDIFF(SECOND, '00:00:00', r.Продолжительность)), '00:00:00') AS time),
                '00:00:00'
            )
        FROM Block_composition bc
        LEFT JOIN Ad_record r ON r.КодРолика = bc.КодРолика AND r.IsDeleted = 0
        WHERE bc.IsDeleted = 0
        GROUP BY bc.КодБлока
    )
    UPDATE ab
    SET ab.Продолжительность = dc.Продолжительность
    FROM Ad_block ab
    JOIN deleted d ON ab.КодБлока = d.КодБлока
    LEFT JOIN DurationCalculation dc ON ab.КодБлока = dc.КодБлока;
END
GO