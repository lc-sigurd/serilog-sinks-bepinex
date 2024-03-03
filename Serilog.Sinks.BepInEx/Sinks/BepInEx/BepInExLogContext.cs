/*
 * This file is largely based upon
 * https://github.com/Lordfirespeed/serilog-sinks-msbuild/blob/4745c56974edeccaabda8e2982b6950f3076a326/Serilog.Sinks.MSBuild/Sinks/MSBuild/MSBuildLogContext.cs
 * Copyright 2019 Theodore Tsirpanis
 * Copyright (c) 2024 Joe Clack
 * Joe Clack licenses the referenced file to the Sigurd Team under the LGPL-3.0-OR-LATER license.
 *
 * Copyright (c) 2024 Sigurd Team
 * The Sigurd Team licenses this file to you under the LGPL-3.0-OR-LATER license.
 */

using BepInEx.Logging;
using Serilog.Events;
using Serilog.Sinks.BepInEx.Extensions;

namespace Serilog.Sinks.BepInEx;

/// <summary>
/// <see cref="LogEvent"/> property names that are significant for <see cref="int"/>
/// and would give MSBuild additional information if specified.
/// </summary>
/// <remarks>All are optional. A value of 0 is used in place of <see langword="null"/> for <see cref="BepInExLogSink"/>s.</remarks>

public record BepInExLogContext
{
    /// <summary>
    /// Populate an <see cref="BepInExLogContext"/> using a SeriLog <see cref="LogEvent"/>.
    /// </summary>
    /// <param name="logEvent">The <see cref="LogEvent"/> used to lookup contextual information.</param>
    /// <returns><see cref="BepInExLogContext"/> populated as much as possible.</returns>
    public static BepInExLogContext FromLogEvent(LogEvent logEvent)
    {
        var level = logEvent.Level.ToBepInExLevel();

        return new BepInExLogContext {
            Level = level,
        };

        object? GetScalarValue(string key)
        {
            if (!logEvent.Properties.TryGetValue(key, out var value)) return null;
            return value is ScalarValue scalarValue ? scalarValue.Value : null;
        }

        string? GetString(string key) => GetScalarValue(key)?.ToString();

        int GetInt(string key)
        {
            var scalarValue = GetScalarValue(key);

            if (scalarValue == null) return 0;
            if (scalarValue is int intValue) return intValue;
            if (int.TryParse(scalarValue.ToString(), out intValue)) return intValue;
            return 0;
        }
    }

    /// <summary>
    /// The message's log level.
    /// </summary>
    public required LogLevel Level { get; init; }
}
