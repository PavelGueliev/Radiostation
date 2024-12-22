USE Radiostation  
-- объявление курсора min_mark_cursor с минимальными оценками
DECLARE track_cursor CURSOR LOCAL SCROLL STATIC
FOR SELECT КодЗаписи, Название, Расписание_плейлистов.КодПлейлиста, Track.duration,  Track.title,  Track.id, Расписание_плейлистов.ДатаВремя

FROM Расписание_плейлистов JOIN Плейлист ON Плейлист.КодПлейлиста = Расписание_плейлистов.КодПлейлиста 
Join Состав_Плейлиста ON Состав_Плейлиста.КодПлейлиста = Плейлист.КодПлейлиста
Join Track ON Состав_Плейлиста.КодТрека = Track.id
GROUP BY КодЗаписи, Название, Расписание_плейлистов.КодПлейлиста,  Track.title,  Track.id, Track.duration, Расписание_плейлистов.ДатаВремя

OPEN track_cursor
-- определить таблицу для хранения результатов
DECLARE @res_table TABLE (КодЗаписи int, Название char(200), КодПлейлиста int, duration time, date_time datetime, title char(200), id int)
-- объявление локальных переменных для FETCH
DECLARE @КодЗаписи int, @Название char(200), @КодПлейлиста int, @duration time, @title char(200), @id int, @i int, @playlistTime datetime;
set @i = 1;
FETCH NEXT FROM track_cursor INTO @КодЗаписи, @Название, @КодПлейлиста, @duration, @title, @id, @playlistTime
DECLARE @currentTime datetime = @playlistTime;  
DECLARE @DiffPlaylist INT = @КодПлейлиста;

WHILE @@FETCH_STATUS = 0
BEGIN
	--INSERT INTO @res_table VALUES (@КодЗаписи, @Название, @КодПлейлиста, @duration, @currentTime, @title, @id)
	INSERT INTO Эфирное_событие VALUES (@i, @id, @currentTime)

	SELECT @currentTime = DATEADD(SECOND, datediff(second, '00:00:00', @duration), @currentTime)

	FETCH NEXT FROM track_cursor INTO @КодЗаписи, @Название, @КодПлейлиста, @duration, @title, @id, @playlistTime
	
	If @DiffPlaylist <> @КодПлейлиста
		BEGIN
			SELECT @DiffPlaylist = @КодПлейлиста
			SELECT @currentTime = @playlistTime
		END
	set @i = @i + 1;
	print @i
END
-- вывести результаты
SELECT * FROM @res_table
CLOSE track_cursor
DEALLOCATE track_cursor