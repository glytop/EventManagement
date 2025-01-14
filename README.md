## Настройка проекта

### Шаг 1. Клонирование репозитория

Склонируйте проект на локальную машину:

```bash
git clone https://github.com/glytop/EventManagement.git
```

### Шаг 2. Настройка базы данных

1. Откройте файл `appsettings.json` и настройте строку подключения к вашей базе данных:

```json
"ConnectionStrings": {
  "DefaultConnection": "YourConnectionString"
}
```

2. В консоли Package Manager Console выполните миграции для создания базы данных:

```bash
Update-Database
```

### Шаг 3. Запуск приложения

1. Убедитесь, что проект **EventsWebApp.API** выбран в качестве стартового.
2. Запустите приложение

### Шаг 4. Тестирование API

Для работы с API используйте **Postman** или другой клиент для отправки HTTP-запросов.

1. **Получение JWT-токена**:

   - Отправьте POST-запрос с запущенным проектом на `http://localhost:{ваши цифры}/api/Auth/register` с телом:
     ```json
     {
       "firstName": "name`",
       "lastName": "lastName",
       "email": "email@email.com",
       "passwordHash": "password",
       "dateOfBirth": "2004-01-01T00:00:00"
     }
     ```
     - Отправьте POST-запрос с запущенным проектом на `http://localhost:{ваши цифры}/api/Auth/login` с телом:
     ```json
     {
       "email": "email@email.com",
       "password": "password"
     }
     ```
   - Ответ будет содержать JWT-токен.

2. **Использование токена**:

   - В Authorization выберете в Auth Type **Bearer Tocken**, вставьте ваш JWT-токен
