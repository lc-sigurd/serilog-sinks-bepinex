/*
 * This file is largely based upon
 * https://github.com/Lordfirespeed/serilog-sinks-msbuild/blob/4745c56974edeccaabda8e2982b6950f3076a326/Serilog.Sinks.MSBuild/Sinks/MSBuild/Rendering/ThemedMessageTemplateRenderer.cs
 * Copyright 2017 Serilog Contributors
 * Copyright (c) 2024 Joe Clack
 * Joe Clack licenses the referenced file to the Sigurd Team under the LGPL-3.0-OR-LATER license.
 *
 * Copyright (c) 2024 Sigurd Team
 * The Sigurd Team licenses this file to you under the LGPL-3.0-OR-LATER license.
 */

using System;
using System.Collections.Generic;
using System.IO;
using Serilog.Events;
using Serilog.Parsing;
using Serilog.Sinks.BepInEx.Formatting;
using Serilog.Sinks.BepInEx.Themes;

namespace Serilog.Sinks.BepInEx.Rendering;

class ThemedMessageTemplateRenderer
{
    readonly BepInExConsoleTheme _theme;
    readonly ThemedValueFormatter _valueFormatter;
    readonly bool _isLiteral;
    static readonly BepInExConsoleTheme NoTheme = new EmptyBepInExConsoleTheme();
    readonly ThemedValueFormatter _unthemedValueFormatter;

    public ThemedMessageTemplateRenderer(BepInExConsoleTheme theme, ThemedValueFormatter valueFormatter, bool isLiteral)
    {
        _theme = theme ?? throw new ArgumentNullException(nameof(theme));
        _valueFormatter = valueFormatter;
        _isLiteral = isLiteral;
        _unthemedValueFormatter = valueFormatter.SwitchTheme(NoTheme);
    }

    public int Render(MessageTemplate template, IReadOnlyDictionary<string, LogEventPropertyValue> properties, BepInExLogContext context, TextWriter output)
    {
        var count = 0;
        foreach (var token in template.Tokens)
        {
            if (token is TextToken tt)
            {
                count += RenderTextToken(tt, properties, context, output);
            }
            else
            {
                var pt = (PropertyToken)token;
                count += RenderPropertyToken(pt, properties, context, output);
            }
        }
        return count;
    }

    int RenderTextToken(TextToken tt, IReadOnlyDictionary<string, LogEventPropertyValue> properties, BepInExLogContext context, TextWriter output)
    {
        var count = 0;
        using (_theme.Apply(context, output, GetDefaultedContextTextThemeStyle(), ref count))
            output.Write(tt.Text);
        return count;

        BepInExConsoleThemeStyle GetDefaultedContextTextThemeStyle()
            => GetContextTextThemeStyle() ?? MessageClass.Default.Style;

        BepInExConsoleThemeStyle? GetContextTextThemeStyle()
        {
            if (!properties.TryGetValue(MessageClass.PropertyName, out var propertyValue))
                return null;

            if (propertyValue is not ScalarValue scalarValue)
                return null;

            if (scalarValue.Value is MessageClass @class)
                return @class.Style;

            return null;
        }
    }

    int RenderPropertyToken(PropertyToken pt, IReadOnlyDictionary<string, LogEventPropertyValue> properties, BepInExLogContext context, TextWriter output)
    {
        if (!properties.TryGetValue(pt.PropertyName, out var propertyValue))
        {
            var count = 0;
            using (_theme.Apply(context, output, BepInExConsoleThemeStyle.Invalid, ref count))
                output.Write(pt.ToString());
            return count;
        }

        if (!pt.Alignment.HasValue)
        {
            return RenderValue(_theme, _valueFormatter, propertyValue, context, output, pt.Format);
        }

        var valueOutput = new StringWriter();

        if (!_theme.CanBuffer)
            return RenderAlignedPropertyTokenUnbuffered(pt, context, output, propertyValue);

        var invisibleCount = RenderValue(_theme, _valueFormatter, propertyValue, context, valueOutput, pt.Format);

        var value = valueOutput.ToString();

        if (value.Length - invisibleCount >= pt.Alignment.Value.Width)
        {
            output.Write(value);
        }
        else
        {
            Padding.Apply(output, value, pt.Alignment.Value.Widen(invisibleCount));
        }

        return invisibleCount;
    }

    int RenderAlignedPropertyTokenUnbuffered(PropertyToken pt, BepInExLogContext context, TextWriter output, LogEventPropertyValue propertyValue)
    {
        if (pt.Alignment == null) throw new ArgumentException("The PropertyToken should have a non-null Alignment.", nameof(pt));

        var valueOutput = new StringWriter();
        RenderValue(NoTheme, _unthemedValueFormatter, propertyValue, context, valueOutput, pt.Format);

        var valueLength = valueOutput.ToString().Length;
        if (valueLength >= pt.Alignment.Value.Width)
        {
            return RenderValue(_theme, _valueFormatter, propertyValue, context, output, pt.Format);
        }

        if (pt.Alignment.Value.Direction == AlignmentDirection.Left)
        {
            var invisible = RenderValue(_theme, _valueFormatter, propertyValue, context, output, pt.Format);
            Padding.Apply(output, string.Empty, pt.Alignment.Value.Widen(-valueLength));
            return invisible;
        }

        Padding.Apply(output, string.Empty, pt.Alignment.Value.Widen(-valueLength));
        return RenderValue(_theme, _valueFormatter, propertyValue, context, output, pt.Format);
    }

    int RenderValue(BepInExConsoleTheme theme, ThemedValueFormatter valueFormatter, LogEventPropertyValue propertyValue, BepInExLogContext context, TextWriter output, string? format)
    {
        if (_isLiteral && propertyValue is ScalarValue sv && sv.Value is string)
        {
            var count = 0;
            using (theme.Apply(context, output, BepInExConsoleThemeStyle.String, ref count))
                output.Write(sv.Value);
            return count;
        }

        return valueFormatter.Format(propertyValue, context, output, format, _isLiteral);
    }
}
