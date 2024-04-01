/*
 * Copyright (c) 2024 Sigurd Team
 * The Sigurd Team licenses this file to you under the LGPL-3.0-OR-LATER license.
 */

using BepInEx.Logging;
using Sigurd.AvaloniaBepInExConsole.Common;
using LogEventArgs = BepInEx.Logging.LogEventArgs;

namespace Serilog.Sinks.BepInEx;

/// <inheritdoc cref="LogEventArgs" />
public class SerilogLogEventArgs : LogEventArgs, IAnsiFormattable
{
    private readonly string _formattedData;

    /// <inheritdoc />
    public SerilogLogEventArgs(string data, string formattedData, LogLevel level, ILogSource source) : base(data, level, source)
    {
        _formattedData = formattedData;
    }

    /// <inheritdoc />
    public override string ToString() => $"{Data}";

    /// <inheritdoc />
    public string ToAnsiFormattedString() => _formattedData;
}
