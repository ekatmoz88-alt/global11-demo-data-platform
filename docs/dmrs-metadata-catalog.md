# Каталог метаданных DMRS

Документ систематизирует определения DMRS-таблиц, переданные для анализа проекта Global11_demo. Это каталог назначения и связей на уровне доступных описаний структуры, а не выгрузка фактических строк.

## 1. Дизайн и модели

### `DMRS_DESIGNS`

Реестр дизайнов. В переданном описании указаны:

- `Design_ID`, `Design_OVID` — идентификаторы дизайна;
- `Design_Name` — имя дизайна;
- `Date_Published`, `Published_By` — сведения о публикации;
- `Persistence_Version`, `Version_Comments` — версия сохранения и комментарии.

Для Global11_demo сначала необходимо получить строку дизайна и использовать её OVID как контекст последующих выборок.

### `DMRS_MODELS`

Состав моделей внутри дизайна:

- `Design_OVID`;
- `Model_ID`, `Model_OVID`;
- `Model_Name`, `Model_Type`;
- `RDBMS_Type`.

Связь с дизайном следует проверять по фактическим значениям OVID.

## 2. Логическая модель

### `DMRS_ENTITIES`

Описание сущностей логической модели. В переданном перечне полей присутствуют имя сущности, Object_ID/OVID, Model_OVID, классификация, число элементов, характеристики объёма и роста, нормальная форма, ссылки на supertype entities, стратегия engineering, owner/source.

### `DMRS_ATTRIBUTES`

Атрибуты сущностей. Указаны, в частности:

- идентификация и контейнер: `Attribute_Name`, `OVID`, `Container_OVID`;
- обязательность и тип: `Mandatory`, `DataType_Kind`, `Value_Type`;
- вычисление и область: `Formula`, `ScopeEntity`;
- типовые ссылки: `Domain_OVID`, `Logical_Type_OVID`, `Distinct_Type_OVID`, `Structured_Type_OVID`, `Collection_Type_OVID`;
- ограничения и значения по умолчанию: `Check_Constraint_Name`, `Default_Value`, `Use_Domain_Constraint`;
- сведения о связи: `Relationship_OVID`, `Relationship_Name`;
- признаки PK/FK, последовательность и характеристики типа.

Точный смысл каждого поля и его допустимые значения необходимо подтвердить данными конкретной версии репозитория.

### `DMRS_RELATIONSHIPS`

Метаданные отношений между сущностями. В описании присутствуют имя отношения, имена и OVID source/target entities, кардинальности и optionality обеих сторон, identifying, role, число атрибутов, transferable и ссылки на arc.

### Ключи

- `DMRS_KEYS` — метаданные ключей.
- `DMRS_KEY_ATTRIBUTES` — членство атрибутов в ключах.
- `DMRS_KEY_ELEMENTS` — элементы ключей и их порядок.

Для восстановления ключей важно сохранять порядок элементов, а не только множество атрибутов.

## 3. Реляционная модель

### `DMRS_TABLES`

Метаданные таблиц реляционной модели, включая идентификаторы и OVID, имя таблицы, модель, а также доступные в описании характеристики таблицы.

### `DMRS_COLUMNS`

Метаданные столбцов. Переданный перечень включает:

- имя, OVID, контейнер и обязательность;
- тип данных, native type, размер, precision и scale;
- вычисляемость и формулу;
- ссылки на домены и логические/distinct/structured/collection types;
- check constraint и default;
- имя таблицы и признаки PK/FK;
- последовательность, сведения о модели и Design_OVID;
- признаки PII/Sensitive/Mask и сведения об autoincrement.

Наличие флага PII или Sensitive в структуре не означает, что он установлен для конкретного столбца: это устанавливается только по строкам.

### Ограничения и индексы

- `DMRS_FOREIGNKEYS` — метаданные внешних ключей.
- `DMRS_INDEXES` — индексы.
- `DMRS_CONSTR_INDEX_COLUMNS` — столбцы ограничений/индексов и их порядок.
- `DMRS_TABLE_CONSTRAINTS` — ограничения таблиц.

Состав и точную семантику FK следует подтверждать строками соответствующих таблиц и сверять с DDL исходной схемы.

## 4. Соответствия моделей

### `DMRS_MAPPINGS`

Переданное описание содержит идентификаторы, OVID, имена и типы объектов логической и реляционной моделей. Таблица используется для анализа соответствия logical entity ↔ relational table и других сопоставлений.

Не следует выводить соответствие только по совпадению имён: первичным доказательством должны быть идентификаторы и записи mappings.

## 5. Диаграммы

### `DMRS_DIAGRAMS`

В описании присутствуют:

- имя, Object_ID, OVID и тип диаграммы;
- признаки отображения и видимости;
- ссылки на master diagram и модель;
- сведения о subview/display;
- notation и параметры отображения;
- autoroute, box-in-box;
- `Diagram_SVG` и `Diagram_PDF`;
- `Design_OVID`.

### `DMRS_DIAGRAM_ELEMENTS`

Описаны элементы диаграмм: имя, тип, geometry type, Object_ID/OVID, view/source/target IDs и OVID, model IDs/OVID, координаты X/Y, высота/ширина, цвета, formatting, points, Diagram_OVID и имена источника/цели.

Для восстановления изображения диаграммы нужно связать элементы с диаграммой и объектами модели. Координаты и стили нельзя интерпретировать без проверки конкретных строк и формата поля `Points`.

## 6. Домены, типы и ограничения значений

### Домены

- `DMRS_DOMAINS` — домены;
- `DMRS_DOMAIN_AVT` — допустимые значения доменов;
- `DMRS_DOMAIN_CHECK_CONSTRAINTS` — проверки доменов;
- `DMRS_DOMAIN_VALUE_RANGES` — диапазоны значений доменов.

### Типы

- `DMRS_LOGICAL_TYPES` — логические типы;
- `DMRS_LOGICAL_TO_NATIVE` и `DMRS_NATIVE_TO_LOGICAL` — сопоставления логических и native types;
- `DMRS_DISTINCT_TYPES` — distinct types;
- `DMRS_STRUCTURED_TYPES` — structured types;
- `DMRS_STRUCT_TYPE_ATTRS` — атрибуты structured types;
- `DMRS_STRUCT_TYPE_METHODS` — методы;
- `DMRS_STRUCT_TYPE_METHOD_PARS` — параметры методов;
- `DMRS_COLLECTION_TYPES` — collection types.

В описании structured types присутствуют ссылки на supertype, признаки predefined/final/instantiable, атрибуты, методы, конструкторы, переопределённые методы и параметры.

### Проверки и допустимые значения

- `DMRS_CHECK_CONSTRAINTS`: DataElement_ID/OVID, Type, Sequence, Constraint_Name, Text, Database_Type, имена контейнера и элемента, Design_OVID.
- `DMRS_VALUE_RANGES`: DataElement_ID/OVID, Type, Sequence, Begin_Value, End_Value, Short_Description, Container_ID/OVID/Name, DataElement_Name, Design_OVID.
- `DMRS_AVT`: DataElement_ID/OVID, Type, Sequence, Value, Short_Description, имена контейнера и элемента, Design_OVID.

Поля `Type` и правила интерпретации последовательностей следует выяснять по фактическим данным, не предполагая заранее их кодировку.

## 7. Процессы и текстовая документация

- `DMRS_PROCESS_ENTITIES` — сущности процесса и их связи с модельными объектами;
- `DMRS_PROCESS_ATTRIBUTES` — атрибуты процессов и признаки CRUD;
- `DMRS_GLOSSARIES` — глоссарии;
- `DMRS_GLOSSARY_TERMS` — термины;
- `DMRS_NOTES` — заметки;
- `DMRS_Large_Text` — большие текстовые значения.

Связи процесса с сущностями и атрибутами потенциально позволяют сформировать CRUD-матрицу, но её фактическое содержание пока не получено.

## 8. Рекомендуемая матрица извлечения

| Приоритет | Таблицы | Результат |
|---|---|---|
| 1 | `DMRS_DESIGNS`, `DMRS_MODELS` | Контекст дизайна и перечень моделей |
| 2 | `DMRS_ENTITIES`, `DMRS_ATTRIBUTES`, `DMRS_RELATIONSHIPS` | Логическая структура |
| 3 | `DMRS_KEYS`, `DMRS_KEY_ATTRIBUTES`, `DMRS_KEY_ELEMENTS` | Ключи и их состав |
| 4 | `DMRS_TABLES`, `DMRS_COLUMNS`, `DMRS_FOREIGNKEYS`, `DMRS_INDEXES` | Реляционная структура |
| 5 | `DMRS_MAPPINGS` | Сопоставление моделей |
| 6 | `DMRS_DIAGRAMS`, `DMRS_DIAGRAM_ELEMENTS` | Диаграммы |
| 7 | Домены, типы, AVT, ranges, checks | Семантика типов и ограничений |
| 8 | Процессы, глоссарии, заметки | Процессная и текстовая документация |

## 9. Методические оговорки

1. Описание столбца — это не значение столбца в конкретной записи.
2. OVID — ключевой идентификатор для связывания объектов; имена могут повторяться или меняться.
3. DDL DMRS repository не доказывает, какие объекты входят в Global11_demo.
4. Выводы о кардинальности, обязательности, PK/FK и типах должны подтверждаться данными и/или экспортом модели.
5. При расхождении XML, DMRS-строк и DDL расхождение фиксируется отдельно, а не разрешается догадкой.
