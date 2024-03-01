/*
 * This file is largely based upon
 * https://github.com/Lordfirespeed/serilog-sinks-msbuild/blob/4745c56974edeccaabda8e2982b6950f3076a326/Serilog.Sinks.MSBuild/Sinks/MSBuild/Themes/MSBuildStyleReset.cs
 * Copyright 2017 Serilog Contributors
 * Copyright (c) 2024 Joe Clack
 * Joe Clack licenses the referenced file to the Sigurd Team under the LGPL-3.0-OR-LATER license.
 *
 * Copyright (c) 2024 Sigurd Team
 * The Sigurd Team licenses this file to you under the LGPL-3.0-OR-LATER license.
 */

using System;
using System.IO;

namespace Serilog.Sinks.BepInEx.Themes;

struct BepInExStyleReset : IDisposable
{
    readonly BepInExConsoleTheme _theme;
    readonly TextWriter _output;
    readonly BepInExLogContext _logContext;

    public BepInExStyleReset(BepInExConsoleTheme theme, BepInExLogContext context, TextWriter output)
    {
        _theme = theme;
        _output = output;
        _logContext = context;
    }

    public void Dispose() => _theme.Reset(_logContext, _output);
}
