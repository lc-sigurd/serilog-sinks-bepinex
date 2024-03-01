/*
 * This file is largely based upon
 * https://github.com/Lordfirespeed/serilog-sinks-msbuild/blob/4745c56974edeccaabda8e2982b6950f3076a326/Serilog.Sinks.MSBuild/Sinks/MSBuild/Output/TextTokenRenderer.cs
 * Copyright 2017 Serilog Contributors
 * Copyright (c) 2024 Joe Clack
 * Joe Clack licenses the referenced file to the Sigurd Team under the LGPL-3.0-OR-LATER license.
 *
 * Copyright (c) 2024 Sigurd Team
 * The Sigurd Team licenses this file to you under the LGPL-3.0-OR-LATER license.
 */

using System.IO;
using Serilog.Events;
using Serilog.Sinks.BepInEx.Themes;

namespace Serilog.Sinks.BepInEx.Output;

class TextTokenRenderer : OutputTemplateTokenRenderer
{
    readonly BepInExConsoleTheme _theme;
    readonly string _text;

    public TextTokenRenderer(BepInExConsoleTheme theme, string text)
    {
        _theme = theme;
        _text = text;
    }

    public override void Render(LogEvent logEvent, BepInExLogContext context, TextWriter output)
    {
        var _ = 0;
        using (_theme.Apply(context, output, BepInExConsoleThemeStyle.TertiaryText, ref _))
            output.Write(_text);
    }
}
