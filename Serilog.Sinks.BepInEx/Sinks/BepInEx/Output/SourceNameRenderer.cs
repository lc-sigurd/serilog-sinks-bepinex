using System.IO;
using Serilog.Events;
using Serilog.Parsing;
using Serilog.Sinks.BepInEx.Rendering;
using Serilog.Sinks.BepInEx.Themes;

namespace Serilog.Sinks.BepInEx.Output;

class SourceNameRenderer : OutputTemplateTokenRenderer
{
    readonly BepInExConsoleTheme _theme;
    readonly PropertyToken _sourceNameToken;

    public SourceNameRenderer(BepInExConsoleTheme theme, PropertyToken sourceNameToken)
    {
        _theme = theme;
        _sourceNameToken = sourceNameToken;
    }

    public override void Render(LogEvent logEvent, BepInExLogContext context, TextWriter output)
    {
        var _ = 0;
        using (_theme.Apply(context, output, BepInExConsoleThemeStyle.Text, ref _))
        {
            Padding.Apply(output, context.SourceName, _sourceNameToken.Alignment);
        }
    }
}
