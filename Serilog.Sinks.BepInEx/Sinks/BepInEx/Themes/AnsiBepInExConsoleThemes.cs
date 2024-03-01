/*
 * This file is largely based upon
 * https://github.com/Lordfirespeed/serilog-sinks-msbuild/blob/4745c56974edeccaabda8e2982b6950f3076a326/Serilog.Sinks.MSBuild/Sinks/MSBuild/Themes/AnsiMSBuildConsoleThemes.cs
 * Copyright 2017 Serilog Contributors
 * Copyright (c) 2024 Joe Clack
 * Joe Clack licenses the referenced file to the Sigurd Team under the LGPL-3.0-OR-LATER license.
 *
 * Copyright (c) 2024 Sigurd Team
 * The Sigurd Team licenses this file to you under the LGPL-3.0-OR-LATER license.
 */

using System.Collections.Generic;

namespace Serilog.Sinks.BepInEx.Themes;

static class AnsiBepInExConsoleThemes
{
    public static AnsiBepInExConsoleTheme Literate { get; } = new AnsiBepInExConsoleTheme(
        new Dictionary<BepInExConsoleThemeStyle, string>
        {
            [BepInExConsoleThemeStyle.Text] = "\x1b[38;5;0015m",
            [BepInExConsoleThemeStyle.SecondaryText] = "\x1b[38;5;0007m",
            [BepInExConsoleThemeStyle.TertiaryText] = "\x1b[38;5;0008m",
            [BepInExConsoleThemeStyle.SuccessText] = "\x1b[38;5;0113m",
            [BepInExConsoleThemeStyle.DangerText] = "\x1b[38;5;0208m",
            [BepInExConsoleThemeStyle.WarningText] = "\x1b[38;5;0011m",
            [BepInExConsoleThemeStyle.ExceptionText] = "\x1b[38;5;0009m",
            [BepInExConsoleThemeStyle.Invalid] = "\x1b[38;5;0011m",
            [BepInExConsoleThemeStyle.Null] = "\x1b[38;5;0027m",
            [BepInExConsoleThemeStyle.Name] = "\x1b[38;5;0007m",
            [BepInExConsoleThemeStyle.String] = "\x1b[38;5;0045m",
            [BepInExConsoleThemeStyle.Number] = "\x1b[38;5;0200m",
            [BepInExConsoleThemeStyle.Boolean] = "\x1b[38;5;0027m",
            [BepInExConsoleThemeStyle.Scalar] = "\x1b[38;5;0085m",
            [BepInExConsoleThemeStyle.LevelVerbose] = "\x1b[38;5;0007m",
            [BepInExConsoleThemeStyle.LevelDebug] = "\x1b[38;5;0007m",
            [BepInExConsoleThemeStyle.LevelInformation] = "\x1b[38;5;0015m",
            [BepInExConsoleThemeStyle.LevelWarning] = "\x1b[38;5;0011m",
            [BepInExConsoleThemeStyle.LevelError] = "\x1b[38;5;0015m\x1b[48;5;0196m",
            [BepInExConsoleThemeStyle.LevelFatal] = "\x1b[38;5;0015m\x1b[48;5;0196m",
        });

    public static AnsiBepInExConsoleTheme Grayscale { get; } = new AnsiBepInExConsoleTheme(
        new Dictionary<BepInExConsoleThemeStyle, string>
        {
            [BepInExConsoleThemeStyle.Text] = "\x1b[37;1m",
            [BepInExConsoleThemeStyle.SecondaryText] = "\x1b[37m",
            [BepInExConsoleThemeStyle.TertiaryText] = "\x1b[30;1m",
            [BepInExConsoleThemeStyle.Invalid] = "\x1b[37;1m\x1b[47m",
            [BepInExConsoleThemeStyle.Null] = "\x1b[1m\x1b[37;1m",
            [BepInExConsoleThemeStyle.Name] = "\x1b[37m",
            [BepInExConsoleThemeStyle.String] = "\x1b[1m\x1b[37;1m",
            [BepInExConsoleThemeStyle.Number] = "\x1b[1m\x1b[37;1m",
            [BepInExConsoleThemeStyle.Boolean] = "\x1b[1m\x1b[37;1m",
            [BepInExConsoleThemeStyle.Scalar] = "\x1b[1m\x1b[37;1m",
            [BepInExConsoleThemeStyle.LevelVerbose] = "\x1b[30;1m",
            [BepInExConsoleThemeStyle.LevelDebug] = "\x1b[30;1m",
            [BepInExConsoleThemeStyle.LevelInformation] = "\x1b[37;1m",
            [BepInExConsoleThemeStyle.LevelWarning] = "\x1b[37;1m\x1b[47m",
            [BepInExConsoleThemeStyle.LevelError] = "\x1b[30m\x1b[47m",
            [BepInExConsoleThemeStyle.LevelFatal] = "\x1b[30m\x1b[47m",
        });

    public static AnsiBepInExConsoleTheme Code { get; } = new AnsiBepInExConsoleTheme(
        new Dictionary<BepInExConsoleThemeStyle, string>
        {
            [BepInExConsoleThemeStyle.Text] = "\x1b[38;5;0253m",
            [BepInExConsoleThemeStyle.SecondaryText] = "\x1b[38;5;0246m",
            [BepInExConsoleThemeStyle.TertiaryText] = "\x1b[38;5;0242m",
            [BepInExConsoleThemeStyle.SuccessText] = "\x1b[38;5;0113m",
            [BepInExConsoleThemeStyle.DangerText] = "\x1b[38;5;0208m",
            [BepInExConsoleThemeStyle.WarningText] = "\x1b[38;5;0011m",
            [BepInExConsoleThemeStyle.ExceptionText] = "\x1b[38;5;0009m",
            [BepInExConsoleThemeStyle.Invalid] = "\x1b[33;1m",
            [BepInExConsoleThemeStyle.Null] = "\x1b[38;5;0038m",
            [BepInExConsoleThemeStyle.Name] = "\x1b[38;5;0081m",
            [BepInExConsoleThemeStyle.String] = "\x1b[38;5;0216m",
            [BepInExConsoleThemeStyle.Number] = "\x1b[38;5;151m",
            [BepInExConsoleThemeStyle.Boolean] = "\x1b[38;5;0038m",
            [BepInExConsoleThemeStyle.Scalar] = "\x1b[38;5;0079m",
            [BepInExConsoleThemeStyle.LevelVerbose] = "\x1b[37m",
            [BepInExConsoleThemeStyle.LevelDebug] = "\x1b[37m",
            [BepInExConsoleThemeStyle.LevelInformation] = "\x1b[37;1m",
            [BepInExConsoleThemeStyle.LevelWarning] = "\x1b[38;5;0229m",
            [BepInExConsoleThemeStyle.LevelError] = "\x1b[38;5;0197m\x1b[48;5;0238m",
            [BepInExConsoleThemeStyle.LevelFatal] = "\x1b[38;5;0197m\x1b[48;5;0238m",
        });

    public static AnsiBepInExConsoleTheme Sixteen { get; } = new AnsiBepInExConsoleTheme(
        new Dictionary<BepInExConsoleThemeStyle, string>
        {
            [BepInExConsoleThemeStyle.Text] = AnsiEscapeSequence.Unthemed,
            [BepInExConsoleThemeStyle.SecondaryText] = AnsiEscapeSequence.Unthemed,
            [BepInExConsoleThemeStyle.TertiaryText] = AnsiEscapeSequence.Unthemed,
            [BepInExConsoleThemeStyle.SuccessText] = AnsiEscapeSequence.BrightGreen,
            [BepInExConsoleThemeStyle.DangerText] = AnsiEscapeSequence.Red,
            [BepInExConsoleThemeStyle.WarningText] = AnsiEscapeSequence.BrightYellow,
            [BepInExConsoleThemeStyle.ExceptionText] = AnsiEscapeSequence.BrightRed,
            [BepInExConsoleThemeStyle.Invalid] = AnsiEscapeSequence.Yellow,
            [BepInExConsoleThemeStyle.Null] = AnsiEscapeSequence.Blue,
            [BepInExConsoleThemeStyle.Name] = AnsiEscapeSequence.Unthemed,
            [BepInExConsoleThemeStyle.String] = AnsiEscapeSequence.Cyan,
            [BepInExConsoleThemeStyle.Number] = AnsiEscapeSequence.Magenta,
            [BepInExConsoleThemeStyle.Boolean] = AnsiEscapeSequence.Blue,
            [BepInExConsoleThemeStyle.Scalar] = AnsiEscapeSequence.Green,
            [BepInExConsoleThemeStyle.LevelVerbose] = AnsiEscapeSequence.Unthemed,
            [BepInExConsoleThemeStyle.LevelDebug] = AnsiEscapeSequence.Bold,
            [BepInExConsoleThemeStyle.LevelInformation] = AnsiEscapeSequence.BrightCyan,
            [BepInExConsoleThemeStyle.LevelWarning] = AnsiEscapeSequence.BrightYellow,
            [BepInExConsoleThemeStyle.LevelError] = AnsiEscapeSequence.BrightRed,
            [BepInExConsoleThemeStyle.LevelFatal] = AnsiEscapeSequence.BrightRed,
        });
}
