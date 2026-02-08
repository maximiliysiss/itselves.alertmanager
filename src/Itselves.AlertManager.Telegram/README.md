# Itselves.AlertManager.Telegram

.NET library for alert manager which uses Telegram bot as a provider

## Install

### Nuget:

`Install-Package Itselves.AlertManager.Telegram`

### Quick start

* Add `AddTelegramAlertManager` into DI container with extension

```csharp
services
    .AddTelegramAlertManager(opt => {});
```

* Configure `appsetting.json`

```json
{
    "TelegramAlertManagerOptions": {
        "BotToken": "123456:ABC-DEF",
        "ChatIds": [
            "123456789"
        ],
        "Message": "Alert: {Alert} at {UtcNow}",
        "ParseMode": "MarkdownV2",
        "DisableNotification": false,
        "LogLevel": "Error",
        "BaseUrl": "https://api.telegram.org"
    }
}
```

## Configuration

### TelegramAlertManagerOptions

1. `BotToken` - Telegram bot token
2. `ChatIds` - list of chat IDs to notify
3. `Message` - message template (use `{Alert}` or `{UtcNow}`)
4. `ParseMode` - parse mode (e.g. `MarkdownV2`, `HTML`)
5. `DisableNotification` - disable notification sound
6. `LogLevel` - log level for alerts
7. `BaseUrl` - base URL for the Telegram Bot API server (optional)

## Using example

### 1. Call with existing alert

```csharp
await alertManager.AlertAsync(new Alert("name"), default);
```

### 2. Call with custom alert

```csharp
await alertManager.AlertAsync(new CustomAlert(), default);

public sealed class CustomAlert : Alert
{
    public CustomAlert() : base("custom") {}
}
```

### 3. Call with a string alert name

```csharp
await alertManager.AlertAsync("name", default);
```
