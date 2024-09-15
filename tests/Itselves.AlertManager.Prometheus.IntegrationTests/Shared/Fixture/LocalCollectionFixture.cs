using Xunit;

namespace Itselves.AlertManager.Prometheus.IntegrationTests.Shared.Fixture;

[CollectionDefinition(nameof(LocalCollectionFixture))]
public sealed class LocalCollectionFixture : ICollectionFixture<LocalFixture>;
