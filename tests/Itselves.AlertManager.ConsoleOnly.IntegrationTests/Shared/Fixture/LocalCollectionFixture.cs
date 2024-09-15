using Xunit;

namespace Itselves.AlertManager.ConsoleOnly.IntegrationTests.Shared.Fixture;

[CollectionDefinition(nameof(LocalCollectionFixture))]
public sealed class LocalCollectionFixture : ICollectionFixture<LocalFixture>;
