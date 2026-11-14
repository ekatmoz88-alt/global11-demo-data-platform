# GLOBAL11 Demo Data Platform

[English](README.md) | [Русский](README.ru.md)

**Опубликованная документация:** [Документация Global11](https://ekatmoz88-alt.github.io/global11-demo-data-platform/)

Техническая документация реальной платформы данных **Global11**, развёрнутой и работавшей на офисном сервере под Oracle Global 11g. Работы по развёртыванию выполнялись совместно с Philip Stoyanov. Репозиторий описывает реляционную модель, проектные изменения схемы, словарь данных, SQL-материалы и размерное проектирование в Oracle SQL Developer Data Modeler.

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
- `data-dictionary/` — извлечённый словарь данных.
- `sql/` — SQL-скрипты и заметки по выполнению.
- `sources/oracle/` — предоставленный архив Oracle Data Modeler.

## Документированные изменения схемы

- В `GLOBAL.UNITS_FACT` добавляются роли временного ключа `SHIP_MONTH_ID` и `DELIVERY_MONTH_ID`; обе заполняются из `MONTH_ID`.
- Таблица `CUST_ACCOUNT` создаётся через CTAS на основе `CUSTOMER_DIM` и `ACCOUNT`; это физическая таблица, а не представление.

## Границы и ограничения

Платформа Global11 была развёрнута и работала на офисном сервере. Документация основана на реальном проекте, материалах Oracle Global 11g, проектных инструкциях, SQL-скриптах и модели Data Modeler. Текущее состояние исторического сервера и базы данных при подготовке публичной версии портфолио повторно не проверялось.

Реляционная платформа отделена от размерного проектирования и фактического экспорта в Oracle Analytic Workspace. Первоначальная установка схемы прямо исключает создание AW; экспорт из Data Modeler является отдельной операцией. Техническая верификация финальных DDL/SQL и объектов Oracle AW остаётся **Pending**. Перед повторным применением SQL-скриптов необходимо проверить версию БД, объекты, права и факт предыдущего выполнения.

Слой SQLite и C#-компоненты относятся к отдельному проекту Deshifrator и не входят в эту платформу Global11.

## Теги

`oracle` · `global11` · `data-modeling` · `sql` · `data-dictionary` · `database-documentation`
