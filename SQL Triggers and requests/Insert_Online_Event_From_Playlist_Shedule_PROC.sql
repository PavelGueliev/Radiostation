

Alter PROCEDURE InsertOnlineEvents
AS
BEGIN
    SET NOCOUNT ON;

DECLARE track_cursor CURSOR LOCAL SCROLL STATIC
FOR 
SELECT 
    rp.���������,  
    rp.������������, 
    t.duration,  
    t.title,  
    t.id, 
    rp.���������
FROM 
    Shedule_playlists rp
    JOIN Playlist p ON p.������������ = rp.������������ 
    JOIN Playlist_composition sp ON sp.������������ = p.������������
    JOIN Track t ON sp.�������� = t.id
WHERE 
    rp.IsDeleted = 0 AND
    p.IsDeleted = 0 AND
    sp.IsDeleted = 0 AND
    t.IsDeleted = 0
ORDER BY 
    rp.������������, rp.���������, t.id; -- ���������, ��� ������� ���������� ������������� ����� ������

OPEN track_cursor;
TRUNCATE TABLE Online_event;
-- ���������� ��������� ���������� ��� FETCH
DECLARE @��������� int, 
        @������������ int, 
        @duration time, 
        @title char(200), 
        @id int, 
        @playlistTime datetime;

DECLARE @i int = 1;
DECLARE @currentTime datetime;
DECLARE @DiffPlaylist INT;

FETCH NEXT FROM track_cursor INTO @���������, @������������, @duration, @title, @id, @playlistTime;

IF @@FETCH_STATUS = 0
BEGIN
    SET @currentTime = @playlistTime;  
    SET @DiffPlaylist = @������������;
END

WHILE @@FETCH_STATUS = 0
BEGIN
    -- ������� � Online_event (��������������, ��� ���������� �������� IDENTITY)
    INSERT INTO Online_event (��������, ���������)
    VALUES (@id, @currentTime);

    -- ���������� �������� ������� �������
    SET @currentTime = DATEADD(SECOND, DATEDIFF(SECOND, '00:00:00', @duration), @currentTime);

    -- ��������� ��������� ������
    FETCH NEXT FROM track_cursor INTO @���������, @������������, @duration, @title, @id, @playlistTime;
    
    -- �������� ����� ���������
    IF @DiffPlaylist <> @������������
    BEGIN
        SET @DiffPlaylist = @������������;
        SET @currentTime = @playlistTime;
    END

    -- ���������� ��������
    SET @i = @i + 1;
    PRINT @i;
END

CLOSE track_cursor;
DEALLOCATE track_cursor;
END
GO