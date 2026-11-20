-- Global11_demo: первичная выгрузка DMRS repository
-- Запуск: Oracle SQL Developer -> Open -> Run Script (F5).
-- Перед запуском проверьте подключение к схеме-владельцу DMRS repository.
-- Скрипт только читает данные. Он не выполняет INSERT/UPDATE/DELETE/DDL.
--
-- Шаг 1: найдите Design_OVID в результате первого запроса.
-- Шаг 2: вставьте найденный OVID в DEFINE DESIGN_OVID ниже.
-- Шаг 3: выполните скрипт повторно (или выделите блоки после DEFINE).

SET PAGESIZE 50000
SET LINESIZE 32767
SET LONG 2000000
SET LONGCHUNKSIZE 2000000
SET TRIMSPOOL ON
SET TAB OFF
SET FEEDBACK ON
SET VERIFY OFF

PROMPT ============================================================
PROMPT 1. Поиск дизайна Global11_demo
PROMPT ============================================================

SELECT *
FROM DMRS_DESIGNS
WHERE UPPER(DESIGN_NAME) = UPPER('global11_demo');

-- ВСТАВЬТЕ OVID найденной строки вместо значения-заполнителя:
DEFINE DESIGN_OVID = 'PASTE_DESIGN_OVID_HERE'

PROMPT ============================================================
PROMPT 2. Модели выбранного дизайна
PROMPT ============================================================

SELECT *
FROM DMRS_MODELS
WHERE DESIGN_OVID = '&DESIGN_OVID'
ORDER BY MODEL_NAME;

PROMPT ============================================================
PROMPT 3. Логические сущности
PROMPT ============================================================

SELECT *
FROM DMRS_ENTITIES
WHERE DESIGN_OVID = '&DESIGN_OVID'
ORDER BY ENTITY_NAME;

PROMPT ============================================================
PROMPT 4. Атрибуты сущностей
PROMPT ============================================================

SELECT *
FROM DMRS_ATTRIBUTES
WHERE DESIGN_OVID = '&DESIGN_OVID'
ORDER BY CONTAINER_OVID, SEQUENCE, ATTRIBUTE_NAME;

PROMPT ============================================================
PROMPT 5. Отношения
PROMPT ============================================================

SELECT *
FROM DMRS_RELATIONSHIPS
WHERE DESIGN_OVID = '&DESIGN_OVID'
ORDER BY RELATIONSHIP_NAME;

PROMPT ============================================================
PROMPT 6. Ключи
PROMPT ============================================================

SELECT *
FROM DMRS_KEYS
WHERE DESIGN_OVID = '&DESIGN_OVID'
ORDER BY CONTAINER_OVID;

PROMPT ============================================================
PROMPT 7. Атрибуты ключей
PROMPT ============================================================

SELECT *
FROM DMRS_KEY_ATTRIBUTES
WHERE DESIGN_OVID = '&DESIGN_OVID'
ORDER BY KEY_OVID;

PROMPT ============================================================
PROMPT 8. Элементы ключей
PROMPT ============================================================

SELECT *
FROM DMRS_KEY_ELEMENTS
WHERE DESIGN_OVID = '&DESIGN_OVID'
ORDER BY KEY_OVID, SEQUENCE;

PROMPT ============================================================
PROMPT Конец выборки.
PROMPT Если запрос сообщает об отсутствующем столбце, не угадывайте имя:
PROMPT пришлите текст ошибки и результат описания таблицы из USER_TAB_COLUMNS.
