# TutorPack.NameParsing

Публичная .NET-библиотека для консервативного офлайн-разбора русскоязычных ФИО.
Она распознаёт распространённые варианты порядка частей имени и возвращает
структурированный результат. При неоднозначности исходная строка не меняется.

Исходный код находится в `TutorPack/av-russian-names-parsing`. Пакет
`TutorPack.NameParsing` публикуется в GitHub Packages организации `TutorPack`.
GitHub требует авторизацию для установки даже публичных NuGet-пакетов:
добавьте источник `https://nuget.pkg.github.com/TutorPack/index.json` и передайте
токен с правом `read:packages` через настройки NuGet, не сохраняя его в репозитории.

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
