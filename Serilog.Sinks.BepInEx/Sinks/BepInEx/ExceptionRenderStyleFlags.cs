/*
 * This file is largely based upon
 * https://github.com/Lordfirespeed/serilog-sinks-msbuild/blob/4745c56974edeccaabda8e2982b6950f3076a326/Serilog.Sinks.MSBuild/Sinks/MSBuild/ExceptionRenderStyleFlags.cs
 * Copyright (c) 2024 Joe Clack
 * Joe Clack licenses the referenced file to the Sigurd Team under the LGPL-3.0-OR-LATER license.
 *
 * Copyright (c) 2024 Sigurd Team
 * The Sigurd Team licenses this file to you under the LGPL-3.0-OR-LATER license.
 */

using System;

namespace Serilog.Sinks.BepInEx;

/// <summary>
/// Contains extension methods that add functionality to <see cref="ExceptionRenderStyleFlags"/>.
/// </summary>
public static class ExceptionRenderStyleFlagsExtensions
{
    /// <summary>
    /// Determine whether some <see cref="ExceptionRenderStyleFlags"/> contain all flags specified by
    /// another set of <see cref="ExceptionRenderStyleFlags"/>.
    /// </summary>
    /// <param name="this">Some <see cref="ExceptionRenderStyleFlags"/> to test.</param>
    /// <param name="flags">The <see cref="ExceptionRenderStyleFlags"/> to test for.</param>
    /// <returns><see langword="true"/> if <paramref name="this"/> specifies all flags specified by <paramref name="flags"/>; Otherwise, <see langword="false"/>.</returns>
    public static bool Has(this ExceptionRenderStyleFlags @this, ExceptionRenderStyleFlags flags) => (@this & flags) == flags;
}

/// <summary>
/// Specifies flags that control the contents of a <see cref="string"/> render of an <see cref="Exception"/> and
/// the format the render takes.
/// </summary>
[Flags]
public enum ExceptionRenderStyleFlags
{
    /// <summary>
    /// Specifies that no <see cref="ExceptionRenderStyleFlags"/> are defined.
    /// </summary>
    Default = 0,

    /// <summary>
    /// Specifies that renders should include inner exceptions.
    /// </summary>
    IncludeInner = 1 << 0,

    /// <summary>
    /// Specifies that renders should include their exception's call stack.
    /// </summary>
    IncludeStackTrace = 1 << 1,

    /// <summary>
    /// Specifies that renders should include their exception's type.
    /// </summary>
    IncludeType = 1 << 2,
}
