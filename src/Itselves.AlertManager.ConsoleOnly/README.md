# Itselves.AlertManager.ConsoleOnly

.NET library for alert manager which use console like an output

## Install

### Nuget:

`Install-Package Itselves.AlertManager.ConsoleOnly`

### Quick start

* Add `AddConsoleOnlyAlertManager` into DI container with extension

```csharp
services
    .AddConsoleOnlyAlertManager(opt => {});
```

* Configure `appsetting.json`

```json
{
    "ConsoleOnlyAlertManagerOptions": {
        "LogLevel": "Error"
    }
}
```

## Configuration

### ConsoleOnlyAlertManagerOptions

1. `LogLevel` - log level for alerts

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
