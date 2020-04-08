namespace TWBuildingAssistant.Data.Json
{
    using System.Collections.Generic;
    using TWBuildingAssistant.Data.JsonModel;
    using TWBuildingAssistant.Data.Model;

    public class BuildingLevelLockRepository : IRepository<IBuildingLevelLock>
    {
        private const string JsonFileName = @"Json\twa_data_building_level_locks.json";

        private readonly JsonSource<IBuildingLevelLock, BuildingLevelLock> jsonSource;

        public BuildingLevelLockRepository()
        {
            this.jsonSource = new JsonSource<IBuildingLevelLock, BuildingLevelLock>(JsonFileName);
        }

        public IList<IBuildingLevelLock> DataSet => this.jsonSource.DataSet;
    }
}