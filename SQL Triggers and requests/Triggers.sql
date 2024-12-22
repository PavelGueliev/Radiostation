-- ============================================================
-- 1. �������� ��� ������� Presenter
-- ============================================================

-- 1.1. ������� ��� �������� ������������ ������ ��������
ALTER TRIGGER trg_Presenter_UniquePhone
ON Presenter
AFTER INSERT, UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    -- �������� ������������ ������� ��������� ������ �����������/����������� �������
    IF EXISTS (
        SELECT *
        FROM (
            SELECT �������������
            FROM inserted
            WHERE IsDeleted = 0
            GROUP BY �������������
            HAVING COUNT(*) > 1
        ) AS DupPhones
    )
    BEGIN
        RAISERROR('����������� ������ ��������� ������ ���� ����������� ����� �������� �������.', 16, 1);
        ROLLBACK TRANSACTION;
        RETURN;
    END

    -- �������� ������������ ������� ��������� ����� ���� �������� �������
    IF EXISTS (
        SELECT *
        FROM Presenter p
        JOIN inserted i ON p.������������� = i.�������������
        WHERE p.����������� <> i.�����������
          AND p.IsDeleted = 0
          AND i.IsDeleted = 0
    )
    BEGIN
        RAISERROR('����� �������� ��� ���������� ����� �������� �������.', 16, 1);
        ROLLBACK TRANSACTION;
        RETURN;
    END
END
GO

-- 1.2. ������� ��� �������� �������� �������� (�� ������ 18 ���)
ALTER TRIGGER trg_Presenter_Age
ON Presenter
AFTER INSERT, UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    IF EXISTS (
        SELECT *
        FROM inserted
        WHERE ������������ > DATEADD(year, -18, GETDATE())
          AND IsDeleted = 0
    )
    BEGIN
        RAISERROR('������� ������ ���� �� ������ 18 ���.', 16, 1);
        ROLLBACK TRANSACTION;
    END
END
GO

-- ============================================================
-- 2. �������� ��� ������� Shedule_playlists
-- ============================================================

-- 2.1. ������� �� ������� � ���������� ���������� ����������
ALTER TRIGGER trg_Shedule_playlists_NoOverlap
ON Shedule_playlists
AFTER INSERT, UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    -- �������� ���������� � ������� ��������� ������������ ����������
    IF EXISTS (
        SELECT *
        FROM inserted i
        JOIN Playlist p1 ON i.������������ = p1.������������
        CROSS APPLY (
            SELECT DATEADD(SECOND, DATEDIFF(SECOND, '00:00:00', p1.�����������������), i.���������) AS EndTime
        ) AS calc
        JOIN Shedule_playlists sp ON sp.��������� > calc.EndTime
                                  AND sp.IsDeleted = 0
                                  AND sp.��������� <> i.���������
    )
    BEGIN
        RAISERROR('���������� ��������� ������������ � ������ �������� ����������.', 16, 1);
        ROLLBACK TRANSACTION;
        RETURN;
    END

    -- �������� ���������� � ��������� ������������ ��������� ������
    IF EXISTS (
        SELECT *
        FROM inserted i
        JOIN Playlist p ON i.������������ = p.������������
        CROSS APPLY (
            SELECT DATEADD(SECOND, DATEDIFF(SECOND, '00:00:00', p.�����������������), i.���������) AS EndTime
        ) AS calc
        JOIN Shedule_ads_block sab ON sab.��������� > calc.EndTime
                                  AND sab.IsDeleted = 0
    )
    BEGIN
        RAISERROR('���������� ��������� ������������ � �������� ��������� ������.', 16, 1);
        ROLLBACK TRANSACTION;
        RETURN;
    END

    -- �������� ������� ���� �� ������ ��������� ��������� � ����������
    IF NOT EXISTS (
        SELECT *
        FROM Shedule_playlists
        WHERE IsDeleted = 0
    )
    BEGIN
        RAISERROR('� ���������� ���������� ������ ���� ��� ������� ���� �������� ��������.', 16, 1);
        ROLLBACK TRANSACTION;
        RETURN;
    END
END
GO

-- 2.2. ������� �� ���������� IsDeleted � Shedule_playlists
ALTER TRIGGER trg_Shedule_playlists_PreventDeleteLast
ON Shedule_playlists
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    -- ��������, ��� ����� ���������� �������� ���� �� ���� �������� ��������
    IF NOT EXISTS (
        SELECT *
        FROM Shedule_playlists
        WHERE IsDeleted = 0
    )
    BEGIN
        RAISERROR('� ���������� ���������� ������ ���� ��� ������� ���� �������� ��������.', 16, 1);
        ROLLBACK TRANSACTION;
        RETURN;
    END
END
GO

-- ============================================================
-- 3. �������� ��� ������� Shedule_ads_block
-- ============================================================

-- 3.1. ������� �� ������� � ���������� ���������� ��������� ������
ALTER TRIGGER trg_Shedule_ads_block_NoOverlap
ON Shedule_ads_block
AFTER INSERT, UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    -- �������� ���������� � ������� ��������� ������������ ��������� ������
    IF EXISTS (
        SELECT *
        FROM inserted i
        JOIN Ad_block ab1 ON i.�������� = ab1.��������
        CROSS APPLY (
            SELECT DATEADD(SECOND, DATEDIFF(SECOND, '00:00:00', ab1.�����������������), i.���������) AS EndTime
        ) AS calc
        JOIN Shedule_ads_block sab ON sab.��������� > calc.EndTime
                                   AND sab.IsDeleted = 0
                                   AND sab.��������� <> i.���������
    )
    BEGIN
        RAISERROR('���������� ���������� ����� ������������ � ������ �������� ��������� ������.', 16, 1);
        ROLLBACK TRANSACTION;
        RETURN;
    END

    -- �������� ���������� � ��������� ������������ ����������
    IF EXISTS (
        SELECT *
        FROM inserted i
        JOIN Ad_block ab ON i.�������� = ab.��������
        CROSS APPLY (
            SELECT DATEADD(SECOND, DATEDIFF(SECOND, '00:00:00', ab.�����������������), i.���������) AS EndTime
        ) AS calc
        JOIN Shedule_playlists sp ON sp.��������� > calc.EndTime
                                 AND sp.IsDeleted = 0
    )
    BEGIN
        RAISERROR('���������� ���������� ����� ������������ � �������� ����������.', 16, 1);
        ROLLBACK TRANSACTION;
        RETURN;
    END

    -- �������� ������� ���� �� ������ ��������� ���������� ����� � ����������
    IF NOT EXISTS (
        SELECT *
        FROM Shedule_ads_block
        WHERE IsDeleted = 0
    )
    BEGIN
        RAISERROR('� ���������� ��������� ������ ������ ���� ��� ������� ���� �������� ��������� ����.', 16, 1);
        ROLLBACK TRANSACTION;
        RETURN;
    END
END
GO

-- 3.2. ������� �� ���������� IsDeleted � Shedule_ads_block
ALTER TRIGGER trg_Shedule_ads_block_PreventDeleteLast
ON Shedule_ads_block
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    -- ��������, ��� ����� ���������� �������� ���� �� ���� �������� ��������� ����
    IF NOT EXISTS (
        SELECT *
        FROM Shedule_ads_block
        WHERE IsDeleted = 0
    )
    BEGIN
        RAISERROR('� ���������� ��������� ������ ������ ���� ��� ������� ���� �������� ��������� ����.', 16, 1);
        ROLLBACK TRANSACTION;
        RETURN;
    END
END
GO

-- ============================================================
-- 4. �������� ��� ������� Playlist
-- ============================================================

-- 4.1. ������� ��� ����������� ������������� ������������ ���������
ALTER TRIGGER trg_Playlist_Duration_Positive
ON Playlist
AFTER INSERT, UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    IF EXISTS (
        SELECT *
        FROM inserted
        WHERE ����������������� <= '00:00:00'
          AND IsDeleted = 0
    )
    BEGIN
        RAISERROR('������������ ��������� ������ ���� ������ 0.', 16, 1);
        ROLLBACK TRANSACTION;
        RETURN;
    END
END
GO

-- 4.2. ������� ��� �������������� ���������� ���������� ��� ��������� ������������ ���������
ALTER TRIGGER trg_Playlist_UpdateDuration_NoOverlap
ON Playlist
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    IF UPDATE(�����������������)
    BEGIN
        -- �������� ���������� � ������� ��������� ������������ ����������
        IF EXISTS (
            SELECT *
            FROM Shedule_playlists sp
            JOIN inserted i ON sp.������������ = i.������������
            JOIN Playlist p ON i.������������ = p.������������
            CROSS APPLY (
                SELECT DATEADD(SECOND, DATEDIFF(SECOND, '00:00:00', i.�����������������), sp.���������) AS EndTime
            ) AS calc
            WHERE sp.IsDeleted = 0
              AND (
                  EXISTS (
                      SELECT *
                      FROM Shedule_playlists sp2
                      JOIN Playlist p2 ON sp2.������������ = p2.������������
                      WHERE sp2.��������� < calc.EndTime
                        AND p2.IsDeleted = 0
                        AND sp2.��������� <> sp.���������
                        AND sp2.IsDeleted = 0
                  )
                  OR EXISTS (
                      SELECT *
                      FROM Shedule_ads_block sab
                      JOIN Ad_block ab ON sab.�������� = ab.��������
                      WHERE sab.��������� < calc.EndTime
                        AND ab.IsDeleted = 0
                        AND sab.IsDeleted = 0
                  )
              )
        )
        BEGIN
            RAISERROR('��������� ������������ ��������� �������� ���������� � ����������.', 16, 1);
            ROLLBACK TRANSACTION;
            RETURN;
        END
    END
END
GO

-- ============================================================
-- 5. �������� ��� ������� Ad_block
-- ============================================================

-- 5.1. ������� ��� ����������� ������������� ������������ ���������� �����
ALTER TRIGGER trg_Ad_block_Duration_Positive
ON Ad_block
AFTER INSERT, UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    IF EXISTS (
        SELECT *
        FROM inserted
        WHERE ����������������� <= '00:00:00'
          AND IsDeleted = 0
    )
    BEGIN
        RAISERROR('������������ ���������� ����� ������ ���� ������ 0.', 16, 1);
        ROLLBACK TRANSACTION;
        RETURN;
    END
END
GO

-- 5.2. ������� ��� �������������� ���������� ���������� ��� ��������� ������������ ���������� �����
ALTER TRIGGER trg_Ad_block_UpdateDuration_NoOverlap
ON Ad_block
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    IF UPDATE(�����������������)
    BEGIN
        -- �������� ���������� � ������� ��������� ������������ ��������� ������
        IF EXISTS (
            SELECT *
            FROM Shedule_ads_block sab
            JOIN inserted i ON sab.�������� = i.��������
            JOIN Ad_block ab ON i.�������� = ab.��������
            CROSS APPLY (
                SELECT DATEADD(SECOND, DATEDIFF(SECOND, '00:00:00', i.�����������������), sab.���������) AS EndTime
            ) AS calc
            WHERE sab.IsDeleted = 0
              AND (
                  EXISTS (
                      SELECT *
                      FROM Shedule_ads_block sab2
                      JOIN Ad_block ab2 ON sab2.�������� = ab2.��������
                      WHERE sab2.��������� < calc.EndTime
                        AND ab2.IsDeleted = 0
                        AND sab2.��������� <> sab.���������
                        AND sab2.IsDeleted = 0
                  )
                  OR EXISTS (
                      SELECT *
                      FROM Shedule_playlists sp
                      JOIN Playlist p ON sp.������������ = p.������������
                      WHERE sp.��������� < calc.EndTime
                        AND p.IsDeleted = 0
                        AND sp.IsDeleted = 0
                  )
              )
        )
        BEGIN
            RAISERROR('��������� ������������ ���������� ����� �������� ���������� � ����������.', 16, 1);
            ROLLBACK TRANSACTION;
            RETURN;
        END
    END
END
GO

-- ============================================================
-- 6. �������� ��� ������� Online_event
-- ============================================================

-- 6.1. ������� �� ���������� �������� ��� ��������� � Online_event
ALTER TRIGGER trg_Online_event_UpdateTime
ON Online_event
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    IF UPDATE(��������) OR UPDATE(���������)
    BEGIN
        UPDATE oe
        SET oe.��������� = DATEADD(SECOND, DATEDIFF(SECOND, '00:00:00', t.duration), oe.���������)
        FROM Online_event oe
        JOIN inserted i ON oe.���������� = i.����������
        JOIN Track t ON i.�������� = t.id
        WHERE oe.IsDeleted = 0
          AND t.IsDeleted = 0;
    END
END
GO

-- ============================================================
-- 7. �������� ��� ����������� ������� ���� �� ������ ��������� ��������� � ���������� ����� � ����������
-- ============================================================

-- 7.1. ������� �� ���������� IsDeleted � Playlist_composition
ALTER TRIGGER trg_Playlist_composition_SoftDelete
ON Playlist_composition
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    -- ����� ����� �������� �������������� ������ ��� �������������
    -- ��������, ����������� ��� ���������� ��������� ������
END
GO

-- 7.2. ������� �� ���������� IsDeleted � Block_composition
ALTER TRIGGER trg_Block_composition_SoftDelete
ON Block_composition
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    -- ����� ����� �������� �������������� ������ ��� �������������
    -- ��������, ����������� ��� ���������� ��������� ������
END
GO

-- ============================================================
-- 8. �������������� �������� ��� Soft Deletes
-- ============================================================

-- 8.1. ������� �� ���������� IsDeleted � Shedule_playlists ��� �������������� �������� ��������� �������� ������
ALTER TRIGGER trg_Shedule_playlists_SoftDelete_PreventLast
ON Shedule_playlists
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    -- ���������, ��� ����� ���������� �������� ���� �� ���� �������� ��������
    IF NOT EXISTS (
        SELECT *
        FROM Shedule_playlists
        WHERE IsDeleted = 0
    )
    BEGIN
        RAISERROR('� ���������� ���������� ������ ���� ��� ������� ���� �������� ��������.', 16, 1);
        ROLLBACK TRANSACTION;
        RETURN;
    END
END
GO

-- 8.2. ������� �� ���������� IsDeleted � Shedule_ads_block ��� �������������� �������� ��������� �������� ������
ALTER TRIGGER trg_Shedule_ads_block_SoftDelete_PreventLast
ON Shedule_ads_block
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    -- ���������, ��� ����� ���������� �������� ���� �� ���� �������� ��������� ����
    IF NOT EXISTS (
        SELECT *
        FROM Shedule_ads_block
        WHERE IsDeleted = 0
    )
    BEGIN
        RAISERROR('� ���������� ��������� ������ ������ ���� ��� ������� ���� �������� ��������� ����.', 16, 1);
        ROLLBACK TRANSACTION;
        RETURN;
    END
END
GO

-- ============================================================
-- 9. �������� ��� ���������� ������� ������� ������� ��� ��������� ����������
-- ============================================================

-- 9.1. ������� �� ���������� ��������� � Shedule_playlists
ALTER TRIGGER trg_Shedule_playlists_UpdateEventTime
ON Shedule_playlists
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    IF UPDATE(���������)
    BEGIN
        UPDATE es
        SET es.��������� = DATEADD(SECOND, DATEDIFF(SECOND, '00:00:00', p.�����������������), sp.���������)
        FROM Online_event es
        JOIN Shedule_playlists sp ON es.���������� = sp.���������
        JOIN Playlist p ON sp.������������ = p.������������
        JOIN inserted i ON sp.��������� = i.���������
        WHERE es.IsDeleted = 0
          AND sp.IsDeleted = 0
          AND p.IsDeleted = 0;
    END
END
GO

-- 9.2. ������� �� ���������� ��������� � Shedule_ads_block
ALTER TRIGGER trg_Shedule_ads_block_UpdateEventTime
ON Shedule_ads_block
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    IF UPDATE(���������)
    BEGIN
        UPDATE es
        SET es.��������� = DATEADD(SECOND, DATEDIFF(SECOND, '00:00:00', ab.�����������������), sab.���������)
        FROM Online_event es
        JOIN Shedule_ads_block sab ON es.���������� = sab.���������
        JOIN Ad_block ab ON sab.�������� = ab.��������
        JOIN inserted i ON sab.��������� = i.���������
        WHERE es.IsDeleted = 0
          AND sab.IsDeleted = 0
          AND ab.IsDeleted = 0;
    END
END
GO

