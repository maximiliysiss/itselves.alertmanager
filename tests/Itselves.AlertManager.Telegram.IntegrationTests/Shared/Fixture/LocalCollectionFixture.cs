using Xunit;

namespace Itselves.AlertManager.Telegram.IntegrationTests.Shared.Fixture;

[CollectionDefinition(nameof(LocalCollectionFixture))]
public sealed class LocalCollectionFixture : ICollectionFixture<LocalFixture>;
