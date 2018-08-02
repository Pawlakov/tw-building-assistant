namespace TWBuildingAssistant.Model.Religions
{
    using System.Collections.Generic;

    public interface IReligionsSource
    {
        IEnumerable<IReligion> GetReligions();
    }
}