namespace TWBuildingAssistant.Data
{
    using System.Collections.Generic;

    public interface IRepository<T>
    {
        IEnumerable<T> DataSet { get; }
    }
}