DECLARE @currentTime datetime = '2024-15-10 07:00:00.000';  
DECLARE @DiffTime INT = 0;
DECLARE @CountRows int = 99;
DECLARE @SelectTimePlaylist int = 0;
DECLARE @SelectTimeAdBlock int = 0;
DECLARE @i int = 1;
print @CurrentTime;
-- Цикл WHILE для изменения цен
WHILE @i <= @CountRows
BEGIN
	SELECT @SelectTimePlaylist = datediff(second, '00:00:00', Плейлист.Продолжительность) 
	FROM Плейлист join Расписание_плейлистов ON Расписание_плейлистов.КодПлейлиста = Плейлист.КодПлейлиста 
	where Расписание_плейлистов.КодЗаписи = @i;
	print @currentTime;
	SELECT @currentTime = DATEADD(SECOND, @SelectTimePlaylist + 5, @currentTime)
	print @currentTime;
	UPDATE Расписание_рекламных_блоков
    SET ДатаВремя = @currentTime
    WHERE КодЗаписи = @i;

	SELECT @SelectTimeAdBlock = datediff(second, '00:00:00', Рекламный_блок.Продолжительность) 
	FROM Рекламный_блок join Расписание_рекламных_блоков ON Расписание_рекламных_блоков.КодБлока = Рекламный_блок.КодБлока 
	where Расписание_рекламных_блоков.КодЗаписи = @i;

	SELECT @currentTime = DATEADD(SECOND, @SelectTimeAdBlock + 5, @currentTime)
	SET @i = @i + 1;  -- Переходим к следующей строке
	UPDATE Расписание_плейлистов
    SET ДатаВремя = @currentTime
    WHERE КодЗаписи = @i;
END;

-- Проверяем результат 
-- SELECT * FROM Продукты;
