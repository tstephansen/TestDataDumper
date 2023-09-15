using System.Diagnostics.CodeAnalysis;

namespace TestDataDumper;

/// <summary>
/// The options used by the <seealso cref="TestDataDumper"/> class.
/// </summary>
public class TestDataDumperOptions
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TestDataDumperOptions"/> class.
    /// </summary>
    /// <param name="exportPath">The path of the folder where the exported data will be written to.</param>
    /// <param name="nameSpace">The namespace of the exported data.</param>
    /// <param name="suffix">(Optional) The suffix added to the "Load" method. Defaults to Data.</param>
    /// <param name="addExcludeFromCodeCoverageAttribute">
    ///     (Optional) If <c>true</c> the <see cref="ExcludeFromCodeCoverageAttribute"/> will be added to
    ///     the top of the class. Defaults to false.
    /// </param>
    /// <param name="additionalNamespaces">
    ///     (Optional) A list of additional namespaces that will be added to the class.
    /// </param>
    public TestDataDumperOptions(string exportPath, string nameSpace, string suffix = "Data",
        bool addExcludeFromCodeCoverageAttribute = false, IEnumerable<string>? additionalNamespaces = null)
    {
        ExportPath = exportPath;
        Namespace = nameSpace;
        Suffix = suffix;
        AddExcludeFromCodeCoverageAttribute = addExcludeFromCodeCoverageAttribute;
        AdditionalNamespaces = additionalNamespaces?.ToList() ?? new List<string>();
    }
    
    /// <summary>
    ///     Gets or sets path where the exported data will be written to.
    /// </summary>
    /// <value>The export path.</value>
    public string ExportPath { get; set; }
    
    /// <summary>
    ///     Gets or sets the namespace.
    /// </summary>
    /// <value>The namespace.</value>
    public string Namespace { get; set; }
    
    /// <summary>
    ///     Determines whether or not to add the ExcludeFromCodeCoverage attribute.
    /// </summary>
    /// <value>Returns <c>true</c> if the attribute should be added, <c>false</c> otherwise.</value>
    public bool AddExcludeFromCodeCoverageAttribute { get; set; }
    
    /// <summary>
    ///     Gets or sets the suffix.
    /// </summary>
    /// <value>The suffix.</value>
    public string Suffix { get; set; }
    
    /// <summary>
    ///     Gets or sets the list of additional namespaces.
    /// </summary>
    /// <value>The list of additional namespaces.</value>
    public List<string>? AdditionalNamespaces { get; set; }
}