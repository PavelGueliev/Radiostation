ALTER TRIGGER trg_Playlist_UpdateOnTrackInsert
ON Playlist_composition
AFTER INSERT
AS
BEGIN
    SET NOCOUNT ON;

    /*
      1. ��������� ������ (���������) �������� ��� ID ����������, 
         ���������� �������� (����� ���� ��������� �� ���, ���� ������� ��������).
      2. ��� ������� ��������� �� inserted ������������� ��������� ������������ 
         ���� ������, ����������� � ������� Playlist_composition.
    */

    UPDATE p
    SET p.����������������� = (
        SELECT CAST(DATEADD(SECOND, 
               SUM(DATEDIFF(SECOND, '00:00:00', t.duration)), 
               '00:00:00') AS time)
        FROM Playlist_composition pc
        JOIN Track t ON t.id = pc.��������
        WHERE pc.������������ = p.������������
          AND pc.IsDeleted = 0  -- ���� ����������� IsDeleted
          AND t.IsDeleted = 0   -- ���� ���� ����� ���� ������� ��� ��������
    )
    FROM Playlist p
    JOIN inserted i ON p.������������ = i.������������;
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
            pc.������������,
            ����������������� = ISNULL(
                CAST(DATEADD(SECOND, SUM(DATEDIFF(SECOND, '00:00:00', t.duration)), '00:00:00') AS time),
                '00:00:00'
            )
        FROM Playlist_composition pc
        LEFT JOIN Track t ON t.id = pc.�������� AND t.IsDeleted = 0
        WHERE pc.IsDeleted = 0
        GROUP BY pc.������������
    )
    UPDATE p
    SET p.����������������� = dc.�����������������
    FROM Playlist p
    JOIN deleted d ON p.������������ = d.������������
    LEFT JOIN DurationCalculation dc ON p.������������ = dc.������������;
END
GO
ALTER TRIGGER trg_AdBlock_UpdateOnAdInsert
ON Block_composition
AFTER INSERT
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE ab
    SET ab.����������������� = (
        SELECT CAST(DATEADD(SECOND,
               SUM(DATEDIFF(SECOND, '00:00:00', r.�����������������)),
               '00:00:00') AS time)
        FROM Block_composition bc
        JOIN Ad_record r ON r.��������� = bc.���������
        WHERE bc.�������� = ab.��������
          AND bc.IsDeleted = 0  -- ���� ����������� IsDeleted
          AND r.IsDeleted = 0
    )
    FROM Ad_block ab
    JOIN inserted i ON ab.�������� = i.��������;
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
            bc.��������,
            ����������������� = ISNULL(
                CAST(DATEADD(SECOND, SUM(DATEDIFF(SECOND, '00:00:00', r.�����������������)), '00:00:00') AS time),
                '00:00:00'
            )
        FROM Block_composition bc
        LEFT JOIN Ad_record r ON r.��������� = bc.��������� AND r.IsDeleted = 0
        WHERE bc.IsDeleted = 0
        GROUP BY bc.��������
    )
    UPDATE ab
    SET ab.����������������� = dc.�����������������
    FROM Ad_block ab
    JOIN deleted d ON ab.�������� = d.��������
    LEFT JOIN DurationCalculation dc ON ab.�������� = dc.��������;
END
GO