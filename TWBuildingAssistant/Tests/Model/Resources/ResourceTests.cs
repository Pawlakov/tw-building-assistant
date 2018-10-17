namespace Tests.Model.Resources
{
    using NUnit.Framework;

    using TWBuildingAssistant.Model.Resources;
    
    [TestFixture]
    public class ResourceTests
    {
        [TestCase("Iron", SlotType.Regular, false)]
        [TestCase("Anything", SlotType.Main, true)]
        public void ValidateValidTest(string name, SlotType buildingType, bool isMandatory)
        {
            var created = new Resource { Name = name, BuildingType = buildingType, Obligatory = isMandatory };
            Assert.AreEqual(true, created.Validate(out _), $"The {nameof(Resource.Validate)} method returned false.");
        }
        
        [TestCase("Iron", SlotType.Regular, true)]
        [TestCase("", SlotType.Main, true)]
        [TestCase(null, SlotType.Coastal, true)]
        public void ValidateInvalidTest(string name, SlotType buildingType, bool isMandatory)
        {
            var created = new Resource { Name = name, BuildingType = buildingType, Obligatory = isMandatory };
            Assert.AreEqual(false, created.Validate(out _), $"The {nameof(Resource.Validate)} method returned true.");
        }
    }
}