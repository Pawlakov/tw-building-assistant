namespace TWBuildingAssistant.Data.Test
{
    using System.Linq;
    using NUnit.Framework;
    using TWBuildingAssistant.Data.Json;

    public abstract class JsonRepositoryTestBase
    {
        protected IRepository<TModel> GetRepository<TModel>()
        {
            IRepository<TModel> source = null;
            try
            {
                source = new JsonRepository<TModel>();
            }
            catch
            {
                Assert.Inconclusive();
            }

            if (source.DataSet.Count() == 0)
            {
                Assert.Inconclusive();
            }

            return source;
        }
    }
}