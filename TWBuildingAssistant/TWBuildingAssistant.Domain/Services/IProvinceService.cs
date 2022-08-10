namespace TWBuildingAssistant.Domain.Services;

using System.Collections.Generic;
using System.Collections.Immutable;
using System.Threading.Tasks;
using TWBuildingAssistant.Data.Model;
using TWBuildingAssistant.Domain.OldModels;
using TWBuildingAssistant.Domain.StateModels;

public interface IProvinceService
{
    Task<string> GetProvinceName(int provinceId);

    ProvinceState GetState(
        Province province,
        in Settings settings,
        Effect predefinedEffect,
        ImmutableArray<Income> predefinedIncomes,
        ImmutableArray<Influence> predefinedInfluences);
}