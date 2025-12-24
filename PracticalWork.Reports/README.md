# Cервис управления отчетами

## Наименование сервиса 

PracticalWork.Reports

## Назначение 

Система управления отчетами с помощью API: возможность генерации отчетов в формате CSV, получения списка отчетов с информацией об активностях библиотеки.

## Исполняемые модули

1. PracticalWork.Library.Web - ASP.NET 8 WebApi
2. PracticalWork.Library.Data.PostgreSql.Migrator - запуск миграций

## Интеграции
1. База данных - PostgreSQL
2. Распределенный кэш - Redis
3. Хранение файлов - MinIO
4. Межсервисная коммуникация - Kafka

## Примеры запросов API

### Получить список отчетов

**GET /api/v1/reports**

```
curl -X 'GET' 'http://localhost:8082/api/v1/reports' -H 'accept: application/json'
```

### Получить логи активности

**GET /api/v1/reports/activity**

Пример с фильтрацией и пагинацией:
```
curl -X 'GET' 'http://localhost:8082/api/v1/reports?EventType=BookCreated&Page=2&PageSize=5' -H 'accept: application/json'
```


### Сгенерировать отчет в формате CSV

**POST /api/v1/reports/generate**

```
curl -X 'POST' 'http://localhost:8082/api/v1/reports/generate?PeriodFrom=2025.12.21&PeriodTo=2025.12.24&EventType=BookCreated' -H 'accept: text/plain' -d ''
```
