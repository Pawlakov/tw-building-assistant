namespace TWBuildingAssistant.Model
{
    using System.Collections.Generic;
    using System.Linq;

    //public static class Extensions
    //{
    //    public static IEnumerable<IInfluence> TakeWorst(this IEnumerable<IInfluence> first, IEnumerable<IInfluence> other)
    //    {
    //        var combined = first.Concat(other).ToArray();
    //        var religions = combined.Select(x => x.GetReligion()).Distinct();
    //        return religions.Select(religion => combined.Where(x => x.GetReligion() == religion).OrderBy(x => x.Value).First()).ToList();
    //    }

    //    public static IEnumerable<IBonus> TakeWorst(this IEnumerable<IBonus> first, IEnumerable<IBonus> other)
    //    {
    //        var combined = first.Concat(other).ToArray();
    //        var categories = combined.Select(x => x.Category).Distinct();
    //        return categories.Select(religion => combined.Where(x => x.Category == religion).OrderBy(x => x.Value).First()).ToList();
    //    }
    //}
}