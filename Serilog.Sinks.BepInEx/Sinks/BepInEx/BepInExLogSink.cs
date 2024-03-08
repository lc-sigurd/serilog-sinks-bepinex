/*
 * This file is largely based upon
 * https://github.com/Lordfirespeed/serilog-sinks-msbuild/blob/4745c56974edeccaabda8e2982b6950f3076a326/Serilog.Sinks.MSBuild/Sinks/MSBuild/MSBuildTaskLogSink.cs
 * Copyright 2019 Theodore Tsirpanis
 * UnityNetcodePatcher Copyright (c) 2023 EvaisaDev
 * Copyright (c) 2024 Joe Clack
 * Joe Clack licenses the referenced file to the Sigurd Team under the LGPL-3.0-OR-LATER license.
 *
 * Copyright (c) 2024 Sigurd Team
 * The Sigurd Team licenses this file to you under the LGPL-3.0-OR-LATER license.
 */

using System;
using System.IO;
using System.Text;
using BepInEx.Logging;
using Serilog.Core;
using Serilog.Events;
using Serilog.Sinks.BepInEx.Formatting;
using Serilog.Sinks.BepInEx.Output;
using Serilog.Sinks.BepInEx.Themes;
using BepInExLogger = BepInEx.Logging.Logger;

namespace Serilog.Sinks.BepInEx;

/// <summary>
/// A <see cref="Serilog"/> sink that redirects <see cref="LogEvent"/>s to a BepInEx <see cref="ManualLogSource"/>
/// instance.
/// </summary>
public class BepInExLogSink : ILogEventSink, IDisposable
{
    const int DefaultWriteBufferCapacity = 256;

    private readonly IBepInExTextFormatter _plainFormatter;
    private readonly IBepInExTextFormatter _formatter;
    private readonly BepInExConsoleTheme _theme;
    private readonly SerilogLogSource _logSource;

    /// <summary>
    /// Creates a <see cref="BepInExLogSink"/> from a <see cref="ManualLogSource"/>.
    /// </summary>
    /// <param name="logSource">The <see cref="ManualLogSource"/> to which log events will be sent.</param>
    /// <param name="theme">The theme to apply to the styled output.</param>
    /// <param name="outputTemplate">Supplies the message template used for rendering.</param>
    /// <param name="formatProvider">Supplies culture-specific formatting information. Can be <see langword="null"/>.</param>
    public BepInExLogSink(SerilogLogSource logSource, BepInExConsoleTheme theme, string outputTemplate, IFormatProvider? formatProvider)
    {
        _logSource = logSource ?? throw new ArgumentNullException(nameof(logSource));
        _theme = theme ?? throw new ArgumentNullException(nameof(theme));
        if (outputTemplate is null) throw new ArgumentNullException(nameof(outputTemplate));

        _plainFormatter = new OutputTemplateRenderer(BepInExConsoleTheme.None, outputTemplate, formatProvider);
        _formatter = new OutputTemplateRenderer(theme, outputTemplate, formatProvider);

        BepInExLogger.Sources.Add(_logSource);
    }

    /// <inheritdoc cref="ILogEventSink.Emit"/>
    public void Emit(LogEvent logEvent)
    {
        if (!_theme.CanBuffer)
            throw new NotSupportedException("BepInEx log themes must support buffering.");

        var context = BepInExLogContext.FromLogEvent(logEvent, _logSource.SourceName);

        var plainBuffer = new StringWriter(new StringBuilder(DefaultWriteBufferCapacity));
        _plainFormatter.Format(logEvent, context, plainBuffer);
        var plainMessage = plainBuffer.ToString().TrimEnd();

        var buffer = new StringWriter(new StringBuilder(DefaultWriteBufferCapacity));
        _formatter.Format(logEvent, context, buffer);
        var message = buffer.ToString().TrimEnd();

        _logSource.Log(context.Level, plainMessage, message);
    }

    public void Dispose()
    {
        BepInExLogger.Sources.Remove(_logSource);
        _logSource.Dispose();
    }
}
