# Global11_demo — установка и подготовка схемы

## 1. Назначение

Документ описывает установку и проектные изменения схемы Global11 на основе Oracle Global 11g. Платформа была развёрнута и работала на офисном сервере; работы по развёртыванию выполнялись совместно с Philip Stoyanov. Приведённые ниже материалы описывают установочную основу и структуру. История проектирования размерной модели в Oracle SQL Developer Data Modeler вынесена в отдельный документ: [Global11 — Data Modeler Design](GLOBAL11-DATA-MODELER-DESIGN.ru.md).

Исходная инструкция проекта:

- [Global 11g README](http://www.oracle.com/technetwork/database/options/olap/global-11g-readme-082667.html)
- [Архив схемы](http://www.oracle.com/technetwork/database/enterprise-edition/downloads/global-11g-schema-1-128202.zip)

## 2. Историческое развёртывание и базовая установка

Global11 была развёрнута на офисном сервере и работала в офисной инфраструктуре. Развёртывание выполнялось с офисных рабочих машин, а не с персонального ноутбука автора документации. Ноутбук использовался для анализа материалов и подготовки документации.

Ниже приведена установочная последовательность, зафиксированная в Oracle Global 11g README. Она не означает, что историческая система должна разворачиваться повторно для подтверждения факта её эксплуатации.

1. Получить официальный Global 11g archive.
2. Прочитать `global_11g_readme.html`.
3. Установить схему `GLOBAL` штатной процедурой из пакета (`global_11g_install.sql` и поставляемый dump-файл, согласно README).
4. Проверить наличие исходных таблиц и доступ пользователя `GLOBAL`.

Архив содержит, в частности:

- `global_11g_source.dmp`
- `global_11g_install.sql`
- `global_11g_remove.sql`
- `global_11g_create_cubes.sql`
- `global_11g_readme.html`
- XML templates: `CHANNEL.XML`, `CUSTOMER.XML`, `GLOBAL.XML`, `GLOBAL_MV.XML`, `PRICE_CUBE.XML`, `PRODUCT.XML`, `TIME.XML`, `UNITS_CUBE.XML`.

### Запрет на создание Analytic Workspace

Для данного проекта **не создавать Analytic Workspace**. Файл `global_11g_create_cubes.sql` является отдельным сценарием создания OLAP-объектов из Oracle-пакета и не должен выполняться в рамках этой инструкции.

## 3. Дополнительные привилегии Oracle 12c

Проектная инструкция требует выдать пользователю `GLOBAL` дополнительные права:

```sql
GRANT CREATE VIEW,
      CREATE CUBE,
      CREATE CUBE DIMENSION,
      CREATE CUBE MEASURE FOLDER
TO GLOBAL;
```

Скрипт: [sql/01_oracle12c_privileges.sql](https://github.com/ekatmoz88-alt/global11-demo-data-platform/blob/main/sql/01_oracle12c_privileges.sql).

Эти права не заменяют базовые привилегии, выдаваемые штатным установщиком Oracle Global 11g.

## 4. Изменения таблицы UNITS_FACT

Добавляются два столбца типа `VARCHAR2(7)`:

- `SHIP_MONTH_ID` — роль времени отгрузки;
- `DELIVERY_MONTH_ID` — роль времени доставки.

Затем обе колонки инициализируются из `MONTH_ID`:

```sql
ALTER TABLE GLOBAL.UNITS_FACT ADD SHIP_MONTH_ID VARCHAR2(7);
ALTER TABLE GLOBAL.UNITS_FACT ADD DELIVERY_MONTH_ID VARCHAR2(7);

UPDATE GLOBAL.UNITS_FACT SET SHIP_MONTH_ID = MONTH_ID;
UPDATE GLOBAL.UNITS_FACT SET DELIVERY_MONTH_ID = MONTH_ID;
```

**Семантическое ограничение:** это точное исходное заполнение из проектной инструкции. Значения двух ролей будут одинаковыми для каждой строки. Данные не доказывают и не восстанавливают отдельные даты реальной отгрузки и доставки. Для такого анализа нужны независимые исходные атрибуты событий.

## 5. Создание CUST_ACCOUNT

Создаётся физическая таблица `GLOBAL.CUST_ACCOUNT` через CTAS. Источники — `GLOBAL.CUSTOMER_DIM` и `GLOBAL.ACCOUNT`, соединение выполняется по `ACCOUNT_ID`.

Поля назначения:

- из `CUSTOMER_DIM`: `SHIP_TO_ID`, `SHIP_TO_DSC`, `SHIP_TO_DSC_FRENCH`, `SHIP_TO_DSC_DUTCH`;
- из `ACCOUNT`: `ACCOUNT_ID`, имя/фамилия, адресные и контактные атрибуты.

Полный SQL находится в [02_global11_demo_schema_changes.sql](https://github.com/ekatmoz88-alt/global11-demo-data-platform/blob/main/sql/02_global11_demo_schema_changes.sql).

CTAS создаёт таблицу на момент выполнения запроса. Это не view и не автоматически синхронизируемая копия исходных таблиц. Ограничения и индексы исходных таблиц автоматически не следует считать перенесёнными.

## 6. Порядок выполнения

1. Установить исходную схему Oracle Global 11g.
2. Подключиться с необходимыми административными правами.
3. Выполнить `sql/01_oracle12c_privileges.sql` (для Oracle 12c, если применимо).
4. Убедиться, что `GLOBAL.UNITS_FACT`, `GLOBAL.CUSTOMER_DIM` и `GLOBAL.ACCOUNT` существуют.
5. Убедиться, что столбцы `SHIP_MONTH_ID` и `DELIVERY_MONTH_ID` ещё не добавлены, а таблица `CUST_ACCOUNT` ещё не создана.
6. Выполнить `sql/02_global11_demo_schema_changes.sql`.
7. Проверить количество строк и заполнение новых столбцов.

## 7. Проверки после изменения

```sql
SELECT COUNT(*) AS total_rows,
       COUNT(SHIP_MONTH_ID) AS ship_month_rows,
       COUNT(DELIVERY_MONTH_ID) AS delivery_month_rows
FROM GLOBAL.UNITS_FACT;

SELECT COUNT(*) AS mismatched_role_keys
FROM GLOBAL.UNITS_FACT
WHERE SHIP_MONTH_ID <> DELIVERY_MONTH_ID
   OR SHIP_MONTH_ID IS NULL
   OR DELIVERY_MONTH_ID IS NULL;

SELECT COUNT(*) AS cust_account_rows
FROM GLOBAL.CUST_ACCOUNT;
```

При исходной инициализации ожидается ноль различий между двумя role keys (при условии, что `MONTH_ID` заполнен). Это проверка результата инициализации, а не бизнес-проверка реальных дат событий.

## 8. Повторное выполнение и транзакции

Скрипт изменений схемы предназначен для однократного применения к исходной структуре. Повторный запуск может завершиться ошибкой на уже существующих колонках или таблице. Перед повторным применением в любой среде проверьте фактическое состояние объектов, используйте резервную копию и согласуйте транзакционный режим с DBA. Исторический факт работы Global11 на офисном сервере не зависит от повторного запуска этих скриптов в рамках подготовки портфолио.
