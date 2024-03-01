/*
 * Copyright (c) 2024 Sigurd Team
 * The Sigurd Team licenses this file to you under the LGPL-3.0-OR-LATER license.
 */

using BepInEx.Logging;

namespace Serilog.Sinks.BepInEx;

/// <inheritdoc />
public class SerilogLogEventArgs : LogEventArgs
{
    /// <inheritdoc />
    public SerilogLogEventArgs(object data, LogLevel level, ILogSource source) : base(data, level, source) { }

    /// <inheritdoc />
    public override string ToString() => $"{Data}";
}
