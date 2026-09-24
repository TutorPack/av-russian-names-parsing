using System.Reflection;
using Parquet;
using Parquet.Schema;

namespace TutorPack.NameParsing;

internal sealed class NameReferenceData(
    IReadOnlySet<string> givenNames,
    IReadOnlySet<string> surnames,
    IReadOnlySet<string> patronymics)
{
    private const string ResourcePrefix = "TutorPack.NameParsing.Data";

    internal IReadOnlySet<string> GivenNames { get; } = givenNames;

    internal IReadOnlySet<string> Surnames { get; } = surnames;

    internal IReadOnlySet<string> Patronymics { get; } = patronymics;

    internal static async Task<NameReferenceData> LoadAsync()
    {
        var givenNames = await LoadValuesAsync("names");
        var surnames = await LoadValuesAsync("surnames");
        var patronymics = await LoadValuesAsync("midnames");
        return new(givenNames, surnames, patronymics);
    }

    private static async Task<HashSet<string>> LoadValuesAsync(string dataset)
    {
        await using var stream = OpenDataset(dataset);
        await using var reader = await ParquetReader.CreateAsync(stream);
        var textField = FindField(reader, "text");
        var values = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        for (var index = 0; index < reader.RowGroupCount; index++)
        {
            using var rowGroup = reader.OpenRowGroupReader(index);
            var texts = new string?[(int)rowGroup.RowCount];
            await rowGroup.ReadAsync(textField, texts);
            foreach (var text in texts)
            {
                if (!string.IsNullOrWhiteSpace(text))
                    values.Add(NormalizeKey(text));
            }
        }
        return values;
    }

    private static Stream OpenDataset(string dataset) =>
        Assembly.GetExecutingAssembly().GetManifestResourceStream($"{ResourcePrefix}.{dataset}.parquet")
        ?? throw new InvalidOperationException($"Embedded name dataset '{dataset}' was not found.");

    private static DataField FindField(ParquetReader reader, string name) =>
        reader.Schema.DataFields.Single(field => field.Name.Equals(name, StringComparison.OrdinalIgnoreCase));

    internal static string NormalizeKey(string value) => value.Replace('ё', 'е').Replace('Ё', 'Е');
}
