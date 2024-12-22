USE Radiostation  
-- ���������� ������� min_mark_cursor � ������������ ��������
DECLARE track_cursor CURSOR LOCAL SCROLL STATIC
FOR SELECT ���������, ��������, ����������_����������.������������, Track.duration,  Track.title,  Track.id, ����������_����������.���������

FROM ����������_���������� JOIN �������� ON ��������.������������ = ����������_����������.������������ 
Join ������_��������� ON ������_���������.������������ = ��������.������������
Join Track ON ������_���������.�������� = Track.id
GROUP BY ���������, ��������, ����������_����������.������������,  Track.title,  Track.id, Track.duration, ����������_����������.���������

OPEN track_cursor
-- ���������� ������� ��� �������� �����������
DECLARE @res_table TABLE (��������� int, �������� char(200), ������������ int, duration time, date_time datetime, title char(200), id int)
-- ���������� ��������� ���������� ��� FETCH
DECLARE @��������� int, @�������� char(200), @������������ int, @duration time, @title char(200), @id int, @i int, @playlistTime datetime;
set @i = 1;
FETCH NEXT FROM track_cursor INTO @���������, @��������, @������������, @duration, @title, @id, @playlistTime
DECLARE @currentTime datetime = @playlistTime;  
DECLARE @DiffPlaylist INT = @������������;

WHILE @@FETCH_STATUS = 0
BEGIN
	--INSERT INTO @res_table VALUES (@���������, @��������, @������������, @duration, @currentTime, @title, @id)
	INSERT INTO �������_������� VALUES (@i, @id, @currentTime)

	SELECT @currentTime = DATEADD(SECOND, datediff(second, '00:00:00', @duration), @currentTime)

	FETCH NEXT FROM track_cursor INTO @���������, @��������, @������������, @duration, @title, @id, @playlistTime
	
	If @DiffPlaylist <> @������������
		BEGIN
			SELECT @DiffPlaylist = @������������
			SELECT @currentTime = @playlistTime
		END
	set @i = @i + 1;
	print @i
END
-- ������� ����������
SELECT * FROM @res_table
CLOSE track_cursor
DEALLOCATE track_cursor