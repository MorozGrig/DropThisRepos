# Вход через Telegram для ASP.NET Core MVC

## Как работает Telegram Login Widget
1. На странице входа размещается виджет Telegram с именем вашего бота.
2. Пользователь подтверждает вход в Telegram.
3. Telegram возвращает на ваш callback данные пользователя (`id`, `first_name`, `username`, `photo_url`, `auth_date`, `hash`).
4. Сервер проверяет подпись `hash` по токену бота.
5. После проверки сервер:
   - создаёт нового пользователя, если он не найден;
   - или привязывает Telegram ID к существующему аккаунту.

## Пример кнопки на странице входа
```html
<script async src="https://telegram.org/js/telegram-widget.js?22"
        data-telegram-login="my_shop_auth_bot"
        data-size="large"
        data-auth-url="https://your-domain.com/account/telegram-callback"
        data-request-access="write"></script>
```

## Пример DTO и проверки подписи на сервере
```csharp
public class TelegramAuthDto
{
    public long id { get; set; }
    public string? first_name { get; set; }
    public string? username { get; set; }
    public string? photo_url { get; set; }
    public long auth_date { get; set; }
    public string hash { get; set; } = string.Empty;
}

private bool ValidateTelegramAuth(TelegramAuthDto dto, string botToken)
{
    using var sha = System.Security.Cryptography.SHA256.Create();
    var secretKey = sha.ComputeHash(System.Text.Encoding.UTF8.GetBytes(botToken));

    var data = new SortedDictionary<string, string?>
    {
        ["auth_date"] = dto.auth_date.ToString(),
        ["first_name"] = dto.first_name,
        ["id"] = dto.id.ToString(),
        ["photo_url"] = dto.photo_url,
        ["username"] = dto.username
    }
    .Where(kv => !string.IsNullOrWhiteSpace(kv.Value))
    .Select(kv => $"{kv.Key}={kv.Value}");

    var dataCheckString = string.Join("\n", data);

    using var hmac = new System.Security.Cryptography.HMACSHA256(secretKey);
    var hashBytes = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(dataCheckString));
    var expectedHash = Convert.ToHexString(hashBytes).ToLowerInvariant();

    return expectedHash == dto.hash.ToLowerInvariant();
}
```

## Пример callback-метода в контроллере
```csharp
[HttpGet]
public async Task<IActionResult> TelegramCallback([FromQuery] TelegramAuthDto dto)
{
    if (!ValidateTelegramAuth(dto, _configuration["Telegram:BotToken"]!))
        return Unauthorized();

    var user = await _context.Users.FirstOrDefaultAsync(u => u.TelegramId == dto.id);

    if (user == null)
    {
        user = new User
        {
            Login = dto.username ?? $"tg_{dto.id}",
            Email = $"tg_{dto.id}@telegram.local",
            Phone = string.Empty,
            Password = Guid.NewGuid().ToString("N"),
            IdRole = 2,
            TelegramId = dto.id
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();
    }

    // Далее стандартная cookie-аутентификация
    // и вход пользователя в систему.
    return RedirectToAction("Index", "Home");
}
```

## Архитектура
- **Клиент**: страница входа с Telegram-виджетом.
- **Сервер**: endpoint callback + валидация подписи.
- **База данных**: поле `TelegramId` в таблице пользователей.
- **Логика входа**: поиск/создание пользователя + выдача auth-cookie.
