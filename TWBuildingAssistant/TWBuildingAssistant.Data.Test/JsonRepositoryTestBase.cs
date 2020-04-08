namespace TWBuildingAssistant.Data.Test
{
    using NUnit.Framework;
    using TWBuildingAssistant.Data.Test.Utils;

    public abstract class JsonRepositoryTestBase
    {
        protected IRepository<T> GetRepository<T>()
        {
            IRepository<T> source = null;
            try
            {
                source = JsonRepositoryResolver.Instance.Resolve<T>();
            }
            catch
            {
                Assert.Inconclusive();
            }

            if (source.DataSet.Count == 0)
            {
                Assert.Inconclusive();
            }

            return source;
        }
    }
}