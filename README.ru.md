# GLOBAL11 Demo Data Platform

[English](README.md) | [Русский](README.ru.md)

**Опубликованная документация:** [Документация Global11](https://ekatmoz88-alt.github.io/global11-demo-data-platform/)

Документация и воспроизводимый анализ учебного проекта **Global11_demo Data Modeler**, основанного на демонстрационной схеме Oracle Global 11g.

## Назначение

Репозиторий посвящён реляционной модели Oracle GLOBAL, подтверждённым изменениям демонстрационной схемы, словарю данных, SQL-скриптам и концептуальным OLAP-артефактам.

## Состав

- [Документация OLAP](docs/GLOBAL11-OLAP-DOCUMENTATION.md).
- [Настройка демонстрационной схемы](docs/GLOBAL11-DEMO-SETUP.md).
- [Реляционная модель](docs/model/GLOBAL11-RELATIONAL-MODEL.md).
- [Источники Oracle](docs/sources/ORACLE-GLOBAL11-SOURCES.md).
- `data-dictionary/` — извлечённый словарь данных.
- `sql/` — проверенные SQL-скрипты и заметки по выполнению.
- `sources/oracle/` — предоставленный архив Oracle Data Modeler.

## Подтверждённые изменения демонстрационной схемы

- В `GLOBAL.UNITS_FACT` добавляются роли временного ключа `SHIP_MONTH_ID` и `DELIVERY_MONTH_ID`; обе заполняются из `MONTH_ID`.
- Таблица `CUST_ACCOUNT` создаётся через CTAS на основе `CUSTOMER_DIM` и `ACCOUNT`; это физическая таблица, а не представление.

## Границы и ограничения

Выполнение скриптов в конкретной установке Oracle не заявляется. Перед запуском следует проверить версию БД, объекты, права и факт предыдущего применения скриптов. Концептуальные OLAP-артефакты не означают, что Analytic Workspace создан или развёрнут.

Слой SQLite и C#-компоненты относятся к отдельному проекту Deshifrator и не входят в эту платформу Global11.

## Теги

`oracle` · `global11` · `data-modeling` · `sql` · `data-dictionary` · `database-documentation`
