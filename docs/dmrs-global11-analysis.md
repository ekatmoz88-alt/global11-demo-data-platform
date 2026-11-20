# DMRS-Global11

Документация и результаты анализа демонстрационного проекта **Global11_demo** для Oracle SQL Developer Data Modeler (DMRS).

> DMRS / Global11_demo — отдельная аналитическая линия проекта Global11. Материалы АРМ «Дешифратор» и ППО «Территория» здесь не размещаются.

## Назначение

Global11_demo рассматривается как демонстрационный проект Oracle Data Modeler, связанный с импортом демонстрационной схемы Oracle GLOBAL и построением многомерной модели (dimensional modeling / OLAP).

В предполагаемый контур анализа входят:

- импорт реляционной схемы;
- преобразование relational model в logical model;
- измерения, иерархии и уровни, включая ragged hierarchies;
- dimension usage и cubes;
- меры и вычисляемые атрибуты;
- экспорт модели в Oracle Analytic Workspace (AW).

## Что известно из переданных материалов

### Метаданные дизайна

| Поле | Значение |
|---|---|
| Design name | `global11_demo` |
| Design ID / OVID | `FA57AA9E-F83B-BFE7-9230-484B711B3059` |
| Версия | `3.5` |
| Создатель | `PSTOYANO` |
| Время создания | `2012-12-12 04:59:41 UTC` |
| Owner design name | `global11` |

Дополнительные объекты из XML:

- `DataTypesDesign`: `main_view`, `visible=false`, Object ID `978818BA-B123-2AF7-3834-1EEFC9320E49`.
- `ProcessModel`: ID `9B20847C-D845-FE55-CBE7-3AEB7C04958B`, создан `2012-12-12 04:59:42 UTC`, owner design `global11`, `shouldBeOpen=false`.

### Пример связи

Для связи `UNITS_FACT_FK_4` переданы следующие атрибуты:

- OVID: `54BAFD65-376C-DE62-D143-C536F4DFE52B`;
- source PK OVID: `0AD26B2A-BDDA-4F93-4113-2011BF954C4A`;
- source entity OVID: `9B0FB279-0E43-18D3-9009-32CB750CC625`;
- target entity OVID: `61059591-E00D-F325-75BD-1580E94E5E84`;
- identifying: `true`;
- optional source: `true`;
- optional target: `false`;
- source cardinality: `1`;
- target cardinality: `*`;
- transferable: `true`;
- delete rule: `NO ACTION`.

Имена сущностей для этих OVID пока не подтверждены записями сущностей, поэтому здесь они намеренно не подставлены.

## Структура DMRS-метаданных, охваченная анализом

Переданы определения (DDL/описания структуры) следующих групп таблиц:

- дизайн и модели: `DMRS_DESIGNS`, `DMRS_MODELS`;
- логическая модель: `DMRS_ENTITIES`, `DMRS_ATTRIBUTES`, `DMRS_RELATIONSHIPS`, `DMRS_KEYS`, `DMRS_KEY_ATTRIBUTES`, `DMRS_KEY_ELEMENTS`;
- реляционная модель: `DMRS_TABLES`, `DMRS_COLUMNS`, `DMRS_FOREIGNKEYS`, `DMRS_INDEXES`, `DMRS_TABLE_CONSTRAINTS`;
- соответствия моделей: `DMRS_MAPPINGS`;
- диаграммы: `DMRS_DIAGRAMS`, `DMRS_DIAGRAM_ELEMENTS`;
- домены и типы: `DMRS_DOMAINS`, `DMRS_DOMAIN_AVT`, `DMRS_DOMAIN_CHECK_CONSTRAINTS`, `DMRS_DOMAIN_VALUE_RANGES`, `DMRS_LOGICAL_TYPES`, `DMRS_LOGICAL_TO_NATIVE`, `DMRS_NATIVE_TO_LOGICAL`;
- проверки и допустимые значения: `DMRS_CHECK_CONSTRAINTS`, `DMRS_VALUE_RANGES`, `DMRS_AVT`;
- структурированные, distinct и collection types: `DMRS_STRUCTURED_TYPES`, `DMRS_STRUCT_TYPE_ATTRS`, `DMRS_STRUCT_TYPE_METHODS`, `DMRS_STRUCT_TYPE_METHOD_PARS`, `DMRS_DISTINCT_TYPES`, `DMRS_COLLECTION_TYPES`;
- процессы и документация: `DMRS_PROCESS_ENTITIES`, `DMRS_PROCESS_ATTRIBUTES`, `DMRS_GLOSSARIES`, `DMRS_GLOSSARY_TERMS`, `DMRS_NOTES`, `DMRS_Large_Text`.

Это перечень структур, а не подтверждение наличия или полноты данных в таблицах.

## Ограничения доказательной базы

На текущем этапе в переписке были переданы преимущественно определения таблиц, отдельные XML-фрагменты и атрибуты связей. **Фактическая модель Global11_demo ещё не восстановлена.**

Не следует считать подтверждёнными без выгрузки строк из DMRS:

- полный состав сущностей, таблиц и атрибутов;
- имена объектов, на которые ссылаются OVID;
- полный набор связей, ключей и ограничений;
- состав диаграмм и их графические элементы;
- кубы, измерения, иерархии и меры;
- соответствие логической и реляционной моделей.

DDL описывает структуру репозитория метаданных, но не заменяет данные конкретного дизайна.

## Рекомендуемый порядок извлечения

1. Найти дизайн `global11_demo` в `DMRS_DESIGNS` и подтвердить его OVID.
2. Получить связанные модели из `DMRS_MODELS`.
3. Выгрузить логические сущности, атрибуты, отношения и ключи.
4. Выгрузить реляционные таблицы, колонки, внешние ключи и индексы.
5. Проверить связи logical-to-relational через `DMRS_MAPPINGS`.
6. Получить диаграммы и элементы диаграмм.
7. При необходимости извлечь процессы, словари, заметки, домены и типы.

### Базовые запросы для выгрузки

Список дизайнов:

```sql
SELECT Design_ID, Design_OVID, Design_Name, Date_Published,
       Published_By, Persistence_Version, Version_Comments
FROM DMRS_DESIGNS
WHERE UPPER(Design_Name) = UPPER('global11_demo');
```

Модели найденного дизайна:

```sql
SELECT Design_OVID, Model_ID, Model_OVID, Model_Name, Model_Type, RDBMS_Type
FROM DMRS_MODELS
WHERE Design_OVID = :design_ovid
ORDER BY Model_Name;
```

Диаграммы дизайна:

```sql
SELECT Diagram_Name, OVID, Diagram_Type, Model_Name, Notation,
       Visible, Is_Display, Master_Diagram_OVID, Diagram_SVG
FROM DMRS_DIAGRAMS
WHERE Design_OVID = :design_ovid
ORDER BY Diagram_Name;
```

Элементы диаграмм (после определения OVID диаграммы):

```sql
SELECT Name, Type, Geometry_Type, Object_ID, OVID, View_ID,
       Source_ID, Source_OVID, Target_ID, Target_OVID,
       X, Y, Height, Width, Diagram_OVID
FROM DMRS_DIAGRAM_ELEMENTS
WHERE Diagram_OVID = :diagram_ovid
ORDER BY Name;
```

Проверки и ограничения:

```sql
SELECT DataElement_Name, Container_Name, Constraint_Name,
       Text, Database_Type, Sequence
FROM DMRS_CHECK_CONSTRAINTS
WHERE Design_OVID = :design_ovid
ORDER BY Container_Name, DataElement_Name, Sequence;
```

Перед выполнением запросов следует сверить имена столбцов с фактической версией DMRS repository.

## Статус

- [x] Создана отдельная документационная площадка.
- [x] Зафиксированы известные метаданные дизайна и пример связи.
- [x] Составлен перечень исследованных групп DMRS-таблиц.
- [ ] Подтверждён состав моделей и объектов по фактическим строкам.
- [ ] Восстановлены логическая и реляционная модели.
- [ ] Разобраны диаграммы и OLAP-объекты.
- [ ] Подготовлена проверенная итоговая схема Global11_demo.

## Принцип ведения документации

Каждый вывод должен иметь происхождение: XML-фрагмент, строка DMRS, DDL, экспорт Data Modeler или явно обозначенная аналитическая гипотеза. Предположения не выдаются за факты. Идентификаторы OVID сохраняются без переименования.
