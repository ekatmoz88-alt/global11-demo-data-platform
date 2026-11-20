# Oracle Global11 — Data Model & OLAP Architecture

Документация реальной платформы Oracle Global11 и связанного размерного дизайна Data Modeler. `Global11_demo` — исходное имя дизайна Data Modeler, а не статус проекта.

## Документация

- [Подготовка среды и изменения схемы](GLOBAL11-DEMO-SETUP.md)
- [Проектирование в Oracle SQL Developer Data Modeler — EN](GLOBAL11-DATA-MODELER-DESIGN.md)
- [Проектирование в Oracle SQL Developer Data Modeler — RU](GLOBAL11-DATA-MODELER-DESIGN.ru.md)
- [Документация OLAP](GLOBAL11-OLAP-DOCUMENTATION.md)
- [Финальная спецификация Global11_demo / Cube_1111](model/CUBE-1111-FINAL-SPECIFICATION.md) — **Documentation Complete; Technical Validation Pending**.
- [Финальная OLAP-схема Cube_1111](model/olap-diagram.mmd)
- [Реляционная модель GLOBAL](model/GLOBAL11-RELATIONAL-MODEL.md)
- [Географическое измерение GEO_ROLLUP](model/geographical-dimension.md) — **Draft**, ключи и иерархия требуют проверки в Oracle Data Modeler.
- [Заметки по восстановлению модели](model/model-recovery-notes.md)
- [Источники и архив Oracle](sources/ORACLE-GLOBAL11-SOURCES.md)
- [DMRS Global11 — анализ метаданных](dmrs-global11-analysis.md)
- [Каталог DMRS metadata tables и hierarchy UUID](dmrs-metadata-catalog.md)
- [Подробная DMRS-диаграмма иерархий](model/olap-diagram-dmrs-source.mmd)
- [Черновые материалы claim-domain](claim-domain-draft/README.md) — отдельная доказательная линия; REST/SOAP-контракты не подтверждают развёрнутые endpoint-ы.
- [Исходные PDF Global11](https://github.com/ekatmoz88-alt/technical-docs-library/tree/main/02_ORACLE) — оригинальные документы и схемы.

## Границы проекта

Этот сайт посвящён только Oracle Global 11g / GLOBAL11_demo. UMMS, Deshifrator, WPF и SQLite документируются в объединённом проектном хабе ППО «Территория» / «Дешифратор» / UMMS и не являются компонентами Global11. UMMS сохраняет собственную границу системы; WPF и SQLite относятся к компонентам Deshifrator.
