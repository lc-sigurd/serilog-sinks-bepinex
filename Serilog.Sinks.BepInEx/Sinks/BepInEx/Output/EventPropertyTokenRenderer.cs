/*
 * This file is largely based upon
 * https://github.com/Lordfirespeed/serilog-sinks-msbuild/blob/4745c56974edeccaabda8e2982b6950f3076a326/Serilog.Sinks.MSBuild/Sinks/MSBuild/Output/EventPropertyTokenRenderer.cs
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
using Serilog.Sinks.BepInEx.Rendering;
using Serilog.Sinks.BepInEx.Themes;

namespace Serilog.Sinks.BepInEx.Output;

class EventPropertyTokenRenderer : OutputTemplateTokenRenderer
{
    readonly BepInExConsoleTheme _theme;
    readonly PropertyToken _token;
    readonly IFormatProvider? _formatProvider;

    public EventPropertyTokenRenderer(BepInExConsoleTheme theme, PropertyToken token, IFormatProvider? formatProvider)
    {
        _theme = theme;
        _token = token;
        _formatProvider = formatProvider;
    }

    public override void Render(LogEvent logEvent, BepInExLogContext context, TextWriter output)
    {
        // If a property is missing, don't render anything (message templates render the raw token here).
        if (!logEvent.Properties.TryGetValue(_token.PropertyName, out var propertyValue))
        {
            Padding.Apply(output, string.Empty, _token.Alignment);
            return;
        }

        var _ = 0;
        using (_theme.Apply(context, output, BepInExConsoleThemeStyle.SecondaryText, ref _))
        {
            var writer = _token.Alignment.HasValue ? new StringWriter() : output;

            // If the value is a scalar string, support some additional formats: 'u' for uppercase
            // and 'w' for lowercase.
            if (propertyValue is ScalarValue sv && sv.Value is string literalString)
            {
                var cased = Casing.Format(literalString, _token.Format);
                writer.Write(cased);
            }
            else
            {
                propertyValue.Render(writer, _token.Format, _formatProvider);
            }

            if (_token.Alignment.HasValue)
            {
                var str = writer.ToString()!;
                Padding.Apply(output, str, _token.Alignment);
            }
        }
    }
}
