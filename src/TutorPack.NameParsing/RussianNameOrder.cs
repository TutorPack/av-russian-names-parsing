namespace TutorPack.NameParsing;

/// <summary>Describes the recognized order of parts in the source value.</summary>
public enum RussianNameOrder
{
    /// <summary>The source uses surname followed by given name.</summary>
    SurnameGivenName,

    /// <summary>The source uses given name followed by surname.</summary>
    GivenNameSurname,

    /// <summary>The source uses surname, given name and patronymic.</summary>
    SurnameGivenNamePatronymic,

    /// <summary>The source uses given name, patronymic and surname.</summary>
    GivenNamePatronymicSurname
}
