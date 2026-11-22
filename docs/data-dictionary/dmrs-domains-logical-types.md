# DMRS: домены и логические типы данных

**Тип материала:** описание модели метаданных Oracle SQL Developer Data Modeler  
**Источник:** одностраничная диаграмма *Domains and Logical types.pdf*  
**Статус:** Draft — атрибуты описаны по диаграмме; физические ограничения и данные Oracle не проверялись.

## 1. Назначение схемы

Диаграмма показывает, как в репозитории DMRS представлены домены и логические типы данных, как логические типы сопоставляются с типами конкретной СУБД и где хранятся дополнительные правила для значений доменов.

В модели представлены:

- &#96;DMRS_DESIGNS&#96; — сведения о дизайне модели и его версии;
- &#96;DMRS_DOMAINS&#96; — определения доменов;
- &#96;DMRS_LOGICAL_TYPES&#96; — определения логических типов;
- &#96;DMRS_LOGICAL_TO_NATIVE&#96; и &#96;DMRS_NATIVE_TO_LOGICAL&#96; — сопоставления логических и физических типов в двух направлениях;
- &#96;DMRS_DOMAIN_AVT&#96;, &#96;DMRS_DOMAIN_VALUE_RANGES&#96; и &#96;DMRS_DOMAIN_CHECK_CONSTRAINTS&#96; — связанные с доменами значения, диапазоны и проверки.

Это **модель метаданных DMRS**, а не бизнес-модель таблиц приложения Global11.

## 2. Состав объектов и атрибутов

### 2.1. &#96;DMRS_DESIGNS&#96; — дизайн модели

| Поле | Тип | Назначение по имени |
|---|---|---|
| Design_ID | VARCHAR2(70) | Идентификатор дизайна |
| Design_OVID | VARCHAR2(36) | OVID дизайна; отмечен как ключевой атрибут |
| Design_Name | VARCHAR2(256) | Имя дизайна |
| Date_Published | TIMESTAMP | Дата и время публикации |
| Published_By | VARCHAR2(80) | Автор публикации |
| Persistence_Version | NUMBER(5,2) | Версия сохранения |
| Version_Comments | VARCHAR2(4000) | Комментарии к версии |

На диаграмме показаны &#96;DMRS_DESIGNS_PK (Design_OVID)&#96; и индекс &#96;DESIGNS_PK_IDX (Design_OVID)&#96;.

### 2.2. &#96;DMRS_DOMAINS&#96; — домены

Домен описывает именованную область значений и связанные с ней характеристики типа.

| Поле | Тип | Назначение по имени |
|---|---|---|
| Domain_ID | VARCHAR2(70) | Идентификатор домена; отмечен как PK |
| Domain_Name | VARCHAR2(256) | Имя домена |
| OVID | VARCHAR2(36) | Идентификатор OVID |
| Synonyms | VARCHAR2(4000) | Синонимы |
| Logical_Type_ID | VARCHAR2(70) | Идентификатор логического типа |
| Logical_Type_OVID | VARCHAR2(36) | OVID логического типа; отмечен F |
| T_Size | NUMBER | Размер типа |
| T_Precision | NUMBER | Точность |
| T_Scale | NUMBER | Масштаб |
| Native_Type | VARCHAR2(60) | Физический тип |
| LT_Name | VARCHAR2(256) | Имя логического типа |
| DESIGN_ID | VARCHAR2(70) | Идентификатор дизайна |
| Design_OVID | VARCHAR2(36) | OVID дизайна; отмечен F |
| Design_Name | VARCHAR2(256) | Имя дизайна |
| DEFAULT_VALUE | VARCHAR2(256) | Значение по умолчанию |
| UNIT_OF_MEASURE | VARCHAR2(30) | Единица измерения |
| Char_Units | CHAR(4) | Единицы измерения длины символьных данных |

На диаграмме показаны &#96;DMRS_DOMAINS_PK (Domain_ID)&#96;, &#96;DOMAINS_PK_IDX (Domain_ID)&#96;, &#96;DOMAINS_FK_IDX (Design_OVID)&#96; и &#96;DOMAINS_FK_IDXV1 (Logical_Type_OVID)&#96;.

### 2.3. &#96;DMRS_LOGICAL_TYPES&#96; — логические типы

| Поле | Тип | Назначение по имени |
|---|---|---|
| DESIGN_ID | VARCHAR2(70) | Идентификатор дизайна |
| Design_OVID | VARCHAR2(36) | OVID дизайна |
| Design_Name | VARCHAR2(256) | Имя дизайна |
| Logical_Type_ID | VARCHAR2(70) | Идентификатор логического типа |
| OVID | VARCHAR2(36) | OVID логического типа; отмечен как PK |
| LT_Name | VARCHAR2(256) | Имя логического типа |

На диаграмме показаны &#96;DMRS_LOGICAL_TYPES_PK (OVID)&#96;, &#96;LOGICAL_TYPES_PK_IDX (OVID)&#96; и &#96;LOGICAL_TYPES_FK_IDX (Design_OVID)&#96;.

### 2.4. &#96;DMRS_LOGICAL_TO_NATIVE&#96; — логический тип в физический

Объект содержит сведения о сопоставлении логического типа с физическим типом конкретной СУБД.

| Поле | Тип | Назначение по имени |
|---|---|---|
| DESIGN_ID | VARCHAR2(70) | Идентификатор дизайна |
| Design_OVID | VARCHAR2(36) | OVID дизайна |
| Design_Name | VARCHAR2(256) | Имя дизайна |
| Logical_Type_ID | VARCHAR2(70) | Идентификатор логического типа |
| Logical_Type_OVID | VARCHAR2(36) | OVID логического типа; отмечен F |
| LT_Name | VARCHAR2(256) | Имя логического типа |
| Native_Type | VARCHAR2(60) | Физический тип |
| RDBMS_Type | VARCHAR2(60) | Тип СУБД |
| RDBMS_Version | VARCHAR2(60) | Версия СУБД |
| Has_Size | VARCHAR2(1) | Признак поддержки размера |
| Has_Precision | VARCHAR2(1) | Признак поддержки точности |
| Has_Scale | VARCHAR2(1) | Признак поддержки масштаба |

На диаграмме показаны индексы &#96;LOGICAL_TO_NATIVE_FK_IDX (Design_OVID)&#96; и &#96;LOGICAL_TO_NATIVE_FK_IDXV1 (Logical_Type_OVID)&#96;.

### 2.5. &#96;DMRS_NATIVE_TO_LOGICAL&#96; — физический тип в логический

Объект описывает обратное направление сопоставления: от типа СУБД к логическому типу.

| Поле | Тип | Назначение по имени |
|---|---|---|
| RDBMS_Type | VARCHAR2(60) | Тип СУБД |
| RDBMS_Version | VARCHAR2(60) | Версия СУБД |
| Native_Type | VARCHAR2(60) | Физический тип |
| LT_Name | VARCHAR2(256) | Имя логического типа |
| Logical_Type_ID | VARCHAR2(70) | Идентификатор логического типа |
| Logical_Type_OVID | VARCHAR2(36) | OVID логического типа; отмечен F |
| DESIGN_ID | VARCHAR2(70) | Идентификатор дизайна |
| Design_OVID | VARCHAR2(36) | OVID дизайна |
| DESIGN_NAME | VARCHAR2(256) | Имя дизайна |

На диаграмме показаны индексы &#96;NATIVE_TO_LOGICAL_FK_IDX (Design_OVID)&#96; и &#96;NATIVE_TO_LOGICAL_FK_IDXV1 (Logical_Type_OVID)&#96;.

### 2.6. &#96;DMRS_DOMAIN_AVT&#96; — значения домена

| Поле | Тип | Назначение по имени |
|---|---|---|
| Domain_ID | VARCHAR2(70) | Идентификатор домена |
| Domain_OVID | VARCHAR2(70) | OVID домена; отмечен F |
| Sequence | NUMBER | Порядковый номер |
| Value | VARCHAR2(256) | Значение |
| Short_Description | VARCHAR2(256) | Краткое описание |
| Domain_Name | VARCHAR2(256) | Имя домена |
| Design_OVID | VARCHAR2(36) | OVID дизайна |

На диаграмме показан индекс &#96;DOMAIN_AVT_FK_IDX (Domain_OVID)&#96;.

### 2.7. &#96;DMRS_DOMAIN_VALUE_RANGES&#96; — диапазоны значений

| Поле | Тип | Назначение по имени |
|---|---|---|
| Domain_ID | VARCHAR2(70) | Идентификатор домена |
| Domain_OVID | VARCHAR2(70) | OVID домена; отмечен F |
| Sequence | NUMBER | Порядковый номер |
| Begin_Value | VARCHAR2(256) | Начало диапазона |
| End_Value | VARCHAR2(256) | Конец диапазона |
| Short_Description | VARCHAR2(256) | Краткое описание |
| Domain_Name | VARCHAR2(256) | Имя домена |
| Design_OVID | VARCHAR2(36) | OVID дизайна |

На диаграмме показан индекс &#96;DOMAIN_VALUE_RANGES_FK_IDX (Domain_OVID)&#96;.

### 2.8. &#96;DMRS_DOMAIN_CHECK_CONSTRAINTS&#96; — проверки домена

| Поле | Тип | Назначение по имени |
|---|---|---|
| Domain_ID | VARCHAR2(70) | Идентификатор домена |
| Domain_OVID | VARCHAR2(70) | OVID домена; отмечен F |
| Sequence | NUMBER | Порядковый номер |
| Text | VARCHAR2(4000) | Текст проверки |
| Database_Type | VARCHAR2(60) | Тип СУБД |
| Domain_Name | VARCHAR2(256) | Имя домена |
| Design_OVID | VARCHAR2(36) | OVID дизайна |

На диаграмме показан индекс &#96;DOMAIN_CHECK_CONSTR_FK_IDX (Domain_OVID)&#96;.

## 3. Логика взаимодействия объектов

1. &#96;DMRS_DESIGNS&#96; задаёт контекст дизайна. В связанных объектах повторяются &#96;Design_ID&#96;, &#96;Design_OVID&#96; и/или &#96;Design_Name&#96;.
2. &#96;DMRS_DOMAINS&#96; связывает описание домена с логическим типом и дизайном. Здесь же указаны характеристики типа, значение по умолчанию и единицы измерения.
3. &#96;DMRS_LOGICAL_TYPES&#96; хранит определения логических типов.
4. &#96;DMRS_LOGICAL_TO_NATIVE&#96; описывает сопоставление логического типа с физическим типом для заданных типа и версии СУБД.
5. &#96;DMRS_NATIVE_TO_LOGICAL&#96; описывает обратное сопоставление.
6. &#96;DMRS_DOMAIN_AVT&#96;, &#96;DMRS_DOMAIN_VALUE_RANGES&#96; и &#96;DMRS_DOMAIN_CHECK_CONSTRAINTS&#96; содержат связанные с доменом отдельные значения, диапазоны и тексты проверок.

## 4. Связи, показанные на диаграмме

| Исходный объект | Атрибут связи | Связанный объект |
|---|---|---|
| &#96;DMRS_DESIGNS&#96; | Design_OVID | &#96;DMRS_DOMAINS&#96; |
| &#96;DMRS_DESIGNS&#96; | Design_OVID | &#96;DMRS_LOGICAL_TYPES&#96; |
| &#96;DMRS_DESIGNS&#96; | Design_OVID | &#96;DMRS_LOGICAL_TO_NATIVE&#96; |
| &#96;DMRS_DESIGNS&#96; | Design_OVID | &#96;DMRS_NATIVE_TO_LOGICAL&#96; |
| &#96;DMRS_LOGICAL_TYPES&#96; | OVID ↔ Logical_Type_OVID | &#96;DMRS_DOMAINS&#96; |
| &#96;DMRS_LOGICAL_TYPES&#96; | OVID ↔ Logical_Type_OVID | &#96;DMRS_LOGICAL_TO_NATIVE&#96; |
| &#96;DMRS_LOGICAL_TYPES&#96; | OVID ↔ Logical_Type_OVID | &#96;DMRS_NATIVE_TO_LOGICAL&#96; |
| &#96;DMRS_DOMAINS&#96; | Domain_OVID | &#96;DMRS_DOMAIN_AVT&#96; |
| &#96;DMRS_DOMAINS&#96; | Domain_OVID | &#96;DMRS_DOMAIN_VALUE_RANGES&#96; |
| &#96;DMRS_DOMAINS&#96; | Domain_OVID | &#96;DMRS_DOMAIN_CHECK_CONSTRAINTS&#96; |

Связи воспроизводят линии и маркеры исходной диаграммы. Точные целевые колонки, кардинальность, обязательность и физические FK необходимо подтвердить по DDL или данным системного каталога Oracle.

## 5. Оговорки и вопросы для проверки

- Обозначения P и F сохранены как маркеры PK и FK, согласно исходной схеме.
- Символ * присутствует у части полей, но легенда для него в предоставленном файле отсутствует. Он не интерпретируется как NOT NULL.
- Индекс с суффиксом FK сам по себе не доказывает наличие физического внешнего ключа.
- Назначение полей описано по их именам; бизнес-семантика и фактические значения не проверялись.
- В исходной диаграмме &#96;DMRS_DOMAINS.OVID&#96; имеет тип &#96;VARCHAR2(36)&#96;, а дочерние поля &#96;Domain_OVID&#96; показаны как &#96;VARCHAR2(70)&#96;. Это различие сохранено и требует проверки по DDL.

Перед переводом документа в статус Final необходимо проверить физические PK/FK, целевые колонки, кардинальность и обязательность связей, значение маркера *, различие типов OVID и свойства индексов.

## 6. Источник

*Domains and Logical types.pdf*, страница 1. Названия объектов, атрибуты, типы данных, индексы и видимые связи перенесены из исходной диаграммы.

**Статус документа:** Draft — описание соответствует доступному изображению; физическая реализация не верифицирована.
