/*
 * This file is based upon
 * https://github.com/lc-sigurd/avalonia-bepinex-console/blob/4d235fddb37bfc8809f6995d4ac98ff957b4760c/AvaloniaBepInExConsole.Common/Extensions/BepInExLogLevelExtensions.cs
 * Copyright (c) 2024 Sigurd Team
 * The Sigurd Team licenses the basis of this file to itself under the LGPL-3.0-OR-LATER license.
 *
 * Copyright (c) 2024 Sigurd Team
 * The Sigurd Team licenses this file to you under the LGPL-3.0-OR-LATER license.
 */

using BepInEx.Logging;

namespace Serilog.Sinks.BepInEx.Extensions;

/// <summary>
/// Contains extension methods that add functionality to <see cref="LogLevel"/>.
/// </summary>
public static class LogLevelExtensions
{
    /// <returns>The appropriate reset string for the <see cref="LogLevel"/>.</returns>
    public static string GetLevelAnsiReset(this LogLevel level)
        => level switch {
            LogLevel.Debug => "\x1b[0;38;5;8m",
            LogLevel.Info => "\x1b[0;38;5;7m",
            LogLevel.Message => "\x1b[m",
            LogLevel.Warning => "\x1b[0;38;5;11m",
            LogLevel.Error => "\x1b[0;1;38;5;1m",
            LogLevel.Fatal => "\x1b[0;1;38;5;9m",
            _ => "\x1b[m"
        };
}
