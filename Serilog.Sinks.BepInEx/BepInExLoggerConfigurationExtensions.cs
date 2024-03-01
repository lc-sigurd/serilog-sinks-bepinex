/*
 * This file is largely based upon
 * https://github.com/Lordfirespeed/serilog-sinks-msbuild/blob/4745c56974edeccaabda8e2982b6950f3076a326/Serilog.Sinks.MSBuild/MSBuildLoggerConfigurationExtensions.cs
 * Copyright 2019 Theodore Tsirpanis
 * Copyright (c) 2024 Joe Clack
 * Joe Clack licenses the referenced file to the Sigurd Team under the LGPL-3.0-OR-LATER license.
 *
 * Copyright (c) 2024 Sigurd Team
 * The Sigurd Team licenses this file to you under the LGPL-3.0-OR-LATER license.
 */

using System;
using BepInEx.Logging;
using Serilog.Events;
using Serilog.Configuration;
using Serilog.Sinks.BepInEx;
using Serilog.Sinks.BepInEx.Output;
using Serilog.Sinks.BepInEx.Themes;

namespace Serilog;

/// <summary>
/// Adds extension methods to <see cref="LoggerConfiguration"/> related to configuring <see cref="LogEvent"/>
/// redirection to BepInEx via <see cref="BepInExLogSink"/>.
/// </summary>
public static class BepInExLoggerConfigurationExtensions
{
    /// <summary>
    /// <code>"[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}"</code>
    /// </summary>
    private const string DefaultBepInExConsoleOutputTemplate = "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}";

    /// <summary>
    /// Redirects log events to the BepInEx logger via an <see cref="ILogSource"/>.
    /// </summary>
    /// <param name="sinkConfiguration">Logger sink configuration.</param>
    /// <param name="logSourceName">A <see cref="ILogSource.SourceName"/> to use for the created <see cref="ILogSource"/>.</param>
    /// <param name="outputTemplate">A message template describing the format used to write to the sink.
    /// If not specified, uses <see cref="DefaultBepInExConsoleOutputTemplate"/>.</param>
    /// <param name="formatProvider">Supplies culture-specific formatting information. Can be <see langword="null"/>.</param>
    /// <param name="theme">The theme to apply to the styled output. If not specified,
    /// uses <see cref="AnsiBepInExConsoleTheme.Literate"/>.</param>
    /// <returns>Configuration object allowing method chaining.</returns>
    /// <remarks>Because this sink redirects messages to another logging system,
    /// it is recommended to allow all event levels to pass through.</remarks>
    public static LoggerConfiguration BepInExLogger(
        this LoggerSinkConfiguration sinkConfiguration,
        string logSourceName,
        string outputTemplate = DefaultBepInExConsoleOutputTemplate,
        IFormatProvider? formatProvider = null,
        BepInExConsoleTheme? theme = null)
    {
        if (sinkConfiguration is null) throw new ArgumentNullException(nameof(sinkConfiguration));
        if (logSourceName is null) throw new ArgumentNullException(nameof(logSourceName));

        theme ??= AnsiBepInExConsoleTheme.Literate;

        var formatter = new OutputTemplateRenderer(theme, outputTemplate, formatProvider);
        var logSource = new SerilogLogSource(logSourceName);
        return sinkConfiguration.Sink(new BepInExLogSink(logSource, theme, formatter));
    }
}
