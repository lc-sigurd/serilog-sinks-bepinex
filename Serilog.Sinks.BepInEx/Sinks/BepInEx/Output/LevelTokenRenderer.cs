/*
 * This file is largely based upon
 * https://github.com/Lordfirespeed/serilog-sinks-msbuild/blob/4745c56974edeccaabda8e2982b6950f3076a326/Serilog.Sinks.MSBuild/Sinks/MSBuild/Output/LevelTokenRenderer.cs
 * Copyright 2017 Serilog Contributors
 * Copyright (c) 2024 Joe Clack
 * Joe Clack licenses the referenced file to the Sigurd Team under the LGPL-3.0-OR-LATER license.
 *
 * Copyright (c) 2024 Sigurd Team
 * The Sigurd Team licenses this file to you under the LGPL-3.0-OR-LATER license.
 */

using System.Collections.Generic;
using System.IO;
using Serilog.Events;
using Serilog.Parsing;
using Serilog.Sinks.BepInEx.Rendering;
using Serilog.Sinks.BepInEx.Themes;

namespace Serilog.Sinks.BepInEx.Output;

class LevelTokenRenderer : OutputTemplateTokenRenderer
{
    readonly BepInExConsoleTheme _theme;
    readonly PropertyToken _levelToken;

    static readonly Dictionary<LogEventLevel, BepInExConsoleThemeStyle> Levels = new Dictionary<LogEventLevel, BepInExConsoleThemeStyle>
    {
        { LogEventLevel.Verbose, BepInExConsoleThemeStyle.LevelVerbose },
        { LogEventLevel.Debug, BepInExConsoleThemeStyle.LevelDebug },
        { LogEventLevel.Information, BepInExConsoleThemeStyle.LevelInformation },
        { LogEventLevel.Warning, BepInExConsoleThemeStyle.LevelWarning },
        { LogEventLevel.Error, BepInExConsoleThemeStyle.LevelError },
        { LogEventLevel.Fatal, BepInExConsoleThemeStyle.LevelFatal },
    };

    public LevelTokenRenderer(BepInExConsoleTheme theme, PropertyToken levelToken)
    {
        _theme = theme;
        _levelToken = levelToken;
    }

    public override void Render(LogEvent logEvent, BepInExLogContext context, TextWriter output)
    {
        var moniker = LevelOutputFormat.GetLevelMoniker(logEvent.Level, _levelToken.Format);
        if (!Levels.TryGetValue(logEvent.Level, out var levelStyle))
            levelStyle = BepInExConsoleThemeStyle.Invalid;

        var _ = 0;
        using (_theme.Apply(context, output, levelStyle, ref _))
            Padding.Apply(output, moniker, _levelToken.Alignment);
    }
}
