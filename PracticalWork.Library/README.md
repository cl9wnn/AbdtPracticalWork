# Cервис управления библиотекой

## Наименование сервиса 

PracticalWork.Library

## Назначение 

Система управления библиотекой с помощью API

## Исполняемые модули

1. PracticalWork.Library.Web - ASP.NET 8 WebApi
2. PracticalWork.Library.Data.PostgreSql.Migrator - запуск миграций

## Интеграции
1. База данных - PostgreSQL
2. Распределенный кэш - Redis
3. Хранение файлов - MinIO
4. Межсервисная коммуникация - Kafka

## Примеры запросов API

### Получить список книг

**GET /api/v1/books**

```
curl -X 'GET' 'http://localhost:8080/api/v1/books' -H 'accept: application/json'
```

### Добавить новую книгу

**POST /api/v1/books**

```
{
    "title": "Краткая история времени",
    "authors": ["Стивен Хокинг"],
    "description": "Исследование природы времени, черных дыр и происхождения Вселенной.",
    "year": 1988,
    "category": 10
  }
```

### Обновить существующую книгу

**PUT /api/v1/books/{id}**

```
{
  "title": "Краткая история времени",
  "authors": [
    "Стивен Хокинг"
  ],
  "description": "Исследование природы времени, черных дыр и происхождения Вселенной.Краткая история планеты Земля во времени.",
  "year": 1988
}
```

### Создать карточку читателя

**POST /api/v1/readers**

```
{
  "fullName": "Иван Гурин",
  "phoneNumber": "+79174998757",
  "expiryDate": "2025-12-26"
}
```

### Получить книги, имеющиеся в библиотеке

**GET /api/v1/library/books**

Пример с фильтрацией и пагинацией:
```
curl -X 'GET' 'http://localhost:8080/api/v1/library/books?AvailableOnly=true&Page=2&PageSize=5' -H 'accept: application/json'
```
