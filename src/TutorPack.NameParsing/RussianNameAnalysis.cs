namespace TutorPack.NameParsing;

/// <summary>Contains a conservative analysis of a display name.</summary>
public sealed record RussianNameAnalysis(
    string OriginalText,
    bool IsRecognized,
    string? Surname = null,
    string? GivenName = null,
    string? Patronymic = null,
    RussianNameOrder? SourceOrder = null)
{
    /// <summary>Returns “Surname GivenName” when recognized, otherwise the source value unchanged.</summary>
    public string DisplayName => IsRecognized
        && !string.IsNullOrWhiteSpace(Surname)
        && !string.IsNullOrWhiteSpace(GivenName)
            ? $"{Surname} {GivenName}"
            : OriginalText;
}
