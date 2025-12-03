# Общая информация

## Наименование сервиса 

PracticalWork.Library

## Назначение 

Система управления библиотекой

## Исполняемые модули

1. PracticalWork.Library.Web - ASP.NET 8 WebApi
2. PracticalWork.Library.Data.PostgreSql.Migrator - запуск миграций

## Интеграции
1. База данных - PostgreSQL
2. Распределенный кэш - Redis
3. Хранение файлов - MinIO

<br>

# Развертывание и конфигурирование сервиса

### 1. Клонирование репозитория

```
git clone https://github.com/cl9wnn/AbdtPracticalWork.git
```

### 2. Конфигурация окружения

Заполните необходимые переменные в файле **appsettings.json** в проекте или создайте файл **.env** (для развертывания в Docker)

### 3. Запуск через Docker (рекомендуется)

Перейдите в каталог со cклонированным репозиторием и выполните команду, чтобы запустить всю ифнраструктуру:

```
docker-compose up --build
```

После запуска инфраструктуры сервис можно запустить локально в любой IDE.

### 4. Локальный запуск

```
dotnet restore
dotnet ef database update
dotnet run
```

<br>

# Примеры запросов API

### Получить список книг

**GET /api/v1/books**

```
curl -X 'GET' 'http://localhost:5251/api/v1/books' -H 'accept: application/json'
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
curl -X 'GET' 'http://localhost:5251/api/v1/library/books?AvailableOnly=true&Page=2&PageSize=5' -H 'accept: application/json'
```
