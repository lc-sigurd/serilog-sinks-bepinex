/*
 * This file is largely based upon
 * https://github.com/Lordfirespeed/serilog-sinks-msbuild/blob/4745c56974edeccaabda8e2982b6950f3076a326/Serilog.Sinks.MSBuild/Sinks/MSBuild/Themes/EmptyMSBuildConsoleTheme.cs
 * Copyright 2017 Serilog Contributors
 * Copyright (c) 2024 Joe Clack
 * Joe Clack licenses the referenced file to the Sigurd Team under the LGPL-3.0-OR-LATER license.
 *
 * Copyright (c) 2024 Sigurd Team
 * The Sigurd Team licenses this file to you under the LGPL-3.0-OR-LATER license.
 */

using System.IO;

namespace Serilog.Sinks.BepInEx.Themes;

class EmptyBepInExConsoleTheme : BepInExConsoleTheme
{
    public override bool CanBuffer => true;

    protected override int GetResetCharCount(BepInExLogContext context) => 0;

    public override int Set(BepInExLogContext context, TextWriter output, BepInExConsoleThemeStyle style) => 0;

    public override void Reset(BepInExLogContext context, TextWriter output) { }
}
