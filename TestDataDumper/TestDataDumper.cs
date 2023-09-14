using System.Text;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;

namespace TestDataDumper;

/// <summary>
/// The test data dumper.
/// </summary>
public class TestDataDumper
{
    private static readonly ILogger<TestDataDumper> Logger = TestDataDumperLoggerFactory.CreateLogger<TestDataDumper>();

    /// <summary>
    ///     Creates a file with that will expose a Load{ClassName} method that returns a list of {ClassName} objects.
    /// </summary>
    /// <param name="exportPath">The path of the folder where the file will be created.</param>
    /// <param name="nameSpace">The namespace of your class.</param>
    /// <param name="className">The name of the class.</param>
    /// <param name="data">The dumped data.</param>
    /// <param name="addExcludeFromCodeCoverageAttribute">
    ///     (Optional) If <c>true</c> the exclude from code coverage attribute will be added to the class.</param>
    /// <param name="suffix">
    ///     (Optional) The suffix that is added after the class name. For example, if the class name is Person
    ///     and the suffix is Data then the file name will be PersonData.cs
    /// </param>
    /// <param name="alternativeMethodName">
    ///     (Optional) The default load method is Load{ClassName}. If an alternative method name is specified the
    ///     load method will be Load{AlternativeMethodName}.
    /// </param>
    /// <param name="additionalNamespaces">A list of additional namespaces to be imported in the class.</param>
    /// <returns>Returns <c>true</c> if the data is created successfully.</returns>
    public static bool CreateClass(string exportPath, string nameSpace, string className, string data,
        bool addExcludeFromCodeCoverageAttribute = false, string suffix = "Data", string? alternativeMethodName = null,
        IEnumerable<string>? additionalNamespaces = null)
    {
        Logger.LogInformation("Creating the {ClassName} class", className);
        try
        {
            return TryCreateClass(exportPath, nameSpace, className, data, addExcludeFromCodeCoverageAttribute, suffix,
                alternativeMethodName, additionalNamespaces);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Error creating the {ClassName} class", className);
            return false;
        }
    }

    private static bool TryCreateClass(string exportPath, string nameSpace, string className, string data,
        bool addExcludeFromCodeCoverageAttribute = false, string suffix = "Data", string? alternativeMethodName = null,
        IEnumerable<string>? additionalNamespaces = null)
    {
        data = Sanitize(data);
        if (alternativeMethodName == null)
        {
            if (className.EndsWith("s"))
                alternativeMethodName ??= className[..^1];
            else
                alternativeMethodName ??= $"{className}s";
        }

        var sb = new StringBuilder();
        if (data.Contains("CultureInfo"))
            sb.AppendLine("using System.Globalization;");
        if (additionalNamespaces != null)
        {
            foreach (var ns in additionalNamespaces)
                sb.AppendLine($"using {ns};");
        }

        sb.AppendLine("");
        sb.AppendLine($"namespace {nameSpace};");
        sb.AppendLine("");
        if (addExcludeFromCodeCoverageAttribute)
            sb.AppendLine("[System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]");
        var lines = data.Split("\n");
        sb.AppendLine($"public static partial class {className}Data");
        sb.AppendLine("{");
        sb.AppendLine($"    public static List<{className}> Load{alternativeMethodName}() => new List<{className}>");
        sb.AppendLine("    {");
        for (var i = 2; i < lines.Length - 1; i++)
        {
            var line = lines[i];
            sb.AppendLine($"        {line}");
        }

        sb.AppendLine("    };");
        sb.AppendLine("}");
        var classText = sb.ToString();
        var fileName = Path.Combine(exportPath, $"{className}{suffix}.cs");
        File.WriteAllText(fileName, classText);
        Logger.LogInformation("Created the {ClassName} class", className);
        return true;
    }

    private static string Sanitize(string text)
    {
        var sanitizedText = text;
        var hasBadData = text.Contains("= ,") || text.Contains(" = \n") || text.Contains(" = \r");
        if (!hasBadData) return ReplaceEmptyInitializers(sanitizedText);
        while (hasBadData)
        {
            var lines = sanitizedText.Split("\n");
            sanitizedText = string.Join("\n",
                lines.Where(line => !line.Contains("= ,") && !line.Contains(" = \n") && !line.Contains(" = \r"))
                    .ToList());
            hasBadData = sanitizedText.Contains("= ,") || sanitizedText.Contains(" = \n") ||
                         sanitizedText.Contains(" = \r");
        }

        return ReplaceEmptyInitializers(sanitizedText);
    }

    private static string ReplaceEmptyInitializers(string text)
    {
        const string pattern = @">\r\n            {\r\n            }";
        const string pattern2 = @">\r\n    {\r\n    }";
        if (Regex.IsMatch(text, pattern))
            text = Regex.Replace(text, pattern, ">()");
        if (Regex.IsMatch(text, pattern2))
            text = Regex.Replace(text, pattern2, ">()");
        return text;
    }
}