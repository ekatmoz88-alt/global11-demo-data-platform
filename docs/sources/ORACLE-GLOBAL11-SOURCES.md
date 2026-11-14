# Oracle Global 11g — исходные материалы и источники

## Официальные ссылки из проектной инструкции

- [Global 11g Readme (Oracle)](http://www.oracle.com/technetwork/database/options/olap/global-11g-readme-082667.html)
- [Global 11g Schema archive (Oracle ZIP)](http://www.oracle.com/technetwork/database/enterprise-edition/downloads/global-11g-schema-1-128202.zip)

## Инвентаризация предоставленного архива

В полученном архиве перечислены:

| Файл | Назначение по имени/содержимому пакета |
| --- | --- |
| `global_11g_source.dmp` | Экспорт исходной схемы и данных |
| `global_11g_install.sql` | Установочный сценарий |
| `global_11g_remove.sql` | Сценарий удаления схемы |
| `global_11g_create_cubes.sql` | Отдельный сценарий создания OLAP cubes / Analytic Workspace |
| `global_11g_readme.html` | Инструкция и описание sample schema |
| `Templates/CHANNEL.XML` | Шаблон dimension/channel |
| `Templates/CUSTOMER.XML` | Шаблон dimension/customer |
| `Templates/GLOBAL.XML` | Шаблон Global |
| `Templates/GLOBAL_MV.XML` | Шаблон materialized views |
| `Templates/PRICE_CUBE.XML` | Шаблон price cube |
| `Templates/PRODUCT.XML` | Шаблон product dimension |
| `Templates/TIME.XML` | Шаблон time dimension |
| `Templates/UNITS_CUBE.XML` | Шаблон units cube |

README и SQL-скрипты архива были просмотрены как текстовые материалы. DMP перечислен в составе пакета, но его внутреннее содержимое не описывается как отдельно проанализированное.

## Таблицы, перечисленные в sample schema

- `CHANNEL_DIM`
- `CUSTOMER_DIM`
- `PRODUCT_DIM`
- `TIME_DIM`
- `PRICE_FACT`
- `UNITS_FACT`
- `ACCOUNT`
- `PRODUCT_CHILD_PARENT`

## Установка и grants

Пакетный установщик создаёт схему `GLOBAL`, выполняет базовую настройку прав и импортирует данные. Среди базовых grants, отражённых в установочном SQL, присутствуют:

```sql
GRANT CREATE SESSION,
      CREATE DIMENSION,
      CREATE MATERIALIZED VIEW,
      ADVISOR,
      OLAP_USER,
      OLAP_XS_ADMIN,
      UNLIMITED TABLESPACE
TO GLOBAL;
```

Отдельная инструкция проекта для Oracle 12c добавляет:

```sql
GRANT CREATE VIEW,
      CREATE CUBE,
      CREATE CUBE DIMENSION,
      CREATE CUBE MEASURE FOLDER
TO GLOBAL;
```

Это два отдельных этапа. Дополнительные grants не следует приписывать штатному базовому установщику без проверки версии архива.

## Важное проектное ограничение

Несмотря на наличие `global_11g_create_cubes.sql` и XML templates, проектная инструкция говорит: **Do not create analytic workspace.** Поэтому этот сценарий не является частью установки Global11_demo, описанной в данном репозитории.


## Data Modeler dimensional-design references

- [Philip Stoyanov — Dimensional Modeling in Oracle SQL Developer (Oracle slide deck, 2013)](https://www.slideshare.net/slideshow/dimensional-modeling-in-oracle-sql-developer/29966332). The title slide identifies Philip Stoyanov as presenter; the deck covers dimensions, levels, level-based / value-based / ragged hierarchies, dimension usage, role-playing dimensions, cubes, measures, pre-computation, and export to Oracle Analytic Workspace.
- [Jeff Smith — Dimensional Modeling in Oracle SQL Developer Data Modeler (13 January 2014)](https://www.thatjeffsmith.com/archive/2014/01/dimensional-modeling-in-oracle-sql-developer-data-modeler/). The article identifies the `global11_demo.dmd` sample and its accompanying PDF build description in the Data Modeler sample installation.

The project history records that the portfolio author worked with Philip Stoyanov on deployment of the Global11 environment. The public slide deck is cited as a technical reference and is not presented as the portfolio author's work.
