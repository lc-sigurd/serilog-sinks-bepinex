/*
 * This file is largely based upon
 * https://github.com/Lordfirespeed/serilog-sinks-msbuild/blob/4745c56974edeccaabda8e2982b6950f3076a326/Serilog.Sinks.MSBuild/Sinks/MSBuild/Formatting/ThemedValueFormatter.cs
 * Copyright 2017 Serilog Contributors
 * Copyright (c) 2024 Joe Clack
 * Joe Clack licenses the referenced file to the Sigurd Team under the LGPL-3.0-OR-LATER license.
 *
 * Copyright (c) 2024 Sigurd Team
 * The Sigurd Team licenses this file to you under the LGPL-3.0-OR-LATER license.
 */

using System;
using System.IO;
using Serilog.Data;
using Serilog.Events;
using Serilog.Sinks.BepInEx.Themes;

namespace Serilog.Sinks.BepInEx.Formatting;

abstract class ThemedValueFormatter : LogEventPropertyValueVisitor<ThemedValueFormatterState, int>
{
    readonly BepInExConsoleTheme _theme;

    protected ThemedValueFormatter(BepInExConsoleTheme theme)
    {
        _theme = theme ?? throw new ArgumentNullException(nameof(theme));
    }

    protected BepInExStyleReset ApplyStyle(BepInExLogContext context, TextWriter output, BepInExConsoleThemeStyle style, ref int invisibleCharacterCount)
        => _theme.Apply(context, output, style, ref invisibleCharacterCount);

    public int Format(LogEventPropertyValue value, BepInExLogContext context, TextWriter output, string? format, bool literalTopLevel = false)
        => Visit(new ThemedValueFormatterState { LogContext = context, Output = output, Format = format, IsTopLevel = literalTopLevel, }, value);

    public abstract ThemedValueFormatter SwitchTheme(BepInExConsoleTheme theme);
}
