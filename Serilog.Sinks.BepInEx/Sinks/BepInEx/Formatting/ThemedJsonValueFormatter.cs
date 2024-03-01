/*
 * This file is largely based upon
 * https://github.com/Lordfirespeed/serilog-sinks-msbuild/blob/4745c56974edeccaabda8e2982b6950f3076a326/Serilog.Sinks.MSBuild/Sinks/MSBuild/Formatting/ThemedJsonValueFormatter.cs
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
using Serilog.Formatting.Json;
using Serilog.Sinks.BepInEx.Themes;

namespace Serilog.Sinks.BepInEx.Formatting;

class ThemedJsonValueFormatter : ThemedValueFormatter
{
    readonly ThemedDisplayValueFormatter _displayFormatter;
    readonly IFormatProvider? _formatProvider;

    public ThemedJsonValueFormatter(BepInExConsoleTheme theme, IFormatProvider? formatProvider)
        : base(theme)
    {
        _displayFormatter = new ThemedDisplayValueFormatter(theme, formatProvider);
        _formatProvider = formatProvider;
    }

    public override ThemedValueFormatter SwitchTheme(BepInExConsoleTheme theme)
    {
        return new ThemedJsonValueFormatter(theme, _formatProvider);
    }

    protected override int VisitScalarValue(ThemedValueFormatterState state, ScalarValue scalar)
    {
        if (scalar is null)
            throw new ArgumentNullException(nameof(scalar));

        // At the top level, for scalar values, use "display" rendering.
        if (state.IsTopLevel)
            return _displayFormatter.FormatLiteralValue(scalar, state.LogContext, state.Output, state.Format);

        return FormatLiteralValue(scalar, state.LogContext, state.Output);
    }

    protected override int VisitSequenceValue(ThemedValueFormatterState state, SequenceValue sequence)
    {
        if (sequence == null)
            throw new ArgumentNullException(nameof(sequence));

        var count = 0;

        using (ApplyStyle(state.LogContext, state.Output, BepInExConsoleThemeStyle.TertiaryText, ref count))
            state.Output.Write('[');

        var delim = string.Empty;
        for (var index = 0; index < sequence.Elements.Count; ++index)
        {
            if (delim.Length != 0)
            {
                using (ApplyStyle(state.LogContext, state.Output, BepInExConsoleThemeStyle.TertiaryText, ref count))
                    state.Output.Write(delim);
            }

            delim = ", ";
            Visit(state.Nest(), sequence.Elements[index]);
        }

        using (ApplyStyle(state.LogContext, state.Output, BepInExConsoleThemeStyle.TertiaryText, ref count))
            state.Output.Write(']');

        return count;
    }

    protected override int VisitStructureValue(ThemedValueFormatterState state, StructureValue structure)
    {
        var count = 0;

        using (ApplyStyle(state.LogContext, state.Output, BepInExConsoleThemeStyle.TertiaryText, ref count))
            state.Output.Write('{');

        var delim = string.Empty;
        for (var index = 0; index < structure.Properties.Count; ++index)
        {
            if (delim.Length != 0)
            {
                using (ApplyStyle(state.LogContext, state.Output, BepInExConsoleThemeStyle.TertiaryText, ref count))
                    state.Output.Write(delim);
            }

            delim = ", ";

            var property = structure.Properties[index];

            using (ApplyStyle(state.LogContext, state.Output, BepInExConsoleThemeStyle.Name, ref count))
                JsonValueFormatter.WriteQuotedJsonString(property.Name, state.Output);

            using (ApplyStyle(state.LogContext, state.Output, BepInExConsoleThemeStyle.TertiaryText, ref count))
                state.Output.Write(": ");

            count += Visit(state.Nest(), property.Value);
        }

        if (structure.TypeTag != null)
        {
            using (ApplyStyle(state.LogContext, state.Output, BepInExConsoleThemeStyle.TertiaryText, ref count))
                state.Output.Write(delim);

            using (ApplyStyle(state.LogContext, state.Output, BepInExConsoleThemeStyle.Name, ref count))
                JsonValueFormatter.WriteQuotedJsonString("$type", state.Output);

            using (ApplyStyle(state.LogContext, state.Output, BepInExConsoleThemeStyle.TertiaryText, ref count))
                state.Output.Write(": ");

            using (ApplyStyle(state.LogContext, state.Output, BepInExConsoleThemeStyle.String, ref count))
                JsonValueFormatter.WriteQuotedJsonString(structure.TypeTag, state.Output);
        }

        using (ApplyStyle(state.LogContext, state.Output, BepInExConsoleThemeStyle.TertiaryText, ref count))
            state.Output.Write('}');

        return count;
    }

    protected override int VisitDictionaryValue(ThemedValueFormatterState state, DictionaryValue dictionary)
    {
        var count = 0;

        using (ApplyStyle(state.LogContext, state.Output, BepInExConsoleThemeStyle.TertiaryText, ref count))
            state.Output.Write('{');

        var delim = string.Empty;
        foreach (var element in dictionary.Elements)
        {
            if (delim.Length != 0)
            {
                using (ApplyStyle(state.LogContext, state.Output, BepInExConsoleThemeStyle.TertiaryText, ref count))
                    state.Output.Write(delim);
            }

            delim = ", ";

            var style = element.Key.Value == null
                ? BepInExConsoleThemeStyle.Null
                : element.Key.Value is string
                    ? BepInExConsoleThemeStyle.String
                    : BepInExConsoleThemeStyle.Scalar;

            using (ApplyStyle(state.LogContext, state.Output, style, ref count))
                JsonValueFormatter.WriteQuotedJsonString((element.Key.Value ?? "null").ToString() ?? "", state.Output);

            using (ApplyStyle(state.LogContext, state.Output, BepInExConsoleThemeStyle.TertiaryText, ref count))
                state.Output.Write(": ");

            count += Visit(state.Nest(), element.Value);
        }

        using (ApplyStyle(state.LogContext, state.Output, BepInExConsoleThemeStyle.TertiaryText, ref count))
            state.Output.Write('}');

        return count;
    }

    int FormatLiteralValue(ScalarValue scalar, BepInExLogContext context, TextWriter output)
    {
        var value = scalar.Value;
        var count = 0;

        if (value == null)
        {
            using (ApplyStyle(context, output, BepInExConsoleThemeStyle.Null, ref count))
                output.Write("null");
            return count;
        }

        if (value is string str)
        {
            using (ApplyStyle(context, output, BepInExConsoleThemeStyle.String, ref count))
                JsonValueFormatter.WriteQuotedJsonString(str, output);
            return count;
        }

        if (value is ValueType)
        {
            if (value is int || value is uint || value is long || value is ulong || value is decimal || value is byte || value is sbyte || value is short || value is ushort)
            {
                using (ApplyStyle(context, output, BepInExConsoleThemeStyle.Number, ref count))
                    output.Write(((IFormattable)value).ToString(null, CultureInfo.InvariantCulture));
                return count;
            }

            if (value is double d)
            {
                using (ApplyStyle(context, output, BepInExConsoleThemeStyle.Number, ref count))
                {
                    if (double.IsNaN(d) || double.IsInfinity(d))
                        JsonValueFormatter.WriteQuotedJsonString(d.ToString(CultureInfo.InvariantCulture), output);
                    else
                        output.Write(d.ToString("R", CultureInfo.InvariantCulture));
                }
                return count;
            }

            if (value is float f)
            {
                using (ApplyStyle(context, output, BepInExConsoleThemeStyle.Number, ref count))
                {
                    if (double.IsNaN(f) || double.IsInfinity(f))
                        JsonValueFormatter.WriteQuotedJsonString(f.ToString(CultureInfo.InvariantCulture), output);
                    else
                        output.Write(f.ToString("R", CultureInfo.InvariantCulture));
                }
                return count;
            }

            if (value is bool b)
            {
                using (ApplyStyle(context, output, BepInExConsoleThemeStyle.Boolean, ref count))
                    output.Write(b ? "true" : "false");

                return count;
            }

            if (value is char ch)
            {
                using (ApplyStyle(context, output, BepInExConsoleThemeStyle.Scalar, ref count))
                    JsonValueFormatter.WriteQuotedJsonString(ch.ToString(), output);
                return count;
            }

            if (value is DateTime || value is DateTimeOffset)
            {
                using (ApplyStyle(context, output, BepInExConsoleThemeStyle.Scalar, ref count))
                {
                    output.Write('"');
                    output.Write(((IFormattable)value).ToString("O", CultureInfo.InvariantCulture));
                    output.Write('"');
                }
                return count;
            }
        }

        using (ApplyStyle(context, output, BepInExConsoleThemeStyle.Scalar, ref count))
            JsonValueFormatter.WriteQuotedJsonString(value.ToString() ?? "", output);

        return count;
    }
}
