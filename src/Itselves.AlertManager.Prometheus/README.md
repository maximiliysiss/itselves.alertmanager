# Itselves.AlertManager.Prometheus

.NET library for alert manager which use `Prometheus` like a provider

Based on libraries:

* [Prometheus.Client](https://github.com/prom-client-net/prom-client)

## Install

### Nuget:

`Install-Package Itselves.AlertManager.Prometheus`

### Quick start

* Add `AddPrometheusAlertManager` into DI container with extension

> Do not forget to add `Prometheus.Client` into DI container

```csharp
services
    .AddPrometheusAlertManager(opt => {});
```

* Configure `appsetting.json`

```json
{
    "PrometheusAlertManagerOptions": {
        "MetricOptions": {
            "Name": "alert_metrics",
            "Description": "test alert metrics",
            "Label": "alert"
        },
        "PreInitializeOptions": {
            "PreInitialize": true,
            "Alerts": [
                "alert1",
                "alert2"
            ]
        },
        "InitialDelay": "00:00:30",
        "LogLevel": "Error"
    }
}
```

## Configuration

### PrometheusAlertManagerOptions

1. `MetricOptions` - options for prometheus metrics
2. `PreInitializeOptions` - options for pre init prometheus metrics
3. `InitialDelay` - delay to send first alert to avoid batching events
4. `LogLevel` - log level for alerts

### PrometheusMetricOptions

1. `Name` - name of prometheus metric
2. `Description` - description of prometheus metric
3. `Label` - label for alert name

### PrometheusPreInitializeOptions

1. `PreInitialize` - need to preinit alerts by 0
2. `Alerts` - alert names which should be preinited

## Using in prometheus alert manager

You can create alert in prom-alertmanager by using query like:
```
sum(increase(alert_metrics{alert="alert1"}[10m])) > 0
```

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
