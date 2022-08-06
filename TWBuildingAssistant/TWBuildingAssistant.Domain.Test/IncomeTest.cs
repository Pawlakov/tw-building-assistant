namespace TWBuildingAssistant.Domain.Test;

using NUnit.Framework;
using TWBuildingAssistant.Data.Model;
using TWBuildingAssistant.Domain.Models;

[TestFixture]
public class IncomeTest
{
    [Test]
    public void SimpleBonus()
    {
        var bonus = IncomeOperations.Create(50, IncomeCategory.Industry, BonusType.Simple);

        var income = IncomeOperations.GetIncome(bonus, 3);

        Assert.AreEqual(50, income);
    }
}