# Cервис управления отчетами

## Наименование сервиса 

PracticalWork.Reports

## Назначение 

Система управления отчетами с помощью API: возможность генерации отчетов в формате CSV, получения списка отчетов с информацией об активностях и статистикой библиотеки.

## Исполняемые модули

1. **PracticalWork.Library.Web** - ASP.NET 10
2. **PracticalWork.Library.Data.PostgreSql.Migrator** - запуск миграций

## Интеграции
1. **База данных** - PostgreSQL
2. **Распределенный кэш** - Redis
3. **Хранение файлов** - MinIO
4. **Межсервисная коммуникация** - Kafka

## Примеры запросов API

### Получить список отчетов

**GET /api/v1/reports**

```
curl -X 'GET' 'http://localhost:8082/api/v1/reports' -H 'accept: application/json'
```

### Получить логи активности

**GET /api/v1/activityLogs**

Пример с фильтрацией и пагинацией:
```
curl -X 'GET' \
  'http://localhost:8082/api/v1/activityLogs?EventType=BookCreated&EventDate=2026-05-16&Page=1&PageSize=20' \
  -H 'accept: text/plain'
```


### Сгенерировать отчет с данными об активности библиотеки в формате CSV

**POST /api/v1/reports/generate-activity**

```
curl -X 'POST' 'http://localhost:8082/api/v1/reports/generate-activity?PeriodFrom=2025.12.21&PeriodTo=2025.12.24&EventType=BookCreated' -H 'accept: text/plain' -d ''
```

### Сгенерировать отчет с еженедельной статистикой библиотеки в формате CSV

**POST /api/v1/reports/generate-weekly**

```
curl -X 'POST' \
  'http://localhost:8082/api/v1/reports/generate-weekly' \
  -H 'accept: */*' \
  -H 'Content-Type: application/json' \
  -d '{
  "periodFrom": "2026-05-16",
  "periodTo": "2026-05-16",
  "newBooksCount": 47,
  "newReadersCount": 16,
  "borrowedBooksCount": 2,
  "returnedBooksCount": 1,
  "overdueBooksCount": 3
}'
```




