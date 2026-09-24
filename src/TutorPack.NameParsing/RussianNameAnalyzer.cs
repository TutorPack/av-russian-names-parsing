using System.Threading;

namespace TutorPack.NameParsing;

/// <summary>Recognizes common Russian full-name layouts using embedded open reference data.</summary>
public static class RussianNameAnalyzer
{
    private static readonly Lazy<NameReferenceData> ReferenceData = new(
        () => NameReferenceData.LoadAsync().GetAwaiter().GetResult(),
        LazyThreadSafetyMode.ExecutionAndPublication);

    /// <summary>Analyzes a value without changing it when its structure is uncertain.</summary>
    public static RussianNameAnalysis Analyze(string fullName)
    {
        ArgumentNullException.ThrowIfNull(fullName);
        var parts = fullName.Split((char[]?)null, StringSplitOptions.RemoveEmptyEntries);
        if (parts.Length is < 2 or > 3 || !parts.All(IsNamePart))
            return Unknown(fullName);

        var data = ReferenceData.Value;
        if (parts.Length == 2)
            return AnalyzeTwoParts(fullName, parts, data);

        return AnalyzeThreeParts(fullName, parts, data);
    }

    /// <summary>Formats a recognized value as “Surname GivenName” and preserves uncertain input.</summary>
    public static string FormatSurnameAndGivenName(string fullName) => Analyze(fullName).DisplayName;

    private static RussianNameAnalysis AnalyzeTwoParts(
        string source, string[] parts, NameReferenceData data)
    {
        if (parts.Any(part => IsPatronymic(part, data)))
            return Unknown(source);
        var surnameFirst = IsSurname(parts[0], data) && IsGivenName(parts[1], data);
        var givenNameFirst = IsGivenName(parts[0], data) && IsSurname(parts[1], data);
        if (surnameFirst == givenNameFirst)
            return Unknown(source);

        return surnameFirst
            ? Recognized(source, parts[0], parts[1], null, RussianNameOrder.SurnameGivenName)
            : Recognized(source, parts[1], parts[0], null, RussianNameOrder.GivenNameSurname);
    }

    private static RussianNameAnalysis AnalyzeThreeParts(
        string source, string[] parts, NameReferenceData data)
    {
        var surnameFirst = IsSurname(parts[0], data)
            && IsGivenName(parts[1], data)
            && IsPatronymic(parts[2], data);
        var givenNameFirst = IsGivenName(parts[0], data)
            && IsPatronymic(parts[1], data)
            && IsSurname(parts[2], data);
        if (surnameFirst == givenNameFirst)
            return Unknown(source);

        return surnameFirst
            ? Recognized(source, parts[0], parts[1], parts[2], RussianNameOrder.SurnameGivenNamePatronymic)
            : Recognized(source, parts[2], parts[0], parts[1], RussianNameOrder.GivenNamePatronymicSurname);
    }

    private static bool IsGivenName(string value, NameReferenceData data) =>
        HasRole(value, data.GivenNames);

    private static bool IsSurname(string value, NameReferenceData data) =>
        HasRole(value, data.Surnames);

    private static bool HasRole(string value, IReadOnlySet<string> values) =>
        value.Split('-').Select(NameReferenceData.NormalizeKey).All(values.Contains);

    private static bool IsPatronymic(string value, NameReferenceData data) =>
        !value.Equals("оглы", StringComparison.OrdinalIgnoreCase)
        && !value.Equals("кызы", StringComparison.OrdinalIgnoreCase)
        && value.Split('-').Select(NameReferenceData.NormalizeKey).All(data.Patronymics.Contains);

    private static bool IsNamePart(string value)
    {
        var components = value.Split('-');
        return components.All(component => component.Length > 1 && component.All(character =>
            character is >= 'А' and <= 'я' or 'Ё' or 'ё'));
    }

    private static RussianNameAnalysis Recognized(
        string source, string surname, string givenName, string? patronymic, RussianNameOrder order) =>
        new(source, true, surname, givenName, patronymic, order);

    private static RussianNameAnalysis Unknown(string source) => new(source, false);
}
