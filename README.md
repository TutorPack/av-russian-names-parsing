# TutorPack.NameParsing

Публичная .NET-библиотека для консервативного офлайн-разбора русскоязычных ФИО.
Она распознаёт распространённые варианты порядка частей имени и возвращает
структурированный результат. При неоднозначности исходная строка не меняется.

Исходный код находится в `TutorPack/av-russian-names-parsing`. Пакет
`TutorPack.NameParsing` публикуется на NuGet.org. Для его установки используется
обычный источник NuGet.org без токена:

```sh
dotnet add package TutorPack.NameParsing --version 0.1.0
```

```csharp
var result = RussianNameAnalyzer.Analyze("Аня Нелюбина");
// result.Surname == "Нелюбина"
// result.GivenName == "Аня"

var displayName = RussianNameAnalyzer.FormatSurnameAndGivenName("Ильяс Исаев");
// "Исаев Ильяс"
```

Справочники встроены в сборку, поэтому библиотеке не нужны Python, база данных
или сетевые запросы во время работы. Собственный код распространяется по лицензии
MIT. Происхождение и лицензия данных описаны в
[`THIRD-PARTY-NOTICES.md`](https://github.com/TutorPack/av-russian-names-parsing/blob/main/src/TutorPack.NameParsing/THIRD-PARTY-NOTICES.md).
