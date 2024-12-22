-- ============================================================
-- 1. Триггеры для таблицы Presenter
-- ============================================================

-- 1.1. Триггер для проверки уникальности номера телефона
ALTER TRIGGER trg_Presenter_UniquePhone
ON Presenter
AFTER INSERT, UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    -- Проверка уникальности номеров телефонов внутри вставленных/обновленных записей
    IF EXISTS (
        SELECT *
        FROM (
            SELECT НомерТелефона
            FROM inserted
            WHERE IsDeleted = 0
            GROUP BY НомерТелефона
            HAVING COUNT(*) > 1
        ) AS DupPhones
    )
    BEGIN
        RAISERROR('Вставляемые номера телефонов должны быть уникальными среди активных записей.', 16, 1);
        ROLLBACK TRANSACTION;
        RETURN;
    END

    -- Проверка уникальности номеров телефонов среди всех активных записей
    IF EXISTS (
        SELECT *
        FROM Presenter p
        JOIN inserted i ON p.НомерТелефона = i.НомерТелефона
        WHERE p.КодВедущего <> i.КодВедущего
          AND p.IsDeleted = 0
          AND i.IsDeleted = 0
    )
    BEGIN
        RAISERROR('Номер телефона уже существует среди активных записей.', 16, 1);
        ROLLBACK TRANSACTION;
        RETURN;
    END
END
GO

-- 1.2. Триггер для проверки возраста ведущего (не младше 18 лет)
ALTER TRIGGER trg_Presenter_Age
ON Presenter
AFTER INSERT, UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    IF EXISTS (
        SELECT *
        FROM inserted
        WHERE ДатаРождения > DATEADD(year, -18, GETDATE())
          AND IsDeleted = 0
    )
    BEGIN
        RAISERROR('Ведущий должен быть не младше 18 лет.', 16, 1);
        ROLLBACK TRANSACTION;
    END
END
GO

-- ============================================================
-- 2. Триггеры для таблицы Shedule_playlists
-- ============================================================

-- 2.1. Триггер на вставку и обновление расписаний плейлистов
ALTER TRIGGER trg_Shedule_playlists_NoOverlap
ON Shedule_playlists
AFTER INSERT, UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    -- Проверка перекрытия с другими активными расписаниями плейлистов
    IF EXISTS (
        SELECT *
        FROM inserted i
        JOIN Playlist p1 ON i.КодПлейлиста = p1.КодПлейлиста
        CROSS APPLY (
            SELECT DATEADD(SECOND, DATEDIFF(SECOND, '00:00:00', p1.Продолжительность), i.ДатаВремя) AS EndTime
        ) AS calc
        JOIN Shedule_playlists sp ON sp.ДатаВремя > calc.EndTime
                                  AND sp.IsDeleted = 0
                                  AND sp.КодЗаписи <> i.КодЗаписи
    )
    BEGIN
        RAISERROR('Расписание плейлиста пересекается с другим активным плейлистом.', 16, 1);
        ROLLBACK TRANSACTION;
        RETURN;
    END

    -- Проверка перекрытия с активными расписаниями рекламных блоков
    IF EXISTS (
        SELECT *
        FROM inserted i
        JOIN Playlist p ON i.КодПлейлиста = p.КодПлейлиста
        CROSS APPLY (
            SELECT DATEADD(SECOND, DATEDIFF(SECOND, '00:00:00', p.Продолжительность), i.ДатаВремя) AS EndTime
        ) AS calc
        JOIN Shedule_ads_block sab ON sab.ДатаВремя > calc.EndTime
                                  AND sab.IsDeleted = 0
    )
    BEGIN
        RAISERROR('Расписание плейлиста пересекается с активным рекламным блоком.', 16, 1);
        ROLLBACK TRANSACTION;
        RETURN;
    END

    -- Проверка наличия хотя бы одного активного плейлиста в расписании
    IF NOT EXISTS (
        SELECT *
        FROM Shedule_playlists
        WHERE IsDeleted = 0
    )
    BEGIN
        RAISERROR('В расписании плейлистов должен быть как минимум один активный плейлист.', 16, 1);
        ROLLBACK TRANSACTION;
        RETURN;
    END
END
GO

-- 2.2. Триггер на обновление IsDeleted в Shedule_playlists
ALTER TRIGGER trg_Shedule_playlists_PreventDeleteLast
ON Shedule_playlists
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    -- Проверка, что после обновления остается хотя бы один активный плейлист
    IF NOT EXISTS (
        SELECT *
        FROM Shedule_playlists
        WHERE IsDeleted = 0
    )
    BEGIN
        RAISERROR('В расписании плейлистов должен быть как минимум один активный плейлист.', 16, 1);
        ROLLBACK TRANSACTION;
        RETURN;
    END
END
GO

-- ============================================================
-- 3. Триггеры для таблицы Shedule_ads_block
-- ============================================================

-- 3.1. Триггер на вставку и обновление расписаний рекламных блоков
ALTER TRIGGER trg_Shedule_ads_block_NoOverlap
ON Shedule_ads_block
AFTER INSERT, UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    -- Проверка перекрытия с другими активными расписаниями рекламных блоков
    IF EXISTS (
        SELECT *
        FROM inserted i
        JOIN Ad_block ab1 ON i.КодБлока = ab1.КодБлока
        CROSS APPLY (
            SELECT DATEADD(SECOND, DATEDIFF(SECOND, '00:00:00', ab1.Продолжительность), i.ДатаВремя) AS EndTime
        ) AS calc
        JOIN Shedule_ads_block sab ON sab.ДатаВремя > calc.EndTime
                                   AND sab.IsDeleted = 0
                                   AND sab.КодЗаписи <> i.КодЗаписи
    )
    BEGIN
        RAISERROR('Расписание рекламного блока пересекается с другим активным рекламным блоком.', 16, 1);
        ROLLBACK TRANSACTION;
        RETURN;
    END

    -- Проверка перекрытия с активными расписаниями плейлистов
    IF EXISTS (
        SELECT *
        FROM inserted i
        JOIN Ad_block ab ON i.КодБлока = ab.КодБлока
        CROSS APPLY (
            SELECT DATEADD(SECOND, DATEDIFF(SECOND, '00:00:00', ab.Продолжительность), i.ДатаВремя) AS EndTime
        ) AS calc
        JOIN Shedule_playlists sp ON sp.ДатаВремя > calc.EndTime
                                 AND sp.IsDeleted = 0
    )
    BEGIN
        RAISERROR('Расписание рекламного блока пересекается с активным плейлистом.', 16, 1);
        ROLLBACK TRANSACTION;
        RETURN;
    END

    -- Проверка наличия хотя бы одного активного рекламного блока в расписании
    IF NOT EXISTS (
        SELECT *
        FROM Shedule_ads_block
        WHERE IsDeleted = 0
    )
    BEGIN
        RAISERROR('В расписании рекламных блоков должен быть как минимум один активный рекламный блок.', 16, 1);
        ROLLBACK TRANSACTION;
        RETURN;
    END
END
GO

-- 3.2. Триггер на обновление IsDeleted в Shedule_ads_block
ALTER TRIGGER trg_Shedule_ads_block_PreventDeleteLast
ON Shedule_ads_block
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    -- Проверка, что после обновления остается хотя бы один активный рекламный блок
    IF NOT EXISTS (
        SELECT *
        FROM Shedule_ads_block
        WHERE IsDeleted = 0
    )
    BEGIN
        RAISERROR('В расписании рекламных блоков должен быть как минимум один активный рекламный блок.', 16, 1);
        ROLLBACK TRANSACTION;
        RETURN;
    END
END
GO

-- ============================================================
-- 4. Триггеры для таблицы Playlist
-- ============================================================

-- 4.1. Триггер для обеспечения положительной длительности плейлиста
ALTER TRIGGER trg_Playlist_Duration_Positive
ON Playlist
AFTER INSERT, UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    IF EXISTS (
        SELECT *
        FROM inserted
        WHERE Продолжительность <= '00:00:00'
          AND IsDeleted = 0
    )
    BEGIN
        RAISERROR('Длительность плейлиста должна быть больше 0.', 16, 1);
        ROLLBACK TRANSACTION;
        RETURN;
    END
END
GO

-- 4.2. Триггер для предотвращения перекрытия расписаний при изменении длительности плейлиста
ALTER TRIGGER trg_Playlist_UpdateDuration_NoOverlap
ON Playlist
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    IF UPDATE(Продолжительность)
    BEGIN
        -- Проверка перекрытия с другими активными расписаниями плейлистов
        IF EXISTS (
            SELECT *
            FROM Shedule_playlists sp
            JOIN inserted i ON sp.КодПлейлиста = i.КодПлейлиста
            JOIN Playlist p ON i.КодПлейлиста = p.КодПлейлиста
            CROSS APPLY (
                SELECT DATEADD(SECOND, DATEDIFF(SECOND, '00:00:00', i.Продолжительность), sp.ДатаВремя) AS EndTime
            ) AS calc
            WHERE sp.IsDeleted = 0
              AND (
                  EXISTS (
                      SELECT *
                      FROM Shedule_playlists sp2
                      JOIN Playlist p2 ON sp2.КодПлейлиста = p2.КодПлейлиста
                      WHERE sp2.ДатаВремя < calc.EndTime
                        AND p2.IsDeleted = 0
                        AND sp2.КодЗаписи <> sp.КодЗаписи
                        AND sp2.IsDeleted = 0
                  )
                  OR EXISTS (
                      SELECT *
                      FROM Shedule_ads_block sab
                      JOIN Ad_block ab ON sab.КодБлока = ab.КодБлока
                      WHERE sab.ДатаВремя < calc.EndTime
                        AND ab.IsDeleted = 0
                        AND sab.IsDeleted = 0
                  )
              )
        )
        BEGIN
            RAISERROR('Изменение длительности плейлиста вызывает перекрытие в расписании.', 16, 1);
            ROLLBACK TRANSACTION;
            RETURN;
        END
    END
END
GO

-- ============================================================
-- 5. Триггеры для таблицы Ad_block
-- ============================================================

-- 5.1. Триггер для обеспечения положительной длительности рекламного блока
ALTER TRIGGER trg_Ad_block_Duration_Positive
ON Ad_block
AFTER INSERT, UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    IF EXISTS (
        SELECT *
        FROM inserted
        WHERE Продолжительность <= '00:00:00'
          AND IsDeleted = 0
    )
    BEGIN
        RAISERROR('Длительность рекламного блока должна быть больше 0.', 16, 1);
        ROLLBACK TRANSACTION;
        RETURN;
    END
END
GO

-- 5.2. Триггер для предотвращения перекрытия расписаний при изменении длительности рекламного блока
ALTER TRIGGER trg_Ad_block_UpdateDuration_NoOverlap
ON Ad_block
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    IF UPDATE(Продолжительность)
    BEGIN
        -- Проверка перекрытия с другими активными расписаниями рекламных блоков
        IF EXISTS (
            SELECT *
            FROM Shedule_ads_block sab
            JOIN inserted i ON sab.КодБлока = i.КодБлока
            JOIN Ad_block ab ON i.КодБлока = ab.КодБлока
            CROSS APPLY (
                SELECT DATEADD(SECOND, DATEDIFF(SECOND, '00:00:00', i.Продолжительность), sab.ДатаВремя) AS EndTime
            ) AS calc
            WHERE sab.IsDeleted = 0
              AND (
                  EXISTS (
                      SELECT *
                      FROM Shedule_ads_block sab2
                      JOIN Ad_block ab2 ON sab2.КодБлока = ab2.КодБлока
                      WHERE sab2.ДатаВремя < calc.EndTime
                        AND ab2.IsDeleted = 0
                        AND sab2.КодЗаписи <> sab.КодЗаписи
                        AND sab2.IsDeleted = 0
                  )
                  OR EXISTS (
                      SELECT *
                      FROM Shedule_playlists sp
                      JOIN Playlist p ON sp.КодПлейлиста = p.КодПлейлиста
                      WHERE sp.ДатаВремя < calc.EndTime
                        AND p.IsDeleted = 0
                        AND sp.IsDeleted = 0
                  )
              )
        )
        BEGIN
            RAISERROR('Изменение длительности рекламного блока вызывает перекрытие в расписании.', 16, 1);
            ROLLBACK TRANSACTION;
            RETURN;
        END
    END
END
GO

-- ============================================================
-- 6. Триггеры для таблицы Online_event
-- ============================================================

-- 6.1. Триггер на обновление КодТрека или ДатаВремя в Online_event
ALTER TRIGGER trg_Online_event_UpdateTime
ON Online_event
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    IF UPDATE(КодТрека) OR UPDATE(ДатаВремя)
    BEGIN
        UPDATE oe
        SET oe.ДатаВремя = DATEADD(SECOND, DATEDIFF(SECOND, '00:00:00', t.duration), oe.ДатаВремя)
        FROM Online_event oe
        JOIN inserted i ON oe.КодСобытия = i.КодСобытия
        JOIN Track t ON i.КодТрека = t.id
        WHERE oe.IsDeleted = 0
          AND t.IsDeleted = 0;
    END
END
GO

-- ============================================================
-- 7. Триггеры для обеспечения наличия хотя бы одного активного плейлиста и рекламного блока в расписании
-- ============================================================

-- 7.1. Триггер на обновление IsDeleted в Playlist_composition
ALTER TRIGGER trg_Playlist_composition_SoftDelete
ON Playlist_composition
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    -- Здесь можно добавить дополнительную логику при необходимости
    -- Например, логирование или обновление связанных данных
END
GO

-- 7.2. Триггер на обновление IsDeleted в Block_composition
ALTER TRIGGER trg_Block_composition_SoftDelete
ON Block_composition
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    -- Здесь можно добавить дополнительную логику при необходимости
    -- Например, логирование или обновление связанных данных
END
GO

-- ============================================================
-- 8. Дополнительные Триггеры для Soft Deletes
-- ============================================================

-- 8.1. Триггер на обновление IsDeleted в Shedule_playlists для предотвращения удаления последней активной записи
ALTER TRIGGER trg_Shedule_playlists_SoftDelete_PreventLast
ON Shedule_playlists
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    -- Проверяем, что после обновления остается хотя бы один активный плейлист
    IF NOT EXISTS (
        SELECT *
        FROM Shedule_playlists
        WHERE IsDeleted = 0
    )
    BEGIN
        RAISERROR('В расписании плейлистов должен быть как минимум один активный плейлист.', 16, 1);
        ROLLBACK TRANSACTION;
        RETURN;
    END
END
GO

-- 8.2. Триггер на обновление IsDeleted в Shedule_ads_block для предотвращения удаления последней активной записи
ALTER TRIGGER trg_Shedule_ads_block_SoftDelete_PreventLast
ON Shedule_ads_block
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    -- Проверяем, что после обновления остается хотя бы один активный рекламный блок
    IF NOT EXISTS (
        SELECT *
        FROM Shedule_ads_block
        WHERE IsDeleted = 0
    )
    BEGIN
        RAISERROR('В расписании рекламных блоков должен быть как минимум один активный рекламный блок.', 16, 1);
        ROLLBACK TRANSACTION;
        RETURN;
    END
END
GO

-- ============================================================
-- 9. Триггеры для обновления времени эфирных событий при изменении расписания
-- ============================================================

-- 9.1. Триггер на обновление ДатаВремя в Shedule_playlists
ALTER TRIGGER trg_Shedule_playlists_UpdateEventTime
ON Shedule_playlists
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    IF UPDATE(ДатаВремя)
    BEGIN
        UPDATE es
        SET es.ДатаВремя = DATEADD(SECOND, DATEDIFF(SECOND, '00:00:00', p.Продолжительность), sp.ДатаВремя)
        FROM Online_event es
        JOIN Shedule_playlists sp ON es.КодСобытия = sp.КодЗаписи
        JOIN Playlist p ON sp.КодПлейлиста = p.КодПлейлиста
        JOIN inserted i ON sp.КодЗаписи = i.КодЗаписи
        WHERE es.IsDeleted = 0
          AND sp.IsDeleted = 0
          AND p.IsDeleted = 0;
    END
END
GO

-- 9.2. Триггер на обновление ДатаВремя в Shedule_ads_block
ALTER TRIGGER trg_Shedule_ads_block_UpdateEventTime
ON Shedule_ads_block
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    IF UPDATE(ДатаВремя)
    BEGIN
        UPDATE es
        SET es.ДатаВремя = DATEADD(SECOND, DATEDIFF(SECOND, '00:00:00', ab.Продолжительность), sab.ДатаВремя)
        FROM Online_event es
        JOIN Shedule_ads_block sab ON es.КодСобытия = sab.КодЗаписи
        JOIN Ad_block ab ON sab.КодБлока = ab.КодБлока
        JOIN inserted i ON sab.КодЗаписи = i.КодЗаписи
        WHERE es.IsDeleted = 0
          AND sab.IsDeleted = 0
          AND ab.IsDeleted = 0;
    END
END
GO

