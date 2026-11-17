# Global11 — эталоны Oracle, аудит и план закрытия пробелов

**Статус аудита:** документальный аудит публичного репозитория.  
**Область:** Oracle Global 11g / схема `GLOBAL` / проектные изменения / размерная модель `Global11_demo` / спецификация `Cube_1111`.  
**Важно:** аудит выполнен по файлам репозитория и опубликованным руководствам Oracle. Подключения к исторической базе и повторного выполнения SQL в Oracle в рамках аудита не было. Поэтому документ не объявляет OLAP-куб развёрнутым и не подтверждает физические ограничения базы.

## 1. Пять эталонных источников

Мы используем пять официальных или непосредственно связанных с Oracle эталонов. Они задают разные части стандарта; ни один из них не следует копировать целиком как будто это схема нашего проекта.

| № | Эталон | Что берём в Global11 | Что не переносим |
|---|---|---|---|
| 1 | [Oracle Global Schema 11g / OLAP Downloads](https://www.oracle.com/database/technologies/olap-downloads.html) | Исходную схему, sample scripts, словарь объектов и контекст Global 11g | Не заменяем имена и структуру из нашего архива чужой версией Global |
| 2 | [Oracle OLAP User's Guide 11g R2](https://docs.oracle.com/cd/E24693_01/olap.11203/e17123.pdf) | Термины Cube, Measure, Dimension, Level, Hierarchy; mapping, loading, querying, drill-down | Не считаем описанные в руководстве операции выполненными в нашей среде |
| 3 | [Building OLAP 11g Cubes](https://www.oracle.com/ocom/groups/public/@otn/documents/webcontent/237509.htm) | Полный жизненный цикл проектирования: AW, dimensions, levels, hierarchies, measures, mapping, loading | OLAPTRAIN / SALESTRACK — другая демонстрационная схема, не Global11 |
| 4 | [Querying OLAP 11g Cubes](https://www.oracle.com/ocom/groups/public/@otn/documents/webcontent/237918.htm) | SQL-доступ к cube views, calculated measures, dimension filters, drill-down, query rewrite | Примеры запросов нельзя запускать без сверки имён наших объектов и версии Oracle |
| 5 | [Generating a Multi-Dimensional Model — SQL Developer Data Modeler](https://www.oracle.com/webfolder/technetwork/tutorials/obe/db/sqldevdm/r40/datamodel4genmulti/datamodel4genmulti_otn.html) | Последовательность DDL → relational → logical → multidimensional model; отчёт по dimensions/levels | Tutorial использует SH schema, а не нашу модель Global11 |

Дополнительный источник именно по Global 11g: [Oracle OLAP Java API — Global sample schema](https://docs.oracle.com/cd/B28359_01/olap.111/b28127/intro.htm). Он подтверждает, что Global schema использовалась как источник для примеров OLAP metadata и Analytic Workspace. Более старое [описание Global Schema 10g](https://docs.oracle.com/cd/B13789_01/olap.101/b10333/global.htm) полезно для архитектурных понятий, но не является заменой пакета 11g.

## 2. Что добавлено в стандарт документации Global11

Ниже — обязательные составляющие, которые следует поддерживать в нашем проекте. Это стандарт полноты документации, а не утверждение, что все элементы уже реализованы в базе.

### 2.1 Business requirements и аналитические сценарии

Перед описанием куба фиксируем:
- бизнес-вопрос или аналитическую задачу;
- пользователя / роль потребителя;
- измерения, по которым нужен анализ;
- требуемый уровень детализации;
- периодичность и ожидаемую свежесть данных;
- показатели и правила их интерпретации;
- требуемые drill-down / roll-up сценарии.

Для Global11 такие сценарии должны быть восстановлены только из исходных проектных требований или подтверждённых материалов. Не следует придумывать бизнес-цели по одному лишь составу таблиц.

### 2.2 Fact grain и source-to-target mapping

Для каждой фактовой таблицы нужна отдельная спецификация:
- зерно факта (что означает одна строка);
- набор измерительных ключей;
- исходные меры и единицы измерения;
- источник каждой колонки;
- преобразование / формула;
- правила NULL и неизвестных членов;
- ожидаемая уникальность комбинации ключей;
- дата/время загрузки и происхождение данных, если они есть в источнике.

Для `UNITS_FACT` кандидат на зерно следует проверять по фактическим данным и DDL; нельзя фиксировать уникальную комбинацию `MONTH_ID + ITEM_ID + SHIP_TO_ID + CHANNEL_ID` без проверки. В DMRS-материалах перечислены также меры `UNITS`, `SALES`, `COST`; их семантика и правила агрегации должны быть описаны отдельно.

### 2.3 Dimension specification

Для каждого измерения документируем:
- business meaning и source table;
- base level / grain;
- level keys и descriptive attributes;
- hierarchies и последовательность levels;
- составные ключи и правила уникальности;
- parent-child relationships;
- ragged / skip-level cases;
- default / all member, если он определён;
- историчность атрибутов, если она подтверждена;
- mapping от исходных колонок к атрибутам Data Modeler / OLAP.

Рекомендуемые отдельные страницы для Global11: Time, Customer/Geography, Product, Channel. Географическая модель `GEO_ROLLUP` пока остаётся Draft до проверки исходной модели.

### 2.4 Measure specification

Для каждой меры фиксируем:
- бизнес-название и техническое имя;
- источник или формулу;
- тип и единицу;
- aggregation / solve order;
- additive, semi-additive или non-additive характер;
- допустимые измерения агрегации;
- NULL / zero handling;
- пример расчёта и контрольный SQL.

Особое правило: ранги, проценты, доли и средние нельзя автоматически агрегировать как `SUM`. Для `CALC_MSR1` в спецификации указан `AGGREGATION = NONE`; это должно оставаться согласованным во всех примерах. Правило для `MSR_2` не фиксируем, пока не восстановим одну авторитетную формулу.

### 2.5 Cube mapping, load и query

Полный цикл OLAP должен быть описан раздельными этапами:
1. исходные relational objects;
2. dimensions, levels и hierarchies;
3. dimension usage / joins;
4. cube и measures;
5. mapping source columns to cube;
6. initial load и последующее обновление;
7. aggregation / pre-computation;
8. SQL access через cube views;
9. query examples и drill-down;
10. эксплуатация, мониторинг и восстановление.

Для нашего проекта пункты, связанные с фактическим созданием AW, загрузкой и query rewrite, остаются проектным/целевым сценарием, пока не появится runtime evidence.

### 2.6 Data quality, security и operational controls

В дополнение к модели нужны:
- проверки PK/FK и orphan rows;
- проверка уникальности ключей уровней;
- проверка NULL, диапазонов и допустимых значений;
- reconciliation count/sum между источником и целевой структурой;
- права владельца, разработчика и read-only потребителя;
- backup / restore и безопасный повторный запуск;
- логирование загрузок и ошибок;
- version / environment matrix.

Существующие скрипты проверяют часть документационных условий, но не подключаются к Oracle и не доказывают качество данных или runtime-состояние.

## 3. Результаты аудита текущего репозитория

| ID | Приоритет | Наблюдение | Риск / требуемое действие |
|---|---|---|---|
| G11-01 | High | CTAS в `02_global11_demo_schema_changes.sql` выбирает 16 колонок, включая `STATE_PROV`; DMRS physical-model excerpt для `CUST_ACCOUNT` содержит 15 колонок и не показывает `STATE_PROV` | Сверить исходный `.dmd`, CTAS и импортированную физическую модель. До этого не менять ни один источник как «правильный» |
| G11-02 | High | В `CUBE-1111-FINAL-SPECIFICATION.md` используются имена `GLOBAL_CUSTOMER_DIM`, `DIM_ACCOUNT_ID`, `GLOBAL_ACCOUNT_*`; setup/CTAS используют `CUSTOMER_DIM`, `ACCOUNT_ID`, `ACCOUNT_*` | Составить mapping-таблицу между именами физической схемы, DMRS model и экспортированным SQL; подтвердить, что это не разные naming layers |
| G11-03 | High | Validation SQL использует `R_SHIP_TO_ID`, `DIM_ACCOUNT_ID`, `GLOBAL_ACCOUNT_COUNTRY`; в relational overview и setup указаны `SHIP_TO_ID`, `ACCOUNT_ID`, `COUNTRY` | Запрос считать примером/непроверенным до сверки с фактическими колонками. Подготовить исполняемый SQL отдельно после подтверждения |
| G11-04 | High | В OLAP overview для `MSR_2` описана формула `MSR_1 * 2`, а в Cube specification — `TOTAL(MSR_1)` | Установить авторитетную формулу по исходному DMD / требованиям. Не выбирать формулу по догадке |
| G11-05 | High | В OLAP overview пример ранга сопровождается `AGGREGATION = SUM`; Cube specification указывает `AGGREGATION = NONE` для ранга | Согласовать описание: rank — вычисляемая неаддитивная мера; не суммировать ранговые значения |
| G11-06 | High | Начальная инструкция установки запрещает создавать Analytic Workspace; Cube specification описывает AW DDL, aggregation и export | Развести Initial relational setup и Optional OLAP build/export. Не включать AW script в обычную установку и не заявлять deployment без evidence |
| G11-07 | Medium | Для `UNITS_FACT` не закреплено проверенное зерно факта и полная source-to-target mapping таблица | Определить grain и проверить уникальность комбинаций ключей на базе / DDL |
| G11-08 | Medium | В словаре/DMRS excerpt есть `UNITS`, `SALES`, `COST`, но Cube_1111 фокусируется на `MSR_1 = UNITS` | Добавить inventory всех source measures и зафиксировать, какие включены/не включены в Cube_1111 |
| G11-09 | Medium | Customer/Geography, Product, Channel и Time описаны неравномерно; `GEO_ROLLUP` прямо помечен Draft | Ввести единый dimension-spec template и одинаковую глубину описания для каждого измерения |
| G11-10 | Medium | Есть setup checks, но нет полного набора source-to-target reconciliation и data-quality checks | Добавить SQL checks после подтверждения реальных ключей и ожидаемых значений |
| G11-11 | Medium | Технический статус AW обозначен Pending, но отдельные таблицы спецификации используют слово Complete для проектных пунктов | Везде различать Design documented, Script prepared, Runtime executed, Runtime verified |
| G11-12 | Low | Oracle Global Schema 10g guide и Global 11g package могут содержать разные имена/варианты таблиц | Для имён и состава объектов приоритет — конкретный архив Global 11g, а старые руководства использовать как архитектурный эталон |

## 4. Важные технические оговорки

### 4.1 Global 11g и старые Oracle examples

В нашем архиве Global 11g перечислены `PRICE_FACT` и `UNITS_FACT`. В старом руководстве Oracle Global 10g встречаются другие имена, например `PRICE_AND_COST_HISTORY_FACT`. Это разные версии/контексты примеров; не переименовывать наши таблицы на основании старого руководства.

### 4.2 Role-playing time

`SHIP_MONTH_ID` и `DELIVERY_MONTH_ID` первоначально заполняются из `MONTH_ID`. Это показывает два назначения одной временной размерности в проектном дизайне, но не подтверждает наличие независимых дат отгрузки и доставки. Любой анализ длительности или фактических дат событий требует отдельных исходных дат.

### 4.3 CUST_ACCOUNT

CTAS создаёт физическую таблицу на момент выполнения. Он не гарантирует автоматическое сохранение индексов, PK/FK, триггеров, grants или синхронизацию будущих изменений источников. Проверить:
- количество колонок и точные имена;
- число строк до/после join;
- дубликаты по предполагаемому ключу;
- строки без соответствующего `ACCOUNT_ID`;
- поведение NULL;
- необходимые индексы и ограничения после CTAS.

### 4.4 AW и Cube_1111

Наличие `global_11g_create_cubes.sql`, XML templates или спецификации Data Modeler не доказывает, что AW был создан. Документировать AW как deployed можно только после runtime evidence: успешный export/build log и/или запросы к Oracle metadata views в целевой среде. До этого `Cube_1111` — проектная спецификация со статусом Technical Validation Pending.

## 5. Рекомендуемая структура документации после аудита

| Документ | Назначение | Статус |
|---|---|---|
| `GLOBAL11-RELATIONAL-MODEL.md` | Таблицы, колонки, ключи, связи, grain | Есть; требуется подтверждение constraints/grain |
| `GLOBAL11-DATA-MODELER-DESIGN.md` | Процесс построения модели и экспорт | Есть |
| `GLOBAL11-DATA-DICTIONARY` | Словарь и типы данных | Есть; требуется reconciliation с физической моделью |
| `GLOBAL11-DIMENSION-SPECIFICATIONS.md` или отдельные страницы dimensions | Единый шаблон Time/Customer/Geography/Product/Channel | Следующий этап |
| `GLOBAL11-MEASURE-CATALOG.md` | Source measures, calculated measures, additivity | Следующий этап |
| `GLOBAL11-SOURCE-TO-TARGET-MAPPING.md` | Колонка источника → атрибут/мера OLAP → преобразование | Следующий этап |
| `GLOBAL11-DATA-QUALITY-CHECKS.md` | SQL checks и reconciliation | Следующий этап после подтверждения имён |
| `CUBE-1111-FINAL-SPECIFICATION.md` | Спецификация целевого куба | Есть; требует устранения G11-02…G11-06 |

## 6. Приоритетный план исправлений

1. **Resolve naming and column mismatches:** сверить CTAS (16 колонок), DMRS physical model (15), Cube DDL и validation SQL.
2. **Resolve measure definitions:** утвердить формулы `MSR_2` и `CALC_MSR1`, их aggregation behavior.
3. **Document fact grain:** установить фактическую гранулярность `UNITS_FACT` и `PRICE_FACT`; не выводить её только из предполагаемого набора ключей.
4. **Complete dimension specifications:** Time, Customer/Geography, Product, Channel с levels, keys, attributes, hierarchy and source mappings.
5. **Complete measure catalogue and mapping:** `UNITS`, `SALES`, `COST` и показатели из `PRICE_FACT` — только по реальному data dictionary.
6. **Add data-quality and reconciliation SQL:** сначала проверить названия колонок и ключи, затем публиковать запросы как executable.
7. **Keep deployment evidence separate:** initial schema install, schema extensions, Data Modeler export, AW build, cube load, query validation — разные этапы.

## 7. Источники и границы аудита

- Исходные проектные документы и SQL внутри этого репозитория — основной источник для конкретных имён объектов Global11.
- Oracle Global Schema 11g и Oracle OLAP 11g User's Guide — официальные эталоны архитектуры и продукта.
- OLAPTRAIN / SALESTRACK и SH Data Modeler tutorial — методические примеры, не физическая схема Global11.
- Ни один внешний пример не отменяет необходимость проверки DDL, DMD/DMRS и runtime metadata конкретной базы.

**Итог:** документация уже содержит хорошую основу — реляционные объекты, проектные schema changes, словарь, Data Modeler workflow и Cube_1111 design. Главные пробелы сейчас не в количестве диаграмм, а в согласованности mapping/имён/формул и доказательности статуса реализации. Сначала закрываем High findings; затем расширяем каталог dimensions, measures и data-quality checks.
