namespace Serilog.Sinks.BepInEx.Formatting.Display;

/// <summary>
/// Describes the available properties unique to BepInEx message template-based
/// output format strings.
/// </summary>
public static class OutputProperties
{
    /// <summary>
    /// The source name of the <see cref="BepInExLogSink"/> formatting the event.
    /// </summary>
    public const string SourceNamePropertyName = "SourceName";
}
