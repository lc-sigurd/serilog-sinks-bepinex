/*
 * This file is largely based upon
 * https://github.com/Lordfirespeed/serilog-sinks-msbuild/blob/4745c56974edeccaabda8e2982b6950f3076a326/Serilog.Sinks.MSBuild/Sinks/MSBuild/Themes/MSBuildConsoleThemeStyle.cs
 * Copyright 2017 Serilog Contributors
 * Copyright (c) 2024 Joe Clack
 * Joe Clack licenses the referenced file to the Sigurd Team under the LGPL-3.0-OR-LATER license.
 *
 * Copyright (c) 2024 Sigurd Team
 * The Sigurd Team licenses this file to you under the LGPL-3.0-OR-LATER license.
 */

namespace Serilog.Sinks.BepInEx.Themes;

/// <summary>
/// Elements styled by a console theme.
/// </summary>
public enum BepInExConsoleThemeStyle
{
    /// <summary>
    /// Prominent text, generally content within an event's message.
    /// </summary>
    Text,

    /// <summary>
    /// Prominent text which follows BepInEx's log-level theming convention.
    /// </summary>
    BepInExText,

    /// <summary>
    /// Boilerplate text, for example items specified in an output template.
    /// </summary>
    SecondaryText,

    /// <summary>
    /// De-emphasized text, for example literal text in output templates and
    /// punctuation used when writing structured data.
    /// </summary>
    TertiaryText,

    /// <summary>
    /// Prominent 'success' text.
    /// </summary>
    SuccessText,

    /// <summary>
    /// Prominent 'danger' text.
    /// </summary>
    DangerText,

    /// <summary>
    /// Prominent 'warning' text.
    /// </summary>
    WarningText,

    /// <summary>
    /// Prominent 'exception' text.
    /// </summary>
    ExceptionText,

    /// <summary>
    /// Output demonstrating some kind of configuration issue, e.g. an invalid
    /// message template token.
    /// </summary>
    Invalid,

    /// <summary>
    /// The built-in <see langword="null"/> value.
    /// </summary>
    Null,

    /// <summary>
    /// Property and type names.
    /// </summary>
    Name,

    /// <summary>
    /// Strings.
    /// </summary>
    String,

    /// <summary>
    /// Numbers.
    /// </summary>
    Number,

    /// <summary>
    /// <see cref="bool"/> values.
    /// </summary>
    Boolean,

    /// <summary>
    /// All other scalar values, e.g. <see cref="System.Guid"/> instances.
    /// </summary>
    Scalar,

    /// <summary>
    /// Level indicator.
    /// </summary>
    LevelVerbose,

    /// <summary>
    /// Level indicator.
    /// </summary>
    LevelDebug,

    /// <summary>
    /// Level indicator.
    /// </summary>
    LevelInformation,

    /// <summary>
    /// Level indicator.
    /// </summary>
    LevelWarning,

    /// <summary>
    /// Level indicator.
    /// </summary>
    LevelError,

    /// <summary>
    /// Level indicator.
    /// </summary>
    LevelFatal,
}
