/*
 * This file is largely based upon
 * https://github.com/Lordfirespeed/serilog-sinks-msbuild/blob/4745c56974edeccaabda8e2982b6950f3076a326/Serilog.Sinks.MSBuild/Sinks/MSBuild/Themes/AnsiMSBuildConsoleTheme.cs
 * Copyright 2017 Serilog Contributors
 * Copyright (c) 2024 Joe Clack
 * Joe Clack licenses the referenced file to the Sigurd Team under the LGPL-3.0-OR-LATER license.
 *
 * Copyright (c) 2024 Sigurd Team
 * The Sigurd Team licenses this file to you under the LGPL-3.0-OR-LATER license.
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Serilog.Sinks.BepInEx.Extensions;

namespace Serilog.Sinks.BepInEx.Themes;

/// <summary>
/// A console theme that applies styles using ANSI terminal escape sequences.
/// </summary>
public class AnsiBepInExConsoleTheme : BepInExConsoleTheme
{
    /// <summary>
    /// A 256-color theme along the lines of Visual Studio Code.
    /// </summary>
    public static AnsiBepInExConsoleTheme Code { get; } = AnsiBepInExConsoleThemes.Code;

    /// <summary>
    /// A theme using only gray, black and white.
    /// </summary>
    public static AnsiBepInExConsoleTheme Grayscale { get; } = AnsiBepInExConsoleThemes.Grayscale;

    /// <summary>
    /// A theme in the style of the original <i>Serilog.Sinks.Literate</i>.
    /// </summary>
    public static AnsiBepInExConsoleTheme Literate { get; } = AnsiBepInExConsoleThemes.Literate;

    /// <summary>
    /// A theme in the style of the original <i>Serilog.Sinks.Literate</i> using only standard 16 terminal colors that will work on light backgrounds.
    /// </summary>
    public static AnsiBepInExConsoleTheme Sixteen { get; } = AnsiBepInExConsoleThemes.Sixteen;

    readonly IReadOnlyDictionary<BepInExConsoleThemeStyle, string> _styles;

    /// <summary>
    /// Construct a theme given a set of styles.
    /// </summary>
    /// <param name="styles">Styles to apply within the theme.</param>
    /// <exception cref="ArgumentNullException">When <paramref name="styles"/> is <code>null</code></exception>
    public AnsiBepInExConsoleTheme(IReadOnlyDictionary<BepInExConsoleThemeStyle, string> styles)
    {
        if (styles is null) throw new ArgumentNullException(nameof(styles));
        _styles = styles.ToDictionary(kv => kv.Key, kv => kv.Value);
    }

    /// <inheritdoc/>
    public override bool CanBuffer => true;

    /// <param name="context">The <see cref="BepInExLogContext"/> of the styled message.</param>
    /// <returns>The appropriate reset string for the <see cref="BepInExLogContext"/>.</returns>
    protected virtual string GetReset(BepInExLogContext context) => context.Level.GetLevelAnsiReset();

    /// <inheritdoc/>
    protected override int GetResetCharCount(BepInExLogContext context) => GetReset(context).Length;

    /// <inheritdoc/>
    public override int Set(BepInExLogContext context, TextWriter output, BepInExConsoleThemeStyle style)
    {
        if (_styles.TryGetValue(style, out var ansiStyle))
        {
            output.Write(ansiStyle);
            return ansiStyle.Length;
        }
        return 0;
    }

    /// <inheritdoc/>
    public override void Reset(BepInExLogContext context, TextWriter output) => output.Write(GetReset(context));
}
