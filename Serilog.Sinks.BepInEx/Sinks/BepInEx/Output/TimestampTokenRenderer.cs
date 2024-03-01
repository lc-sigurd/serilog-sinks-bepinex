/*
 * This file is largely based upon
 * https://github.com/Lordfirespeed/serilog-sinks-msbuild/blob/4745c56974edeccaabda8e2982b6950f3076a326/Serilog.Sinks.MSBuild/Sinks/MSBuild/Output/TimestampTokenRenderer.cs
 * Copyright 2017 Serilog Contributors
 * Copyright (c) 2024 Joe Clack
 * Joe Clack licenses the referenced file to the Sigurd Team under the LGPL-3.0-OR-LATER license.
 *
 * Copyright (c) 2024 Sigurd Team
 * The Sigurd Team licenses this file to you under the LGPL-3.0-OR-LATER license.
 */

using System;
using System.Globalization;
using System.IO;
using Serilog.Events;
using Serilog.Parsing;
using Serilog.Sinks.BepInEx.Rendering;
using Serilog.Sinks.BepInEx.Themes;

namespace Serilog.Sinks.BepInEx.Output;

class TimestampTokenRenderer : OutputTemplateTokenRenderer
{
    readonly BepInExConsoleTheme _theme;
    readonly PropertyToken _token;
    readonly IFormatProvider? _formatProvider;

    public TimestampTokenRenderer(BepInExConsoleTheme theme, PropertyToken token, IFormatProvider? formatProvider)
    {
        _theme = theme;
        _token = token;
        _formatProvider = formatProvider;
    }

    public override void Render(LogEvent logEvent, BepInExLogContext context, TextWriter output)
    {
        var sv = new DateTimeOffsetValue(logEvent.Timestamp);

        var _ = 0;
        using (_theme.Apply(context, output, BepInExConsoleThemeStyle.SecondaryText, ref _))
        {
            if (_token.Alignment is null)
            {
                sv.Render(output, _token.Format, _formatProvider);
            }
            else
            {
                var buffer = new StringWriter();
                sv.Render(buffer, _token.Format, _formatProvider);
                var str = buffer.ToString();
                Padding.Apply(output, str, _token.Alignment);
            }
        }
    }

    readonly struct DateTimeOffsetValue
    {
        public DateTimeOffsetValue(DateTimeOffset value)
        {
            Value = value;
        }

        public DateTimeOffset Value { get; }

        public void Render(TextWriter output, string? format = null, IFormatProvider? formatProvider = null)
        {
            var custom = (ICustomFormatter?)formatProvider?.GetFormat(typeof(ICustomFormatter));
            if (custom != null)
            {
                output.Write(custom.Format(format, Value, formatProvider));
                return;
            }

#if FEATURE_SPAN
                Span<char> buffer = stackalloc char[32];
                if (Value.TryFormat(buffer, out int written, format, formatProvider ?? CultureInfo.InvariantCulture))
                    output.Write(buffer.Slice(0, written));
                else
                    output.Write(Value.ToString(format, formatProvider ?? CultureInfo.InvariantCulture));
#else
            output.Write(Value.ToString(format, formatProvider ?? CultureInfo.InvariantCulture));
#endif
        }
    }
}
