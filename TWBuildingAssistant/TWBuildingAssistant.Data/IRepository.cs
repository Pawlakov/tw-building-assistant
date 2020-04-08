namespace TWBuildingAssistant.Data
{
    using System.Collections.Generic;

    public interface IRepository<T>
    {
        IList<T> DataSet { get; }
    }
}