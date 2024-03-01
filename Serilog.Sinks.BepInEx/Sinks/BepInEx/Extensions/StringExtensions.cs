/*
 * This file is largely based upon
 * https://github.com/Lordfirespeed/serilog-sinks-msbuild/blob/4745c56974edeccaabda8e2982b6950f3076a326/Serilog.Sinks.MSBuild/Sinks/MSBuild/Extensions/StringExtensions.cs
 * Copyright (c) 2024 Joe Clack
 * Joe Clack licenses the referenced file to the Sigurd Team under the LGPL-3.0-OR-LATER license.
 *
 * Copyright (c) 2024 Sigurd Team
 * The Sigurd Team licenses this file to you under the LGPL-3.0-OR-LATER license.
 */

using System;
using System.Linq;

namespace Serilog.Sinks.BepInEx.Extensions;

/// <summary>
/// Contains extension methods that add functionality to <see cref="string"/>.
/// </summary>
public static class StringExtensions
{
    /// <summary>
    /// Indent a <see cref="string"/> by prepending space characters to it.
    /// </summary>
    /// <param name="input">The <see cref="string"/>to indent.</param>
    /// <param name="spaces">The number of spaces to prepend to <paramref name="input"/>.</param>
    /// <returns>The indented <see cref="string"/>.</returns>
    /// <seealso cref="MultilineIndent"/>
    public static string Indent(this string input, int spaces) => $"{String.Concat(Enumerable.Repeat(" ", spaces))}{input}";

    /// <summary>
    /// Indent a multiline <see cref="string"/> by prepending space characters to every line of the string.
    /// </summary>
    /// <param name="input">The multiline <see cref="string"/>to indent.</param>
    /// <param name="spaces">The number of spaces to prepend to each line of <paramref name="input"/>.</param>
    /// <returns>The indented <see cref="string"/>.</returns>
    public static string MultilineIndent(this string input, int spaces)
        => String.Join(
            Environment.NewLine,
            input.Split("\n")
                .Select(line => line.Trim())
                .Select(line => line.Indent(spaces))
            );
}
