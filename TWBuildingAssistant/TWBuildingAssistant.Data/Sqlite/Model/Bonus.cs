﻿namespace TWBuildingAssistant.Data.Sqlite.Model
{
    using TWBuildingAssistant.Data.Model;

    public class Bonus
    {
        public int Id { get; set; }

        public int Value { get; set; }

        public IncomeCategory Category { get; set; }

        public BonusType Type { get; set; }

        public int? RegionalEffectId { get; set; }

        public int? ProvincialEffectId { get; set; }
    }
}