using Xunit;

namespace Itselves.AlertManager.Mail.IntegrationTests.Shared.Fixture;

[CollectionDefinition(nameof(LocalCollectionFixture))]
public sealed class LocalCollectionFixture : ICollectionFixture<LocalFixture>;
