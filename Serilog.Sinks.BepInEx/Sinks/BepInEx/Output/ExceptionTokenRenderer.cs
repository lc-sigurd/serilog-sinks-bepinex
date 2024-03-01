/*
 * This file is largely based upon
 * https://github.com/Lordfirespeed/serilog-sinks-msbuild/blob/4745c56974edeccaabda8e2982b6950f3076a326/Serilog.Sinks.MSBuild/Sinks/MSBuild/Output/ExceptionTokenRenderer.cs
 * Copyright 2017 Serilog Contributors
 * Copyright (c) 2024 Joe Clack
 * Joe Clack licenses the referenced file to the Sigurd Team under the LGPL-3.0-OR-LATER license.
 *
 * Copyright (c) 2024 Sigurd Team
 * The Sigurd Team licenses this file to you under the LGPL-3.0-OR-LATER license.
 */

using System.IO;
using System.Linq;
using Serilog.Events;
using Serilog.Parsing;
using Serilog.Sinks.BepInEx.Extensions;
using Serilog.Sinks.BepInEx.Themes;

namespace Serilog.Sinks.BepInEx.Output;

class ExceptionTokenRenderer : OutputTemplateTokenRenderer
{
    private readonly BepInExConsoleTheme _theme;

    private const ExceptionRenderStyleFlags ExceptionRenderStyle = 0
        | ExceptionRenderStyleFlags.IncludeInner
        | ExceptionRenderStyleFlags.IncludeStackTrace
        | ExceptionRenderStyleFlags.IncludeType;

    public ExceptionTokenRenderer(BepInExConsoleTheme theme, PropertyToken pt)
    {
        _theme = theme;
    }

    public override void Render(LogEvent logEvent, BepInExLogContext context, TextWriter output)
    {
        // Padding is never applied by this renderer.

        if (logEvent.Exception is null)
            return;

        var lines = new StringReader(logEvent.RenderException(ExceptionRenderStyle));
        while (lines.ReadLine() is { } nextLine)
        {
            var style = LineIsStackTrace(nextLine) ? BepInExConsoleThemeStyle.TertiaryText : BepInExConsoleThemeStyle.ExceptionText;
            var _ = 0;
            using (_theme.Apply(context, output, style, ref _))
                output.Write(nextLine);
            output.WriteLine();
        }

        // https://stackoverflow.com/a/20411839/11045433
        int LeadingSpaceCount(string line) => line
            .TakeWhile(c => c is ' ')
            .Count();

        bool LineIsStackTrace(string line) => LeadingSpaceCount(line) % 4 == 3;
    }
}
