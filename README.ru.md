# Oracle Global11 — архитектура данных и OLAP

[English](README.md) | [Русский](README.ru.md)

**Опубликованная документация:** [Документация Global11](https://ekatmoz88-alt.github.io/global11-demo-data-platform/)

Техническая документация платформы данных **Global11**, развёрнутой и работавшей на офисном сервере под Oracle Global 11g. Репозиторий описывает реляционную модель, проектные изменения схемы, словарь данных, SQL-материалы и размерное проектирование в Oracle SQL Developer Data Modeler.

## Назначение

Репозиторий посвящён реляционной модели Oracle GLOBAL, изменениям схемы, выполненным в рамках проекта, словарю данных, SQL-скриптам и материалам OLAP-проектирования.

## Состав

- [Документация OLAP](docs/GLOBAL11-OLAP-DOCUMENTATION.md).
- [Финальная спецификация Global11_demo / Cube_1111](docs/model/CUBE-1111-FINAL-SPECIFICATION.md) — **Documentation Complete; Technical Validation Pending**.
- [Финальная OLAP-схема Cube_1111](docs/model/olap-diagram.mmd).
- [Настройка схемы Global11](docs/GLOBAL11-DEMO-SETUP.md).
- [Проектирование в Oracle SQL Developer Data Modeler](docs/GLOBAL11-DATA-MODELER-DESIGN.ru.md) — история проекта, размерная модель, role-playing dimensions, иерархии, меры и экспорт в Oracle AW.
- [Реляционная модель](docs/model/GLOBAL11-RELATIONAL-MODEL.md).
- [Источники Oracle](docs/sources/ORACLE-GLOBAL11-SOURCES.md).
- [DMRS: анализ метаданных Global11](docs/dmrs-global11-analysis.md).
- [DMRS: каталог метаданных](docs/dmrs-metadata-catalog.md).
- [DMRS: домены и логические типы](docs/data-dictionary/dmrs-domains-logical-types.md) и [диаграмма связей](docs/model/dmrs-domains-logical-types.mmd).
- `data-dictionary/` — извлечённый словарь данных.
- `sql/` — SQL-скрипты и заметки по выполнению.
- `sources/oracle/` — предоставленный архив Oracle Data Modeler.

## Документированные изменения схемы

- В `GLOBAL.UNITS_FACT` добавляются роли временного ключа `SHIP_MONTH_ID` и `DELIVERY_MONTH_ID`; обе заполняются из `MONTH_ID`.
- Таблица `CUST_ACCOUNT` создаётся через CTAS на основе `CUSTOMER_DIM` и `ACCOUNT`; это физическая таблица, а не представление.

## Дополнительные материалы: Claim-domain drafts

В каталоге [`docs/claim-domain-draft/`](docs/claim-domain-draft/README.md) собраны отдельные проектные черновики по предметной области claim:

- [Обзор и границы доказательств](docs/claim-domain-draft/README.md).
- [HTML-обзор](docs/claim-domain-draft/index.html).
- [Анализ архитектуры базы данных](docs/claim-domain-draft/database-architecture-review.md).
- [Черновик руководства администратора](docs/claim-domain-draft/administrator-guide.md).
- [Черновик REST API](docs/claim-domain-draft/rest-api-specification.md) и [OpenAPI 3.1](docs/claim-domain-draft/openapi-draft.yaml).
- [Материалы SOAP-интеграции](docs/claim-domain-draft/soap-integration-specification.md) и [фрагмент XSD запроса](docs/claim-domain-draft/soap-create-claim-request.xsd).
- [Заметки по SQL](docs/claim-domain-draft/sql-design-notes.sql).

**Статус этих материалов — Draft.** Они представляют собой проектные наброски и анализ доступных фрагментов, а не подтверждённую спецификацию работающей системы. В частности, наличие этих документов не доказывает, что claim-domain реализован в той же базе данных или в OLAP-кубе `Cube_1111`.

Извлечённые метаданные `Cube_1111` и проектные материалы claim-domain рассматриваются раздельно. Для подтверждения физической модели необходимы DDL или системный каталог Oracle; для REST — первичные сведения о маршрутах, методах и авторизации; для SOAP — WSDL, binding, endpoint и схемы ответов. SQL-фрагменты являются иллюстративными заметками и не предназначены для непосредственного запуска как production migration.

## Границы и ограничения

Платформа Global11 была развёрнута и работала на офисном сервере. Документация основана на материалах реального проекта, Oracle Global 11g, проектных инструкциях, SQL-скриптах и модели Data Modeler. Текущее состояние исторического сервера и базы данных при подготовке публичной версии портфолио повторно не проверялось.

Реляционная платформа отделена от размерного проектирования и фактического экспорта в Oracle Analytic Workspace. Первоначальная установка схемы прямо исключает создание AW; экспорт из Data Modeler является отдельной операцией. Техническая верификация финальных DDL/SQL и объектов Oracle AW остаётся **Pending**. Перед повторным применением SQL-скриптов необходимо проверить версию БД, объекты, права и факт предыдущего выполнения.

Слой SQLite и C#-компоненты относятся к отдельному проекту Deshifrator и не входят в платформу Global11.

## Теги

`oracle` · `global11` · `data-modeling` · `sql` · `data-dictionary` · `database-documentation`

## Исходные документы

- [Global11_demo.pdf — исходный документ](https://github.com/ekatmoz88-alt/technical-docs-library/blob/main/02_ORACLE/Global11_demo.pdf)
- [Global11demo schemes.pdf — исходные схемы](https://github.com/ekatmoz88-alt/technical-docs-library/blob/main/02_ORACLE/Global11demo%20schemes.pdf)
- [Детальная DMRS-диаграмма иерархий](docs/model/olap-diagram-dmrs-source.mmd)
- [Каталог UUID иерархий DMRS](docs/dmrs-metadata-catalog.md)
