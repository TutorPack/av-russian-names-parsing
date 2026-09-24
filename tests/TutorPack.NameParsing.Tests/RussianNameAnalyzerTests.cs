using TutorPack.NameParsing;

namespace TutorPack.NameParsing.Tests;

public sealed class RussianNameAnalyzerTests
{
    [Theory]
    [InlineData("Аня Нелюбина", "Нелюбина Аня")]
    [InlineData("Ильяс Исаев", "Исаев Ильяс")]
    [InlineData("Алиса Болдырева", "Болдырева Алиса")]
    [InlineData("Бондарченко Егор", "Бондарченко Егор")]
    [InlineData("Гойдин Никита Алексеевич", "Гойдин Никита")]
    [InlineData("Никита Алексеевич Гойдин", "Гойдин Никита")]
    [InlineData("Пётр Ильич Иванов", "Иванов Пётр")]
    [InlineData("  Анна   Иванова-Петрова  ", "Иванова-Петрова Анна")]
    [InlineData("Анна-Мария Иванова", "Иванова Анна-Мария")]
    [InlineData("Дима Краснощёков", "Краснощёков Дима")]
    [InlineData("Маша Красильникова", "Красильникова Маша")]
    [InlineData("Сулейманов Гарун", "Сулейманов Гарун")]
    [InlineData("Амир Курмангалиев", "Курмангалиев Амир")]
    [InlineData("Эльмир Аббасов", "Аббасов Эльмир")]
    public void RecognizedNames_UseSurnameThenGivenName(string source, string expected) =>
        Assert.Equal(expected, RussianNameAnalyzer.FormatSurnameAndGivenName(source));

    [Theory]
    [InlineData("Седракян")]
    [InlineData("nkossova")]
    [InlineData("student@example.test")]
    [InlineData("Иван Петрович")]
    [InlineData("Иван Алексей")]
    [InlineData("Jean Claude Van Damme")]
    [InlineData("John Smith")]
    [InlineData("Anna Иванова")]
    [InlineData("Иван И. Петров")]
    [InlineData("Иван 123")]
    [InlineData("  неизвестное имя  ")]
    [InlineData("Иванов Иван оглы")]
    [InlineData("")]
    public void UncertainNames_PreserveOriginal(string source) =>
        Assert.Equal(source, RussianNameAnalyzer.FormatSurnameAndGivenName(source));

    [Fact]
    public void Analyze_ReturnsStructuredPartsAndSourceOrder()
    {
        var result = RussianNameAnalyzer.Analyze("Ильяс Исаев");

        Assert.True(result.IsRecognized);
        Assert.Equal("Исаев", result.Surname);
        Assert.Equal("Ильяс", result.GivenName);
        Assert.Null(result.Patronymic);
        Assert.Equal(RussianNameOrder.GivenNameSurname, result.SourceOrder);
    }
}
