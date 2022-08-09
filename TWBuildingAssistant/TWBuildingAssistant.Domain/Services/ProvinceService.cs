namespace TWBuildingAssistant.Domain.Services;

using TWBuildingAssistant.Data.Sqlite;

public class ProvinceService
    : IProvinceService
{
    private readonly DatabaseContextFactory contextFactory;

    public ProvinceService(DatabaseContextFactory contextFactory)
    {
        this.contextFactory = contextFactory;
    }
}