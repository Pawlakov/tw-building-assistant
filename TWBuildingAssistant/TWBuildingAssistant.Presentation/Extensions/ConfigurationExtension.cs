namespace TWBuildingAssistant.Presentation.Extensions;

using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using TWBuildingAssistant.Domain;

public static class ConfigurationExtension
{
    private const string CertainValueKey = "CertainValue";
    private const string OtherValueKey = "OtherValue";

    public static string GetCertainValue(this IConfiguration configuration)
    {
        return configuration.GetValue<string>(CertainValueKey);
    }

    public static BuildingLevel GetOtherValue(this IConfiguration configuration)
    {
        var section = configuration.GetSection(OtherValueKey);
        var dictionary = section.GetValue<BuildingLevel>(OtherValueKey);
        if (dictionary == null)
        {
            dictionary = BuildingLevelOperations.Empty;
        }

        return dictionary;
    }

    public static void SetCertainValue(this IConfiguration configuration, string value)
    {
        configuration[CertainValueKey] = value;
        AddOrUpdateAppSettings(CertainValueKey, value);
    }

    public static void SetOtherValue(this IConfiguration configuration, BuildingLevel value)
    {
        configuration[OtherValueKey] = JsonConvert.SerializeObject(value);
        AddOrUpdateAppSettings(OtherValueKey, (dynamic)JsonConvert.DeserializeObject(JsonConvert.SerializeObject(value)));
    }

    private static void AddOrUpdateAppSettings<TValue>(string sectionPathKey, TValue value)
    {
        var filePath = Path.Combine(AppContext.BaseDirectory, "appsettings.json");
        var json = File.ReadAllText(filePath);
        dynamic jsonObj = JsonConvert.DeserializeObject(json);

        SetValueRecursively(sectionPathKey, jsonObj, value);

        var output = JsonConvert.SerializeObject(jsonObj, Formatting.Indented);
        File.WriteAllText(filePath, output);
    }

    private static void SetValueRecursively<TValue>(string sectionPathKey, dynamic jsonObj, TValue value)
    {
        var remainingSections = sectionPathKey.Split(":", 2);
        var currentSection = remainingSections[0];
        if (remainingSections.Length > 1)
        {
            var nextSection = remainingSections[1];
            SetValueRecursively(nextSection, jsonObj[currentSection], value);
        }
        else
        {
            jsonObj[currentSection] = value;
        }
    }
}