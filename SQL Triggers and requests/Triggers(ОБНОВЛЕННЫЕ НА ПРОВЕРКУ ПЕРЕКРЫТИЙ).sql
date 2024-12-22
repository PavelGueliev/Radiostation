ALTER TRIGGER trg_Shedule_playlists_NoOverlap
ON Shedule_playlists
AFTER INSERT, UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    /*
      Логика проверки пересечения (Overlap):
      Два интервала (startA, endA) и (startB, endB) пересекаются, если
         (startA < endB) AND (endA > startB).
    */

    -- Проверка, что вставленный/обновлённый плейлист 
    -- не пересекается с другими активными плейлистами
    IF EXISTS (
        SELECT *
        FROM inserted i
        JOIN Playlist p ON i.КодПлейлиста = p.КодПлейлиста
        CROSS APPLY (
            SELECT 
                i.ДатаВремя AS iStart,
                DATEADD(SECOND, DATEDIFF(SECOND, '00:00:00', p.Продолжительность), i.ДатаВремя) AS iEnd
        ) AS cur
        JOIN Shedule_playlists sp ON sp.IsDeleted = 0 
                                  AND sp.КодЗаписи <> i.КодЗаписи
        JOIN Playlist p2 ON sp.КодПлейлиста = p2.КодПлейлиста
        CROSS APPLY (
            SELECT 
                sp.ДатаВремя AS spStart,
                DATEADD(SECOND, DATEDIFF(SECOND, '00:00:00', p2.Продолжительность), sp.ДатаВремя) AS spEnd
        ) AS oth
        WHERE 
            cur.iStart < oth.spEnd
            AND cur.iEnd   > oth.spStart
    )
    BEGIN
        RAISERROR('Расписание плейлиста пересекается с другим активным плейлистом.', 16, 1);
        ROLLBACK TRANSACTION;
        RETURN;
    END;

    -- Проверка, что вставленный/обновлённый плейлист 
    -- не пересекается с активными расписаниями рекламных блоков
    IF EXISTS (
        SELECT *
        FROM inserted i
        JOIN Playlist p ON i.КодПлейлиста = p.КодПлейлиста
        CROSS APPLY (
            SELECT 
                i.ДатаВремя AS iStart,
                DATEADD(SECOND, DATEDIFF(SECOND, '00:00:00', p.Продолжительность), i.ДатаВремя) AS iEnd
        ) AS cur
        JOIN Shedule_ads_block sab ON sab.IsDeleted = 0
        JOIN Ad_block ab ON sab.КодБлока = ab.КодБлока
        CROSS APPLY (
            SELECT 
                sab.ДатаВремя AS sabStart,
                DATEADD(SECOND, DATEDIFF(SECOND, '00:00:00', ab.Продолжительность), sab.ДатаВремя) AS sabEnd
        ) AS oth
        WHERE 
            cur.iStart < oth.sabEnd
            AND cur.iEnd   > oth.sabStart
    )
    BEGIN
        RAISERROR('Расписание плейлиста пересекается с активным рекламным блоком.', 16, 1);
        ROLLBACK TRANSACTION;
        RETURN;
    END;

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
    END;
END
GO
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
        JOIN Ad_block ab ON i.КодБлока = ab.КодБлока
        CROSS APPLY (
            SELECT 
                i.ДатаВремя AS iStart,
                DATEADD(SECOND, DATEDIFF(SECOND, '00:00:00', ab.Продолжительность), i.ДатаВремя) AS iEnd
        ) AS cur
        JOIN Shedule_ads_block sab ON sab.IsDeleted = 0
                                   AND sab.КодЗаписи <> i.КодЗаписи
        JOIN Ad_block ab2 ON sab.КодБлока = ab2.КодБлока
        CROSS APPLY (
            SELECT 
                sab.ДатаВремя AS sabStart,
                DATEADD(SECOND, DATEDIFF(SECOND, '00:00:00', ab2.Продолжительность), sab.ДатаВремя) AS sabEnd
        ) AS oth
        WHERE 
            cur.iStart < oth.sabEnd
            AND cur.iEnd   > oth.sabStart
    )
    BEGIN
        RAISERROR('Расписание рекламного блока пересекается с другим активным рекламным блоком.', 16, 1);
        ROLLBACK TRANSACTION;
        RETURN;
    END;

    -- Проверка перекрытия с активными расписаниями плейлистов
    IF EXISTS (
        SELECT *
        FROM inserted i
        JOIN Ad_block ab ON i.КодБлока = ab.КодБлока
        CROSS APPLY (
            SELECT 
                i.ДатаВремя AS iStart,
                DATEADD(SECOND, DATEDIFF(SECOND, '00:00:00', ab.Продолжительность), i.ДатаВремя) AS iEnd
        ) AS cur
        JOIN Shedule_playlists sp ON sp.IsDeleted = 0
        JOIN Playlist p ON sp.КодПлейлиста = p.КодПлейлиста
        CROSS APPLY (
            SELECT 
                sp.ДатаВремя AS spStart,
                DATEADD(SECOND, DATEDIFF(SECOND, '00:00:00', p.Продолжительность), sp.ДатаВремя) AS spEnd
        ) AS oth
        WHERE 
            cur.iStart < oth.spEnd
            AND cur.iEnd   > oth.spStart
    )
    BEGIN
        RAISERROR('Расписание рекламного блока пересекается с активным плейлистом.', 16, 1);
        ROLLBACK TRANSACTION;
        RETURN;
    END;

    -- Проверка наличия хотя бы одного активного рекламного блока
    IF NOT EXISTS (
        SELECT *
        FROM Shedule_ads_block
        WHERE IsDeleted = 0
    )
    BEGIN
        RAISERROR('В расписании рекламных блоков должен быть как минимум один активный рекламный блок.', 16, 1);
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

    IF UPDATE(Продолжительность)
    BEGIN
        /*
            1. Получаем новую длительность i.Продолжительность для рекламного блока ab (i.КодБлока = ab.КодБлока).
            2. Находим расписания Shedule_ads_block (sab), у которых sab.КодБлока = i.КодБлока.
            3. Вычисляем интервал [sabStart, sabEnd) = [sab.ДатаВремя, sab.ДатаВремя + i.Продолжительность].
            4. Сравниваем c другими рекламными блоками (sab2) и с плейлистами (sp).
        */

        ----------------------------------------------------------------------
        -- 3.1. Проверка пересечения с другими рекламными блоками
        ----------------------------------------------------------------------
        IF EXISTS (
            SELECT *
            FROM Shedule_ads_block sab
            JOIN inserted i ON sab.КодБлока = i.КодБлока
                           AND sab.IsDeleted = 0
            CROSS APPLY (
                SELECT 
                    sab.ДатаВремя AS iStart,
                    DATEADD(SECOND, DATEDIFF(SECOND, '00:00:00', i.Продолжительность), sab.ДатаВремя) AS iEnd
            ) AS cur
            JOIN Shedule_ads_block sab2 ON sab2.IsDeleted = 0
                                       AND sab2.КодЗаписи <> sab.КодЗаписи
            JOIN Ad_block ab2 ON ab2.КодБлока = sab2.КодБлока
                              AND ab2.IsDeleted = 0
            CROSS APPLY (
                SELECT 
                    sab2.ДатаВремя AS sab2Start,
                    DATEADD(SECOND, DATEDIFF(SECOND, '00:00:00', ab2.Продолжительность), sab2.ДатаВремя) AS sab2End
            ) AS oth
            WHERE
                cur.iStart < oth.sab2End
                AND cur.iEnd   > oth.sab2Start
        )
        BEGIN
            RAISERROR('Изменение длительности рекламного блока вызывает перекрытие с другим рекламным блоком.', 16, 1);
            ROLLBACK TRANSACTION;
            RETURN;
        END;

        ----------------------------------------------------------------------
        -- 3.2. Проверка пересечения с плейлистами
        ----------------------------------------------------------------------
        IF EXISTS (
            SELECT *
            FROM Shedule_ads_block sab
            JOIN inserted i ON sab.КодБлока = i.КодБлока
                           AND sab.IsDeleted = 0
            CROSS APPLY (
                SELECT 
                    sab.ДатаВремя AS iStart,
                    DATEADD(SECOND, DATEDIFF(SECOND, '00:00:00', i.Продолжительность), sab.ДатаВремя) AS iEnd
            ) AS cur
            JOIN Shedule_playlists sp ON sp.IsDeleted = 0
            JOIN Playlist p ON p.КодПлейлиста = sp.КодПлейлиста
                            AND p.IsDeleted = 0
            CROSS APPLY (
                SELECT 
                    sp.ДатаВремя AS spStart,
                    DATEADD(SECOND, DATEDIFF(SECOND, '00:00:00', p.Продолжительность), sp.ДатаВремя) AS spEnd
            ) AS oth
            WHERE
                cur.iStart < oth.spEnd
                AND cur.iEnd   > oth.spStart
        )
        BEGIN
            RAISERROR('Изменение длительности рекламного блока вызывает перекрытие с плейлистом.', 16, 1);
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

    IF UPDATE(Продолжительность)
    BEGIN
        /*
            1. Получаем из inserted новые значения длительности (i.Продолжительность)
               для плейлиста p (i.КодПлейлиста = p.КодПлейлиста).
            2. Вычисляем интервал [iStart, iEnd) = [sp.ДатаВремя, sp.ДатаВремя + i.Продолжительность]
               для каждого расписания Shedule_playlists (sp) ссылающегося на изменённый плейлист.
            3. Проверяем пересечение с другими плейлистами sp2 и с рекламными блоками sab.
        */

        ----------------------------------------------------------------------
        -- 2.1. Проверка пересечения с другими плейлистами
        ----------------------------------------------------------------------
        IF EXISTS (
            SELECT *
            FROM Shedule_playlists sp
            JOIN inserted i ON sp.КодПлейлиста = i.КодПлейлиста
                           AND sp.IsDeleted = 0
            -- Получаем "новый" интервал [iStart, iEnd)
            CROSS APPLY (
                SELECT 
                    sp.ДатаВремя AS iStart,
                    DATEADD(SECOND, DATEDIFF(SECOND, '00:00:00', i.Продолжительность), sp.ДатаВремя) AS iEnd
            ) AS cur
            -- Находим ДРУГИЕ плейлисты sp2 (IsDeleted=0, другой КодЗаписи)
            JOIN Shedule_playlists sp2 ON sp2.IsDeleted = 0
                                       AND sp2.КодЗаписи <> sp.КодЗаписи
            JOIN Playlist p2 ON p2.КодПлейлиста = sp2.КодПлейлиста
                             AND p2.IsDeleted = 0
            CROSS APPLY (
                SELECT
                    sp2.ДатаВремя AS sp2Start,
                    DATEADD(SECOND, DATEDIFF(SECOND, '00:00:00', p2.Продолжительность), sp2.ДатаВремя) AS sp2End
            ) AS oth
            WHERE 
                -- Условие нахлёста интервалов:
                cur.iStart < oth.sp2End
                AND cur.iEnd   > oth.sp2Start
        )
        BEGIN
            RAISERROR('Изменение длительности плейлиста вызывает перекрытие с другим плейлистом.', 16, 1);
            ROLLBACK TRANSACTION;
            RETURN;
        END;

        ----------------------------------------------------------------------
        -- 2.2. Проверка пересечения с рекламными блоками
        ----------------------------------------------------------------------
        IF EXISTS (
            SELECT *
            FROM Shedule_playlists sp
            JOIN inserted i ON sp.КодПлейлиста = i.КодПлейлиста
                           AND sp.IsDeleted = 0
            CROSS APPLY (
                SELECT 
                    sp.ДатаВремя AS iStart,
                    DATEADD(SECOND, DATEDIFF(SECOND, '00:00:00', i.Продолжительность), sp.ДатаВремя) AS iEnd
            ) AS cur
            JOIN Shedule_ads_block sab ON sab.IsDeleted = 0
            JOIN Ad_block ab ON ab.КодБлока = sab.КодБлока
                            AND ab.IsDeleted = 0
            CROSS APPLY (
                SELECT 
                    sab.ДатаВремя AS sabStart,
                    DATEADD(SECOND, DATEDIFF(SECOND, '00:00:00', ab.Продолжительность), sab.ДатаВремя) AS sabEnd
            ) AS oth
            WHERE
                cur.iStart < oth.sabEnd
                AND cur.iEnd   > oth.sabStart
        )
        BEGIN
            RAISERROR('Изменение длительности плейлиста вызывает перекрытие с рекламным блоком.', 16, 1);
            ROLLBACK TRANSACTION;
            RETURN;
        END;
    END
END
GO