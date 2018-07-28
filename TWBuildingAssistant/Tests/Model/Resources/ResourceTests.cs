﻿namespace Tests.Model.Resources
{
    using NUnit.Framework;

    using TWBuildingAssistant.Model.Resources;

    /// <summary>
    /// Test fixture containing tests of the <see cref="Resource"/> class.
    /// </summary>
    [TestFixture]
    public class ResourceTests
    {
        /// <summary>
        /// Checks whether creation of <see cref="Resource"/> objects undergoes correctly.
        /// </summary>
        /// <param name="name">
        /// The value used to set <see cref="Resource.Name"/>.
        /// </param>
        /// <param name="buildingType">
        /// The value used to set <see cref="Resource.BuildingType"/>.
        /// </param>
        /// <param name="isMandatory">
        /// The value used to set <see cref="Resource.IsMandatory"/>.
        /// </param>
        [TestCase("Iron", SlotType.Regular, false)]
        [TestCase("Anything", SlotType.Main, true)]
        [TestCase("Iron", SlotType.Regular, true)]
        [TestCase("", SlotType.Main, true)]
        [TestCase(null, SlotType.Coastal, true)]
        public void Creation(string name, SlotType buildingType, bool isMandatory)
        {
            IResource created = new Resource { Name = name, BuildingType = buildingType, IsMandatory = isMandatory };
            Assert.AreEqual(name, created.Name, $"The {nameof(Resource.Name)} property was not set correctly.");
            Assert.AreEqual(buildingType, created.BuildingType, $"The {nameof(Resource.BuildingType)} property was not set correctly.");
            Assert.AreEqual(isMandatory, created.IsMandatory, $"The {nameof(Resource.IsMandatory)} property was not set correctly.");
        }

        /// <summary>
        /// Checks wheter validation of <see cref="Resource"/>'s values undergoes correctly.
        /// </summary>
        /// <param name="name">
        /// The value used to set <see cref="Resource.Name"/>.
        /// </param>
        /// <param name="buildingType">
        /// The value used to set <see cref="Resource.BuildingType"/>.
        /// </param>
        /// <param name="isMandatory">
        /// The value used to set <see cref="Resource.IsMandatory"/>.
        /// </param>
        /// <param name="expectedResult">
        /// The expected result of validation.
        /// </param>
        [TestCase("Iron", SlotType.Regular, false, true)]
        [TestCase("Anything", SlotType.Main, true, true)]
        [TestCase("Iron", SlotType.Regular, true, false)]
        [TestCase("", SlotType.Main, true, false)]
        [TestCase(null, SlotType.Coastal, true, false)]
        public void Validation(string name, SlotType buildingType, bool isMandatory, bool expectedResult)
        {
            IResource created = new Resource { Name = name, BuildingType = buildingType, IsMandatory = isMandatory };
            Assert.AreEqual(expectedResult, created.Validate(out _), $"The {nameof(Resource.Validate)} method returned unexpected value.");
        }

        /// <summary>
        /// The to string.
        /// </summary>
        /// <param name="name">
        /// The value used to set <see cref="Resource.Name"/>.
        /// </param>
        /// <param name="buildingType">
        /// The value used to set <see cref="Resource.BuildingType"/>.
        /// </param>
        /// <param name="isMandatory">
        /// The value used to set <see cref="Resource.IsMandatory"/>.
        /// </param>
        [TestCase("Iron", SlotType.Regular, false)]
        [TestCase("Anything", SlotType.Main, true)]
        [TestCase("Iron", SlotType.Regular, true)]
        [TestCase("", SlotType.Main, true)]
        public void ToString(string name, SlotType buildingType, bool isMandatory)
        {
            IResource created = new Resource { Name = name, BuildingType = buildingType, IsMandatory = isMandatory };
            Assert.True(name.Equals(created.ToString()), $"The {nameof(Resource.ToString)} method returned a string that isn't the object's name.");
        }
    }
}