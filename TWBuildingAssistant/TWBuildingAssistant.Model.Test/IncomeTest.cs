﻿namespace TWBuildingAssistant.Model.Test
{
    using NUnit.Framework;
    using TWBuildingAssistant.Data.Model;

    [TestFixture]
    public class IncomeTest
    {
        [Test]
        public void SimpleBonus()
        {
            var bonus = new Income(50, IncomeCategory.Industry, BonusType.Simple);

            var income = bonus.GetIncome(3);

            Assert.AreEqual(50, income);
        }
    }
}