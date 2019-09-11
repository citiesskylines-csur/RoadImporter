using ICities;

namespace RoadImporter
{
    public class RoadImporterMod : IUserMod
    {
        public string Name => "Road Importer";
        public string Description => "Automated creation of roads in Cities: Skylines. Imports and creates road assets from pre-built configuration file";
    }
}
