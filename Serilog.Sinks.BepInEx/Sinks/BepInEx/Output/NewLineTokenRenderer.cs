/*
 * This file is largely based upon
 * https://github.com/Lordfirespeed/serilog-sinks-msbuild/blob/4745c56974edeccaabda8e2982b6950f3076a326/Serilog.Sinks.MSBuild/Sinks/MSBuild/Output/NewLineTokenRenderer.cs
 * Copyright 2017 Serilog Contributors
 * Copyright (c) 2024 Joe Clack
 * Joe Clack licenses the referenced file to the Sigurd Team under the LGPL-3.0-OR-LATER license.
 *
 * Copyright (c) 2024 Sigurd Team
 * The Sigurd Team licenses this file to you under the LGPL-3.0-OR-LATER license.
 */

using System;
using System.IO;
using Serilog.Events;
using Serilog.Parsing;
using Serilog.Sinks.BepInEx.Rendering;

namespace Serilog.Sinks.BepInEx.Output;

class NewLineTokenRenderer : OutputTemplateTokenRenderer
{
    readonly Alignment? _alignment;

    public NewLineTokenRenderer(Alignment? alignment)
    {
        _alignment = alignment;
    }

    public override void Render(LogEvent logEvent, BepInExLogContext context, TextWriter output)
    {
        if (_alignment.HasValue)
            Padding.Apply(output, Environment.NewLine, _alignment.Value.Widen(Environment.NewLine.Length));
        else
            output.WriteLine();
    }
}
