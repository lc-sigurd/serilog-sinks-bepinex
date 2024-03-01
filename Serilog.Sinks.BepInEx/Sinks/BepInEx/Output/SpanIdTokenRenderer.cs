/*
 * This file is largely based upon
 * https://github.com/Lordfirespeed/serilog-sinks-msbuild/blob/4745c56974edeccaabda8e2982b6950f3076a326/Serilog.Sinks.MSBuild/Sinks/MSBuild/Output/SpanIdTokenRenderer.cs
 * Copyright 2017 Serilog Contributors
 * Copyright (c) 2024 Joe Clack
 * Joe Clack licenses the referenced file to the Sigurd Team under the LGPL-3.0-OR-LATER license.
 *
 * Copyright (c) 2024 Sigurd Team
 * The Sigurd Team licenses this file to you under the LGPL-3.0-OR-LATER license.
 */

using System.IO;
using Serilog.Events;
using Serilog.Parsing;
using Serilog.Sinks.BepInEx.Rendering;
using Serilog.Sinks.BepInEx.Themes;

namespace Serilog.Sinks.BepInEx.Output;

class SpanIdTokenRenderer : OutputTemplateTokenRenderer
{
    readonly BepInExConsoleTheme _theme;
    readonly Alignment? _alignment;

    public SpanIdTokenRenderer(BepInExConsoleTheme theme, PropertyToken spanIdToken)
    {
        _theme = theme;
        _alignment = spanIdToken.Alignment;
    }

    public override void Render(LogEvent logEvent, BepInExLogContext context, TextWriter output)
    {
        if (logEvent.SpanId is not { } spanId)
            return;

        var _ = 0;
        using (_theme.Apply(context, output, BepInExConsoleThemeStyle.Text, ref _))
        {
            if (_alignment is {} alignment)
                Padding.Apply(output, spanId.ToString(), alignment);
            else
                output.Write(spanId);
        }
    }
}
