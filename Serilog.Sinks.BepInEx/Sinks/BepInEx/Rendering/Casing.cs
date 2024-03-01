/*
 * This file is largely based upon
 * https://github.com/Lordfirespeed/serilog-sinks-msbuild/blob/4745c56974edeccaabda8e2982b6950f3076a326/Serilog.Sinks.MSBuild/Sinks/MSBuild/Rendering/Casing.cs
 * Copyright 2017 Serilog Contributors
 * Copyright (c) 2024 Joe Clack
 * Joe Clack licenses the referenced file to the Sigurd Team under the LGPL-3.0-OR-LATER license.
 *
 * Copyright (c) 2024 Sigurd Team
 * The Sigurd Team licenses this file to you under the LGPL-3.0-OR-LATER license.
 */

namespace Serilog.Sinks.BepInEx.Rendering;

static class Casing
{
    /// <summary>
    /// Apply upper or lower casing to <paramref name="value"/> when <paramref name="format"/> is provided.
    /// Returns <paramref name="value"/> when no or invalid format provided.
    /// </summary>
    /// <param name="value">Provided string for formatting.</param>
    /// <param name="format">Format string.</param>
    /// <returns>The provided <paramref name="value"/> with formatting applied.</returns>
    public static string Format(string value, string? format = null)
    {
        switch (format)
        {
            case "u":
                return value.ToUpperInvariant();
            case "w":
                return value.ToLowerInvariant();
            default:
                return value;
        }
    }
}
