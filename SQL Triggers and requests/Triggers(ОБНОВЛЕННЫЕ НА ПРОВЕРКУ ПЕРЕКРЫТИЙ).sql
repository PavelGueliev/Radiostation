ALTER TRIGGER trg_Shedule_playlists_NoOverlap
ON Shedule_playlists
AFTER INSERT, UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    /*
      ������ �������� ����������� (Overlap):
      ��� ��������� (startA, endA) � (startB, endB) ������������, ����
         (startA < endB) AND (endA > startB).
    */

    -- ��������, ��� �����������/���������� �������� 
    -- �� ������������ � ������� ��������� �����������
    IF EXISTS (
        SELECT *
        FROM inserted i
        JOIN Playlist p ON i.������������ = p.������������
        CROSS APPLY (
            SELECT 
                i.��������� AS iStart,
                DATEADD(SECOND, DATEDIFF(SECOND, '00:00:00', p.�����������������), i.���������) AS iEnd
        ) AS cur
        JOIN Shedule_playlists sp ON sp.IsDeleted = 0 
                                  AND sp.��������� <> i.���������
        JOIN Playlist p2 ON sp.������������ = p2.������������
        CROSS APPLY (
            SELECT 
                sp.��������� AS spStart,
                DATEADD(SECOND, DATEDIFF(SECOND, '00:00:00', p2.�����������������), sp.���������) AS spEnd
        ) AS oth
        WHERE 
            cur.iStart < oth.spEnd
            AND cur.iEnd   > oth.spStart
    )
    BEGIN
        RAISERROR('���������� ��������� ������������ � ������ �������� ����������.', 16, 1);
        ROLLBACK TRANSACTION;
        RETURN;
    END;

    -- ��������, ��� �����������/���������� �������� 
    -- �� ������������ � ��������� ������������ ��������� ������
    IF EXISTS (
        SELECT *
        FROM inserted i
        JOIN Playlist p ON i.������������ = p.������������
        CROSS APPLY (
            SELECT 
                i.��������� AS iStart,
                DATEADD(SECOND, DATEDIFF(SECOND, '00:00:00', p.�����������������), i.���������) AS iEnd
        ) AS cur
        JOIN Shedule_ads_block sab ON sab.IsDeleted = 0
        JOIN Ad_block ab ON sab.�������� = ab.��������
        CROSS APPLY (
            SELECT 
                sab.��������� AS sabStart,
                DATEADD(SECOND, DATEDIFF(SECOND, '00:00:00', ab.�����������������), sab.���������) AS sabEnd
        ) AS oth
        WHERE 
            cur.iStart < oth.sabEnd
            AND cur.iEnd   > oth.sabStart
    )
    BEGIN
        RAISERROR('���������� ��������� ������������ � �������� ��������� ������.', 16, 1);
        ROLLBACK TRANSACTION;
        RETURN;
    END;

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
    END;
END
GO
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
        JOIN Ad_block ab ON i.�������� = ab.��������
        CROSS APPLY (
            SELECT 
                i.��������� AS iStart,
                DATEADD(SECOND, DATEDIFF(SECOND, '00:00:00', ab.�����������������), i.���������) AS iEnd
        ) AS cur
        JOIN Shedule_ads_block sab ON sab.IsDeleted = 0
                                   AND sab.��������� <> i.���������
        JOIN Ad_block ab2 ON sab.�������� = ab2.��������
        CROSS APPLY (
            SELECT 
                sab.��������� AS sabStart,
                DATEADD(SECOND, DATEDIFF(SECOND, '00:00:00', ab2.�����������������), sab.���������) AS sabEnd
        ) AS oth
        WHERE 
            cur.iStart < oth.sabEnd
            AND cur.iEnd   > oth.sabStart
    )
    BEGIN
        RAISERROR('���������� ���������� ����� ������������ � ������ �������� ��������� ������.', 16, 1);
        ROLLBACK TRANSACTION;
        RETURN;
    END;

    -- �������� ���������� � ��������� ������������ ����������
    IF EXISTS (
        SELECT *
        FROM inserted i
        JOIN Ad_block ab ON i.�������� = ab.��������
        CROSS APPLY (
            SELECT 
                i.��������� AS iStart,
                DATEADD(SECOND, DATEDIFF(SECOND, '00:00:00', ab.�����������������), i.���������) AS iEnd
        ) AS cur
        JOIN Shedule_playlists sp ON sp.IsDeleted = 0
        JOIN Playlist p ON sp.������������ = p.������������
        CROSS APPLY (
            SELECT 
                sp.��������� AS spStart,
                DATEADD(SECOND, DATEDIFF(SECOND, '00:00:00', p.�����������������), sp.���������) AS spEnd
        ) AS oth
        WHERE 
            cur.iStart < oth.spEnd
            AND cur.iEnd   > oth.spStart
    )
    BEGIN
        RAISERROR('���������� ���������� ����� ������������ � �������� ����������.', 16, 1);
        ROLLBACK TRANSACTION;
        RETURN;
    END;

    -- �������� ������� ���� �� ������ ��������� ���������� �����
    IF NOT EXISTS (
        SELECT *
        FROM Shedule_ads_block
        WHERE IsDeleted = 0
    )
    BEGIN
        RAISERROR('� ���������� ��������� ������ ������ ���� ��� ������� ���� �������� ��������� ����.', 16, 1);
        ROLLBACK TRANSACTION;
        RETURN;
    END;
END
GO

ALTER TRIGGER trg_Ad_block_UpdateDuration_NoOverlap
ON Ad_block
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    IF UPDATE(�����������������)
    BEGIN
        /*
            1. �������� ����� ������������ i.����������������� ��� ���������� ����� ab (i.�������� = ab.��������).
            2. ������� ���������� Shedule_ads_block (sab), � ������� sab.�������� = i.��������.
            3. ��������� �������� [sabStart, sabEnd) = [sab.���������, sab.��������� + i.�����������������].
            4. ���������� c ������� ���������� ������� (sab2) � � ����������� (sp).
        */

        ----------------------------------------------------------------------
        -- 3.1. �������� ����������� � ������� ���������� �������
        ----------------------------------------------------------------------
        IF EXISTS (
            SELECT *
            FROM Shedule_ads_block sab
            JOIN inserted i ON sab.�������� = i.��������
                           AND sab.IsDeleted = 0
            CROSS APPLY (
                SELECT 
                    sab.��������� AS iStart,
                    DATEADD(SECOND, DATEDIFF(SECOND, '00:00:00', i.�����������������), sab.���������) AS iEnd
            ) AS cur
            JOIN Shedule_ads_block sab2 ON sab2.IsDeleted = 0
                                       AND sab2.��������� <> sab.���������
            JOIN Ad_block ab2 ON ab2.�������� = sab2.��������
                              AND ab2.IsDeleted = 0
            CROSS APPLY (
                SELECT 
                    sab2.��������� AS sab2Start,
                    DATEADD(SECOND, DATEDIFF(SECOND, '00:00:00', ab2.�����������������), sab2.���������) AS sab2End
            ) AS oth
            WHERE
                cur.iStart < oth.sab2End
                AND cur.iEnd   > oth.sab2Start
        )
        BEGIN
            RAISERROR('��������� ������������ ���������� ����� �������� ���������� � ������ ��������� ������.', 16, 1);
            ROLLBACK TRANSACTION;
            RETURN;
        END;

        ----------------------------------------------------------------------
        -- 3.2. �������� ����������� � �����������
        ----------------------------------------------------------------------
        IF EXISTS (
            SELECT *
            FROM Shedule_ads_block sab
            JOIN inserted i ON sab.�������� = i.��������
                           AND sab.IsDeleted = 0
            CROSS APPLY (
                SELECT 
                    sab.��������� AS iStart,
                    DATEADD(SECOND, DATEDIFF(SECOND, '00:00:00', i.�����������������), sab.���������) AS iEnd
            ) AS cur
            JOIN Shedule_playlists sp ON sp.IsDeleted = 0
            JOIN Playlist p ON p.������������ = sp.������������
                            AND p.IsDeleted = 0
            CROSS APPLY (
                SELECT 
                    sp.��������� AS spStart,
                    DATEADD(SECOND, DATEDIFF(SECOND, '00:00:00', p.�����������������), sp.���������) AS spEnd
            ) AS oth
            WHERE
                cur.iStart < oth.spEnd
                AND cur.iEnd   > oth.spStart
        )
        BEGIN
            RAISERROR('��������� ������������ ���������� ����� �������� ���������� � ����������.', 16, 1);
            ROLLBACK TRANSACTION;
            RETURN;
        END;
    END
END
GO
ALTER TRIGGER trg_Playlist_UpdateDuration_NoOverlap
ON Playlist
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    IF UPDATE(�����������������)
    BEGIN
        /*
            1. �������� �� inserted ����� �������� ������������ (i.�����������������)
               ��� ��������� p (i.������������ = p.������������).
            2. ��������� �������� [iStart, iEnd) = [sp.���������, sp.��������� + i.�����������������]
               ��� ������� ���������� Shedule_playlists (sp) ������������ �� ��������� ��������.
            3. ��������� ����������� � ������� ����������� sp2 � � ���������� ������� sab.
        */

        ----------------------------------------------------------------------
        -- 2.1. �������� ����������� � ������� �����������
        ----------------------------------------------------------------------
        IF EXISTS (
            SELECT *
            FROM Shedule_playlists sp
            JOIN inserted i ON sp.������������ = i.������������
                           AND sp.IsDeleted = 0
            -- �������� "�����" �������� [iStart, iEnd)
            CROSS APPLY (
                SELECT 
                    sp.��������� AS iStart,
                    DATEADD(SECOND, DATEDIFF(SECOND, '00:00:00', i.�����������������), sp.���������) AS iEnd
            ) AS cur
            -- ������� ������ ��������� sp2 (IsDeleted=0, ������ ���������)
            JOIN Shedule_playlists sp2 ON sp2.IsDeleted = 0
                                       AND sp2.��������� <> sp.���������
            JOIN Playlist p2 ON p2.������������ = sp2.������������
                             AND p2.IsDeleted = 0
            CROSS APPLY (
                SELECT
                    sp2.��������� AS sp2Start,
                    DATEADD(SECOND, DATEDIFF(SECOND, '00:00:00', p2.�����������������), sp2.���������) AS sp2End
            ) AS oth
            WHERE 
                -- ������� ������� ����������:
                cur.iStart < oth.sp2End
                AND cur.iEnd   > oth.sp2Start
        )
        BEGIN
            RAISERROR('��������� ������������ ��������� �������� ���������� � ������ ����������.', 16, 1);
            ROLLBACK TRANSACTION;
            RETURN;
        END;

        ----------------------------------------------------------------------
        -- 2.2. �������� ����������� � ���������� �������
        ----------------------------------------------------------------------
        IF EXISTS (
            SELECT *
            FROM Shedule_playlists sp
            JOIN inserted i ON sp.������������ = i.������������
                           AND sp.IsDeleted = 0
            CROSS APPLY (
                SELECT 
                    sp.��������� AS iStart,
                    DATEADD(SECOND, DATEDIFF(SECOND, '00:00:00', i.�����������������), sp.���������) AS iEnd
            ) AS cur
            JOIN Shedule_ads_block sab ON sab.IsDeleted = 0
            JOIN Ad_block ab ON ab.�������� = sab.��������
                            AND ab.IsDeleted = 0
            CROSS APPLY (
                SELECT 
                    sab.��������� AS sabStart,
                    DATEADD(SECOND, DATEDIFF(SECOND, '00:00:00', ab.�����������������), sab.���������) AS sabEnd
            ) AS oth
            WHERE
                cur.iStart < oth.sabEnd
                AND cur.iEnd   > oth.sabStart
        )
        BEGIN
            RAISERROR('��������� ������������ ��������� �������� ���������� � ��������� ������.', 16, 1);
            ROLLBACK TRANSACTION;
            RETURN;
        END;
    END
END
GO