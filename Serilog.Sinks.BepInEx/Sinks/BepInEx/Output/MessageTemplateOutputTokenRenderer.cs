/*
 * This file is largely based upon
 * https://github.com/Lordfirespeed/serilog-sinks-msbuild/blob/4745c56974edeccaabda8e2982b6950f3076a326/Serilog.Sinks.MSBuild/Sinks/MSBuild/Output/MessageTemplateOutputTokenRenderer.cs
 * Copyright 2017 Serilog Contributors
 * Copyright (c) 2024 Joe Clack
 * Joe Clack licenses the referenced file to the Sigurd Team under the LGPL-3.0-OR-LATER license.
 *
 * Copyright (c) 2024 Sigurd Team
 * The Sigurd Team licenses this file to you under the LGPL-3.0-OR-LATER license.
 */

using System;
using System.IO;
using Serilog.Events;
using Serilog.Parsing;
using Serilog.Sinks.BepInEx.Formatting;
using Serilog.Sinks.BepInEx.Rendering;
using Serilog.Sinks.BepInEx.Themes;

namespace Serilog.Sinks.BepInEx.Output;

class MessageTemplateOutputTokenRenderer : OutputTemplateTokenRenderer
{
    readonly BepInExConsoleTheme _theme;
    readonly PropertyToken _token;
    readonly ThemedMessageTemplateRenderer _renderer;

    public MessageTemplateOutputTokenRenderer(BepInExConsoleTheme theme, PropertyToken token, IFormatProvider? formatProvider)
    {
        _theme = theme ?? throw new ArgumentNullException(nameof(theme));
        _token = token ?? throw new ArgumentNullException(nameof(token));

        bool isLiteral = false, isJson = false;

        if (token.Format != null)
        {
            for (var i = 0; i < token.Format.Length; ++i)
            {
                if (token.Format[i] == 'l')
                    isLiteral = true;
                else if (token.Format[i] == 'j')
                    isJson = true;
            }
        }

        var valueFormatter = isJson
            ? (ThemedValueFormatter)new ThemedJsonValueFormatter(theme, formatProvider)
            : new ThemedDisplayValueFormatter(theme, formatProvider);

        _renderer = new ThemedMessageTemplateRenderer(theme, valueFormatter, isLiteral);
    }

    public override void Render(LogEvent logEvent, BepInExLogContext context, TextWriter output)
    {
        if (_token.Alignment is null || !_theme.CanBuffer)
        {
            _renderer.Render(logEvent.MessageTemplate, logEvent.Properties, context, output);
            return;
        }

        var buffer = new StringWriter();
        var invisible = _renderer.Render(logEvent.MessageTemplate, logEvent.Properties, context, buffer);
        var value = buffer.ToString();
        Padding.Apply(output, value, _token.Alignment.Value.Widen(invisible));
    }
}
