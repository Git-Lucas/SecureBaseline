#:property TreatWarningsAsErrors=true
#:property Nullable=enable
#:property ImplicitUsings=enable
// SUP-14/D3: o app nunca e publicado, entao AOT nao agrega nada aqui; sem
// desligar, o lock file passaria a registrar os pacotes ILCompiler/ILLink
// versionados pelo SDK instalado, e um SDK local mais novo que o da CI (via
// rollForward de global.json) quebraria `asvs-matrix` sem nenhuma mudanca
// real de dependencia.
#:property PublishAot=false

using System.Text.Json;
using System.Text.RegularExpressions;

// Valida docs/matriz-asvs.md contra o JSON oficial do ASVS 5.0.0 (GOV-13).
// Executado via `dotnet run tools/asvs-matrix/ValidateAsvsMatrix.cs` a partir
// da raiz do repositório (é onde `dotnet run` resolve os caminhos abaixo).
const string AsvsJsonPath = "tools/asvs-matrix/asvs-5.0.0-en.json";
const string MatrixPath = "docs/matriz-asvs.md";

// V5 (file upload) e V17 (WebRTC) ficam fora de escopo por decisão do projeto
// (GOV-10): a matriz os marca como uma única linha N/A por capítulo, nunca
// requisito por requisito.
var outOfScopeChapters = new HashSet<string> { "V5", "V17" };
var validStatuses = new HashSet<string> { "Implementado", "Pendente", "N/A" };

if (!File.Exists(AsvsJsonPath))
{
    await Console.Error.WriteLineAsync($"Official ASVS JSON not found: {AsvsJsonPath}");
    return 1;
}

if (!File.Exists(MatrixPath))
{
    await Console.Error.WriteLineAsync($"ASVS matrix not found: {MatrixPath}");
    return 1;
}

var problems = new List<string>();

// --- carrega os requisitos oficiais ---
using var jsonStream = File.OpenRead(AsvsJsonPath);
using var document = await JsonDocument.ParseAsync(jsonStream);
var chapters = document.RootElement.GetProperty("Requirements");

var officialLevels = new Dictionary<string, string>();
var inScopeIds = new HashSet<string>();

foreach (var chapter in chapters.EnumerateArray())
{
    var chapterCode = chapter.GetProperty("Shortcode").GetString()
        ?? throw new InvalidOperationException("Chapter without a Shortcode in the official ASVS JSON.");

    foreach (var section in chapter.GetProperty("Items").EnumerateArray())
    {
        foreach (var item in section.GetProperty("Items").EnumerateArray())
        {
            var id = item.GetProperty("Shortcode").GetString()
                ?? throw new InvalidOperationException("Requirement without a Shortcode in the official ASVS JSON.");
            var level = item.GetProperty("L").GetString()
                ?? throw new InvalidOperationException($"Requirement {id} without a level in the official ASVS JSON.");

            officialLevels[id] = level;
            if ((level == "1" || level == "2") && !outOfScopeChapters.Contains(chapterCode))
            {
                inScopeIds.Add(id);
            }
        }
    }
}

// --- percorre as linhas de tabela da matriz versionada ---
// Formato fixo (design D8): | ID | Nivel | Status | Controle | Evidencia | Justificativa |
var rowPattern = new Regex(
    @"^\|\s*(V\d+(?:\.\d+){0,2})\s*\|([^|]*)\|([^|]*)\|([^|]*)\|([^|]*)\|([^|]*)\|\s*$",
    RegexOptions.None,
    TimeSpan.FromSeconds(1));

var seenIds = new HashSet<string>();
var seenChapterRows = new HashSet<string>();

foreach (var line in File.ReadLines(MatrixPath))
{
    var match = rowPattern.Match(line);
    if (!match.Success)
    {
        continue;
    }

    var id = match.Groups[1].Value.Trim();
    var level = match.Groups[2].Value.Trim();
    var status = match.Groups[3].Value.Trim();
    var justification = match.Groups[6].Value.Trim();

    if (outOfScopeChapters.Contains(id))
    {
        // Linha de capítulo inteiro (V5 ou V17), não de requisito individual.
        seenChapterRows.Add(id);
        if (status != "N/A")
        {
            problems.Add($"{id}: out-of-scope chapter must be N/A, found '{status}'.");
        }
        else if (string.IsNullOrWhiteSpace(justification))
        {
            problems.Add($"{id}: N/A row without a justification.");
        }

        continue;
    }

    if (!id.Contains('.'))
    {
        problems.Add($"{id}: unexpected chapter-level shortcode in the matrix.");
        continue;
    }

    if (!seenIds.Add(id))
    {
        problems.Add($"{id}: duplicate ID in the matrix.");
        continue;
    }

    var chapterOfId = id[..id.IndexOf('.')];

    if (!officialLevels.TryGetValue(id, out var officialLevel))
    {
        problems.Add($"{id}: unknown ID (not present in the official ASVS 5.0.0).");
        continue;
    }

    if (officialLevel == "3")
    {
        problems.Add($"{id}: out-of-scope L3 requirement must not appear in the matrix.");
        continue;
    }

    if (outOfScopeChapters.Contains(chapterOfId))
    {
        problems.Add($"{id}: chapter {chapterOfId} is out of scope and must not be expanded per requirement.");
        continue;
    }

    if (level != officialLevel)
    {
        problems.Add($"{id}: level '{level}' does not match the official level '{officialLevel}'.");
    }

    if (!validStatuses.Contains(status))
    {
        problems.Add($"{id}: invalid status '{status}'.");
    }
    else if (status == "N/A" && string.IsNullOrWhiteSpace(justification))
    {
        problems.Add($"{id}: N/A row without a justification.");
    }
}

foreach (var id in inScopeIds.Where(id => !seenIds.Contains(id)))
{
    problems.Add($"{id}: in-scope L1/L2 requirement missing from the matrix.");
}

foreach (var chapter in outOfScopeChapters.Where(chapter => !seenChapterRows.Contains(chapter)))
{
    problems.Add($"{chapter}: out-of-scope chapter row is missing (expected a justified N/A).");
}

if (problems.Count > 0)
{
    await Console.Error.WriteLineAsync($"ASVS matrix validation failed with {problems.Count} problem(s):");
    foreach (var problem in problems.OrderBy(p => p, StringComparer.Ordinal))
    {
        await Console.Error.WriteLineAsync($"  - {problem}");
    }

    return 1;
}

await Console.Out.WriteLineAsync($"ASVS matrix is valid: {seenIds.Count} requirements covered, {inScopeIds.Count} in scope.");
return 0;
