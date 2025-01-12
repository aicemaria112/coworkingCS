using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

[CollectionDefinition("Database collection")]
public class DatabaseCollection : ICollectionFixture<WebApplicationFactory<Program>>
{
    // Esta clase no contiene lógica directa.
    // Solo define la colección y la comparte entre pruebas.
}