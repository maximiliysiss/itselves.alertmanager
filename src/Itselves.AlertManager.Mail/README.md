# Itselves.AlertManager.Mail

.NET library for alert manager which use SMTP mail as a provider

## Install

### Nuget:

`Install-Package Itselves.AlertManager.Mail`

### Quick start

* Add `AddMailAlertManager` into DI container with extension

```csharp
services
    .AddMailAlertManager(opt => {});
```

* Configure `appsetting.json`

```json
{
    "MailAlertManagerOptions": {
        "Host": "smtp.example.com",
        "Port": 587,
        "EnableSsl": true,
        "UseDefaultCredentials": false,
        "Username": "alert@example.com",
        "Password": "secret",
        "From": "alert@example.com",
        "To": [
            "ops@example.com"
        ],
        "Subject": "Alert: {Alert}",
        "Body": "Alert: {Alert} at {UtcNow}",
        "IsBodyHtml": false,
        "LogLevel": "Error"
    }
}
```

## Configuration

### MailAlertManagerOptions

1. `Host` - SMTP host
2. `Port` - SMTP port
3. `EnableSsl` - enable SSL/TLS
4. `UseDefaultCredentials` - use default process credentials
5. `Username` - SMTP username
6. `Password` - SMTP password
7. `From` - sender address
8. `To` - recipients list
9. `Subject` - subject template (use `{Alert}` or `{UtcNow}`)
10. `Body` - body template (use `{Alert}` or `{UtcNow}`)
11. `IsBodyHtml` - body content type
12. `LogLevel` - log level for alerts

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

### 3. Call with string alert name

```csharp
await alertManager.AlertAsync("name", default);
```
