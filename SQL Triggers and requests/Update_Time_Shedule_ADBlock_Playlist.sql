DECLARE @currentTime datetime = '2024-15-10 07:00:00.000';  
DECLARE @DiffTime INT = 0;
DECLARE @CountRows int = 99;
DECLARE @SelectTimePlaylist int = 0;
DECLARE @SelectTimeAdBlock int = 0;
DECLARE @i int = 1;
print @CurrentTime;
-- ���� WHILE ��� ��������� ���
WHILE @i <= @CountRows
BEGIN
	SELECT @SelectTimePlaylist = datediff(second, '00:00:00', ��������.�����������������) 
	FROM �������� join ����������_���������� ON ����������_����������.������������ = ��������.������������ 
	where ����������_����������.��������� = @i;
	print @currentTime;
	SELECT @currentTime = DATEADD(SECOND, @SelectTimePlaylist + 5, @currentTime)
	print @currentTime;
	UPDATE ����������_���������_������
    SET ��������� = @currentTime
    WHERE ��������� = @i;

	SELECT @SelectTimeAdBlock = datediff(second, '00:00:00', ���������_����.�����������������) 
	FROM ���������_���� join ����������_���������_������ ON ����������_���������_������.�������� = ���������_����.�������� 
	where ����������_���������_������.��������� = @i;

	SELECT @currentTime = DATEADD(SECOND, @SelectTimeAdBlock + 5, @currentTime)
	SET @i = @i + 1;  -- ��������� � ��������� ������
	UPDATE ����������_����������
    SET ��������� = @currentTime
    WHERE ��������� = @i;
END;

-- ��������� ��������� 
-- SELECT * FROM ��������;
