/*
 * This file is largely based upon
 * https://github.com/Lordfirespeed/serilog-sinks-msbuild/blob/4745c56974edeccaabda8e2982b6950f3076a326/Serilog.Sinks.MSBuild/Sinks/MSBuild/Themes/MSBuildConsoleTheme.cs
 * Copyright 2017 Serilog Contributors
 * Copyright (c) 2024 Joe Clack
 * Joe Clack licenses the referenced file to the Sigurd Team under the LGPL-3.0-OR-LATER license.
 *
 * Copyright (c) 2024 Sigurd Team
 * The Sigurd Team licenses this file to you under the LGPL-3.0-OR-LATER license.
 */

using System.IO;

namespace Serilog.Sinks.BepInEx.Themes;

/// <summary>
/// The base class for styled terminal output.
/// </summary>
public abstract class BepInExConsoleTheme
{
    /// <summary>
    /// No styling applied.
    /// </summary>
    public static BepInExConsoleTheme None { get; } = new EmptyBepInExConsoleTheme();

    /// <summary>
    /// True if styling applied by the theme is written into the output, and can thus be
    /// buffered and measured.
    /// </summary>
    public abstract bool CanBuffer { get; }

    /// <summary>
    /// Begin a span of text in the specified <paramref name="style"/>.
    /// </summary>
    /// /// <param name="context">Output <see cref="BepInExLogContext"/>.</param>
    /// <param name="output">Output destination.</param>
    /// <param name="style">Style to apply.</param>
    /// <returns>The number of characters written to <paramref name="output"/>.</returns>
    public abstract int Set(BepInExLogContext context, TextWriter output, BepInExConsoleThemeStyle style);

    /// <summary>
    /// Reset the output to default BepInEx colours for the context.
    /// </summary>
    /// <param name="context">Output <see cref="BepInExLogContext"/>.</param>
    /// <param name="output">Output destination.</param>
    public abstract void Reset(BepInExLogContext context, TextWriter output);

    /// <param name="context">Output <see cref="BepInExLogContext"/>.</param>
    /// <returns>The number of characters written by the <see cref="Reset(BepInExLogContext,TextWriter)"/> method.</returns>
    protected abstract int GetResetCharCount(BepInExLogContext context);

    internal BepInExStyleReset Apply(BepInExLogContext context, TextWriter output, BepInExConsoleThemeStyle style, ref int invisibleCharacterCount)
    {
        invisibleCharacterCount += Set(context, output, style);
        invisibleCharacterCount += GetResetCharCount(context);

        return new BepInExStyleReset(this, context, output);
    }
}
