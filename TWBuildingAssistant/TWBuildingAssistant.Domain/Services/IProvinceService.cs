﻿namespace TWBuildingAssistant.Domain.Services;

using System.Collections.Generic;
using System.Threading.Tasks;
using TWBuildingAssistant.Domain;
using TWBuildingAssistant.Domain.StateModels;

public interface IProvinceService
{
    Task<Province> GetProvince(int provinceId);

    ProvinceState GetState(
        IEnumerable<IEnumerable<BuildingLevel>> buildings,
        in Settings settings,
        in EffectSet predefinedEffect);
}