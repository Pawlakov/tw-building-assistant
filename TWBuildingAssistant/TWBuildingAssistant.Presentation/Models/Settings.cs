namespace TWBuildingAssistant.Presentation.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public record struct Settings(int ProvinceId, int FertilityDrop, int TechnologyTier, bool UseAntilegacyTechnologies, int ReligionId, int FactionId, int WeatherId, int SeasonId, int DifficultyId, int TaxId, int CorruptionRate, int PiracyRate);