using System.Text;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;

namespace TestDataDumper;

/// <summary>
/// The test data dumper.
/// </summary>
public class TestDataDumper
{
    private static readonly ILogger Logger = TestDataDumperLoggerFactory.CreateLogger<TestDataDumper>();

    /// <summary>
    ///     Creates a file with that will expose a Load{ClassName} method that returns a list of {ClassName} objects.
    /// </summary>
    /// <param name="className">The name of the class.</param>
    /// <param name="data">The exported data.</param>
    /// <param name="options">The export options.</param>
    /// <param name="alternativeMethodName">
    ///     (Optional) The default load method is Load{ClassName}. If an alternative method name is specified
    ///     the load method will be Load{AlternativeMethodName}.
    /// </param>
    /// <returns>Returns <c>true</c> if the data is created successfully.</returns>
    public static bool CreateClass(string className, string data, TestDataDumperOptions options,
        string? alternativeMethodName = null)
    {
        Logger.LogInformation("Creating the {ClassName} class", className);
        try
        {
            return TryCreateClass(className, data, options, alternativeMethodName);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Error creating the {ClassName} class", className);
            return false;
        }
    }

    private static bool TryCreateClass(string className, string data, TestDataDumperOptions options,
        string? alternativeMethodName = null)
    {
        var sb = new StringBuilder();
        // Sanitize the data.
        data = Sanitize(data);
        // Set the method name if the alternative method name is null.
        if (alternativeMethodName == null)
        {
            if (className.EndsWith("s"))
                alternativeMethodName ??= className[..^1];
            else
                alternativeMethodName ??= $"{className}s";
        }
        // Use System.Globalization namespace if the entity contains a DateTime property.
        if (data.Contains("CultureInfo"))
            sb.AppendLine("using System.Globalization;");
        // Add additional namespaces to the file.
        if (options.AdditionalNamespaces != null)
        {
            foreach (var ns in options.AdditionalNamespaces)
                sb.AppendLine($"using {ns};");
        }
        sb.AppendLine("");
        sb.AppendLine($"namespace {options.Namespace};");
        sb.AppendLine("");
        // Add the ExcludeFromCodeCoverage attribute.
        if (options.AddExcludeFromCodeCoverageAttribute)
            sb.AppendLine("[System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]");
        // Determine how to split the data (on Windows it's typically \r\n but on Mac it's typically \n).
        var splitChar = data.Contains("\r\n") ? "\r\n" : "\n";
        var lines = data.Split(splitChar);
        // Create the class and load method.
        sb.AppendLine($"public static partial class {className}Data");
        sb.AppendLine("{");
        sb.AppendLine($"    public static List<{className}> Load{alternativeMethodName}() => new List<{className}>");
        sb.AppendLine("    {");
        // Append the data to the file.
        for (var i = 2; i < lines.Length - 1; i++)
        {
            var line = lines[i];
            sb.AppendLine($"        {line}");
        }
        sb.AppendLine("    };");
        sb.AppendLine("}");
        // Get the file's text from the StringBuilder.
        var classText = sb.ToString();
        var fileName = Path.Combine(options.ExportPath, $"{className}{options.Suffix}.cs");
        if (File.Exists(fileName))
            File.Delete(fileName);
        // Write the text to the file.
        File.WriteAllText(fileName, classText);
        Logger.LogInformation("Created the {ClassName} class", className);
        return true;
    }

    /// <summary>
    /// This method removes empty initializers and other characters that will cause problems from the data.
    /// </summary>
    /// <param name="data">The data.</param>
    /// <returns>The sanitized data.</returns>
    private static string Sanitize(string data)
    {
        var sanitizedText = data;
        var hasBadData = data.Contains("= ,") || data.Contains(" = \n") || data.Contains(" = \r");
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