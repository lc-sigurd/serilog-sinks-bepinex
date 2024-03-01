/*
 * This file is largely based upon
 * https://github.com/Lordfirespeed/serilog-sinks-msbuild/blob/4745c56974edeccaabda8e2982b6950f3076a326/Serilog.Sinks.MSBuild/Sinks/MSBuild/Themes/MessageClass.cs
 * Copyright (c) 2024 Joe Clack
 * Joe Clack licenses the referenced file to the Sigurd Team under the LGPL-3.0-OR-LATER license.
 *
 * Copyright (c) 2024 Sigurd Team
 * The Sigurd Team licenses this file to you under the LGPL-3.0-OR-LATER license.
 */

using System;
using Serilog.Core;
using Serilog.Events;

namespace Serilog.Sinks.BepInEx.Themes;

/// <summary>
/// Describes the message classes available for applying <see cref="BepInExConsoleThemeStyle"/> to text content
/// in log messages.
/// </summary>
public sealed class MessageClass : ILogEventEnricher
{
    /// <summary>
    /// The <see cref="string"/> key used as the name for the <see cref="LogEventProperty"/> that determines which
    /// <see cref="BepInExConsoleThemeStyle"/> to apply.
    /// </summary>
    /// <seealso cref="Enrich"/>
    public const string PropertyName = nameof(MessageClass);

    /// <summary>
    /// Apply the standard text theme.
    /// </summary>
    /// <seealso cref="BepInExConsoleThemeStyle.Text"/>
    public static MessageClass Default = new MessageClass("Default", BepInExConsoleThemeStyle.Text);

    /// <summary>
    /// Apply a prominently positive 'success' text theme.
    /// </summary>
    /// <seealso cref="BepInExConsoleThemeStyle.SuccessText"/>
    public static MessageClass Success = new MessageClass("Success", BepInExConsoleThemeStyle.SuccessText);

    /// <summary>
    /// Apply a prominently negative 'danger' text theme.
    /// </summary>
    /// <seealso cref="BepInExConsoleThemeStyle.DangerText"/>
    public static MessageClass Danger = new MessageClass("Danger", BepInExConsoleThemeStyle.DangerText);

    /// <summary>
    /// Apply a neutral 'warning' text theme.
    /// </summary>
    /// <seealso cref="BepInExConsoleThemeStyle.WarningText"/>
    public static MessageClass Warning = new MessageClass("Warning", BepInExConsoleThemeStyle.WarningText);

    /// <summary>
    /// The name of this <see cref="MessageClass"/>.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// The <see cref="BepInExConsoleThemeStyle"/> that this <see cref="MessageClass"/> applies to templated message text.
    /// </summary>
    public BepInExConsoleThemeStyle Style { get; }

    private MessageClass(string name, BepInExConsoleThemeStyle style)
    {
        Name = name;
        Style = style;
    }

    /// <inheritdoc />
    public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
    {
        if (logEvent is null) throw new ArgumentNullException(nameof(logEvent));
        if (propertyFactory is null) throw new ArgumentNullException(nameof(propertyFactory));

        logEvent.AddOrUpdateProperty(new LogEventProperty(PropertyName, new ScalarValue(this)));
    }
}
