# Itselves.AlertManager

[![publish](https://github.com/maximiliysiss/itselves.alertmanager/actions/workflows/dotnet.yml/badge.svg?branch=master)](https://github.com/maximiliysiss/itselves.alertmanager/actions/workflows/dotnet.yml)

.NET Library for alert manager

## Install

### Nuget:

`Install-Package Itselves.AlertManager.Abstraction`

## API

`Itselves.AlertManager.Abstraction` is main package contains abstraction `IAlertManager`

```csharp
public interface IAlertManager
{
    Task AlertAsync(Alert alert, CancellationToken cancellationToken);
    Task AlertAsync(string alert, CancellationToken cancellationToken); // Extension
}
```

## Providers

There are several packages for alert manager providers

> You can use several providers in one application. They will be called in parallel.

| Name                                                                                 | Description           |
|--------------------------------------------------------------------------------------|-----------------------|
| [Itselves.AlertManager.ConsoleOnly](src/Itselves.AlertManager.ConsoleOnly/README.md) | Console only provider |
| [Itselves.AlertManager.Prometheus](src/Itselves.AlertManager.Prometheus/README.md)   | Prometheus provider   |
| [Itselves.AlertManager.Mail](src/Itselves.AlertManager.Mail/README.md)               | Mail provider         |
| [Itselves.AlertManager.Telegram](src/Itselves.AlertManager.Telegram/README.md)       | Telegram provider     |
