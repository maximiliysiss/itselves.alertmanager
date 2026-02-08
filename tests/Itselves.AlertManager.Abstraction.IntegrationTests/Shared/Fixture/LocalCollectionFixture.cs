using Xunit;

namespace Itselves.AlertManager.Abstraction.IntegrationTests.Shared.Fixture;

[CollectionDefinition(nameof(LocalCollectionFixture))]
public sealed class LocalCollectionFixture : ICollectionFixture<LocalFixture>;
