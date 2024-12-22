

Alter PROCEDURE InsertOnlineEvents
AS
BEGIN
    SET NOCOUNT ON;

DECLARE track_cursor CURSOR LOCAL SCROLL STATIC
FOR 
SELECT 
    rp.КодЗаписи,  
    rp.КодПлейлиста, 
    t.duration,  
    t.title,  
    t.id, 
    rp.ДатаВремя
FROM 
    Shedule_playlists rp
    JOIN Playlist p ON p.КодПлейлиста = rp.КодПлейлиста 
    JOIN Playlist_composition sp ON sp.КодПлейлиста = p.КодПлейлиста
    JOIN Track t ON sp.КодТрека = t.id
WHERE 
    rp.IsDeleted = 0 AND
    p.IsDeleted = 0 AND
    sp.IsDeleted = 0 AND
    t.IsDeleted = 0
ORDER BY 
    rp.КодПлейлиста, rp.ДатаВремя, t.id; -- Убедитесь, что порядок сортировки соответствует вашей логике

OPEN track_cursor;
TRUNCATE TABLE Online_event;
-- Объявление локальных переменных для FETCH
DECLARE @КодЗаписи int, 
        @КодПлейлиста int, 
        @duration time, 
        @title char(200), 
        @id int, 
        @playlistTime datetime;

DECLARE @i int = 1;
DECLARE @currentTime datetime;
DECLARE @DiffPlaylist INT;

FETCH NEXT FROM track_cursor INTO @КодЗаписи, @КодПлейлиста, @duration, @title, @id, @playlistTime;

IF @@FETCH_STATUS = 0
BEGIN
    SET @currentTime = @playlistTime;  
    SET @DiffPlaylist = @КодПлейлиста;
END

WHILE @@FETCH_STATUS = 0
BEGIN
    -- Вставка в Online_event (предполагается, что КодСобытия является IDENTITY)
    INSERT INTO Online_event (КодТрека, ДатаВремя)
    VALUES (@id, @currentTime);

    -- Обновление текущего времени события
    SET @currentTime = DATEADD(SECOND, DATEDIFF(SECOND, '00:00:00', @duration), @currentTime);

    -- Получение следующей записи
    FETCH NEXT FROM track_cursor INTO @КодЗаписи, @КодПлейлиста, @duration, @title, @id, @playlistTime;
    
    -- Проверка смены плейлиста
    IF @DiffPlaylist <> @КодПлейлиста
    BEGIN
        SET @DiffPlaylist = @КодПлейлиста;
        SET @currentTime = @playlistTime;
    END

    -- Увеличение счётчика
    SET @i = @i + 1;
    PRINT @i;
END

CLOSE track_cursor;
DEALLOCATE track_cursor;
END
GO