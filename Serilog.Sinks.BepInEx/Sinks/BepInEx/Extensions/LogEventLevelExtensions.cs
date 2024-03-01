/*
 * This file is largely based upon
 * https://github.com/Lordfirespeed/serilog-sinks-msbuild/blob/4745c56974edeccaabda8e2982b6950f3076a326/Serilog.Sinks.MSBuild/Sinks/MSBuild/Extensions/LogEventLevelExtensions.cs
 * Copyright (c) 2024 Joe Clack
 * Joe Clack licenses the referenced file to the Sigurd Team under the LGPL-3.0-OR-LATER license.
 *
 * Copyright (c) 2024 Sigurd Team
 * The Sigurd Team licenses this file to you under the LGPL-3.0-OR-LATER license.
 */

using System;
using BepInEx.Logging;
using Serilog.Events;

namespace Serilog.Sinks.BepInEx.Extensions;

/// <summary>
/// Contains extension methods that add functionality to <see cref="LogEventLevel"/>.
/// </summary>
public static class LogEventLevelExtensions
{
    /// <summary>
    /// Get the corresponding <see cref="LogLevel"/> for this <see cref="LogEventLevel"/>.
    /// </summary>
    /// <param name="level">The <see cref="LogEventLevel"/> to map.</param>
    /// <returns>The appropriate <see cref="LogLevel"/>.</returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="level"/> is not a recognised <see cref="LogEventLevel"/>.</exception>
    public static LogLevel ToBepInExLevel(this LogEventLevel level) => level switch {
        LogEventLevel.Verbose => LogLevel.Debug,
        LogEventLevel.Debug => LogLevel.Info,
        LogEventLevel.Information => LogLevel.Message,
        LogEventLevel.Warning => LogLevel.Warning,
        LogEventLevel.Error => LogLevel.Error,
        LogEventLevel.Fatal => LogLevel.Fatal,
        _ => throw new ArgumentOutOfRangeException(nameof(level), level, $"Unrecognised {nameof(LogEventLevel)}"),
    };
}
