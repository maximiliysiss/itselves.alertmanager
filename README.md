# Itselves.AlertManager

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

| Name                                                                               | Description         |
|------------------------------------------------------------------------------------|---------------------|
| [Itselves.AlertManager.Prometheus](src/Itselves.AlertManager.Prometheus/README.md) | Prometheus provider |
