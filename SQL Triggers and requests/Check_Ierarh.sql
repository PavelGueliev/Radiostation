SELECT 
    t.name AS TableName,
    c.name AS ColumnName,
    c.column_id AS ColumnID,
    tp.name AS DataType,
    c.max_length AS MaxLength,
    c.is_nullable AS IsNullable,
    c.is_identity AS IsIdentity,
    c.precision AS Precision,
    c.scale AS Scale
FROM 
    sys.tables t
INNER JOIN 
    sys.columns c ON t.object_id = c.object_id
INNER JOIN 
    sys.types tp ON c.user_type_id = tp.user_type_id
WHERE 
    t.is_ms_shipped = 0  -- Исключает системные таблицы
ORDER BY 
    t.name, c.column_id;