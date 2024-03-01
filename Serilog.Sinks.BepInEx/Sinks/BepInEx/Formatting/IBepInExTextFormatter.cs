/*
 * This file is largely based upon
 * https://github.com/Lordfirespeed/serilog-sinks-msbuild/blob/4745c56974edeccaabda8e2982b6950f3076a326/Serilog.Sinks.MSBuild/Sinks/MSBuild/Formatting/IMSBuildTextFormatter.cs
 * Copyright 2013-2015 Serilog Contributors
 * Copyright (c) 2024 Joe Clack
 * Joe Clack licenses the referenced file to the Sigurd Team under the LGPL-3.0-OR-LATER license.
 *
 * Copyright (c) 2024 Sigurd Team
 * The Sigurd Team licenses this file to you under the LGPL-3.0-OR-LATER license.
 */

using System.IO;
using Serilog.Events;

namespace Serilog.Sinks.BepInEx.Formatting;

/// <summary>
/// Formats BepInEx log events in a textual representation.
/// </summary>
public interface IBepInExTextFormatter
{
    /// <summary>
    /// Format the log event into the output.
    /// </summary>
    /// <param name="logEvent">The event to format.</param>
    /// <param name="context">Output <see cref="BepInExLogContext"/>.</param>
    /// <param name="output">The output.</param>
    void Format(LogEvent logEvent, BepInExLogContext context, TextWriter output);
}
