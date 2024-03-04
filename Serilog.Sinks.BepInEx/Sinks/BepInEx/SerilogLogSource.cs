/*
 * Copyright (c) 2024 Sigurd Team
 * The Sigurd Team licenses this file to you under the LGPL-3.0-OR-LATER license.
 */

using System;
using BepInEx.Logging;

namespace Serilog.Sinks.BepInEx;

/// <inheritdoc />
public class SerilogLogSource : ILogSource
{
    /// <summary>
    /// Creates a <see cref="SerilogLogSource"/> with the specified <see langword="string"/> source name.
    /// </summary>
    public SerilogLogSource(string sourceName)
    {
        SourceName = sourceName;
    }

    /// <inheritdoc />
    public void Dispose() { }

    /// <inheritdoc />
    public string SourceName { get; }

    /// <inheritdoc />
    public event EventHandler<LogEventArgs>? LogEvent;

    /// <summary>
    /// Log a message of the specified level.
    /// </summary>
    /// <param name="level"><see cref="LogLevel"/> to log at.</param>
    /// <param name="plainMessage"><see langword="string"/> message.</param>
    /// <param name="ansiFormattedMessage">ANSI formatted <see langword="string"/> message.</param>
    public void Log(LogLevel level, string plainMessage, string ansiFormattedMessage) => LogEvent?.Invoke(this, new SerilogLogEventArgs(plainMessage, ansiFormattedMessage, level, this));
}
