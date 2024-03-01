/*
 * This file is largely based upon
 * https://github.com/Lordfirespeed/serilog-sinks-msbuild/blob/4745c56974edeccaabda8e2982b6950f3076a326/Serilog.Sinks.MSBuild/Sinks/MSBuild/Formatting/ThemedDisplayValueFormatter.cs
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
using Serilog.Formatting.Json;
using Serilog.Sinks.BepInEx.Themes;

namespace Serilog.Sinks.BepInEx.Formatting;

class ThemedDisplayValueFormatter : ThemedValueFormatter
{
    readonly IFormatProvider? _formatProvider;

    public ThemedDisplayValueFormatter(BepInExConsoleTheme theme, IFormatProvider? formatProvider)
        : base(theme)
    {
        _formatProvider = formatProvider;
    }

    public override ThemedValueFormatter SwitchTheme(BepInExConsoleTheme theme)
    {
        return new ThemedDisplayValueFormatter(theme, _formatProvider);
    }

    protected override int VisitScalarValue(ThemedValueFormatterState state, ScalarValue scalar)
    {
        if (scalar is null)
            throw new ArgumentNullException(nameof(scalar));
        return FormatLiteralValue(scalar, state.LogContext, state.Output, state.Format);
    }

    protected override int VisitSequenceValue(ThemedValueFormatterState state, SequenceValue sequence)
    {
        if (sequence is null)
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
            Visit(state, sequence.Elements[index]);
        }

        using (ApplyStyle(state.LogContext, state.Output, BepInExConsoleThemeStyle.TertiaryText, ref count))
            state.Output.Write(']');

        return count;
    }

    protected override int VisitStructureValue(ThemedValueFormatterState state, StructureValue structure)
    {
        var count = 0;

        if (structure.TypeTag != null)
        {
            using (ApplyStyle(state.LogContext, state.Output, BepInExConsoleThemeStyle.Name, ref count))
                state.Output.Write(structure.TypeTag);

            state.Output.Write(' ');
        }

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
                state.Output.Write(property.Name);

            using (ApplyStyle(state.LogContext, state.Output, BepInExConsoleThemeStyle.TertiaryText, ref count))
                state.Output.Write('=');

            count += Visit(state.Nest(), property.Value);
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

            using (ApplyStyle(state.LogContext, state.Output, BepInExConsoleThemeStyle.TertiaryText, ref count))
                state.Output.Write('[');

            using (ApplyStyle(state.LogContext, state.Output, BepInExConsoleThemeStyle.String, ref count))
                count += Visit(state.Nest(), element.Key);

            using (ApplyStyle(state.LogContext, state.Output, BepInExConsoleThemeStyle.TertiaryText, ref count))
                state.Output.Write("]=");

            count += Visit(state.Nest(), element.Value);
        }

        using (ApplyStyle(state.LogContext, state.Output, BepInExConsoleThemeStyle.TertiaryText, ref count))
            state.Output.Write('}');

        return count;
    }

    public int FormatLiteralValue(ScalarValue scalar, BepInExLogContext context, TextWriter output, string? format)
    {
        var value = scalar.Value;
        var count = 0;

        if (value is null)
        {
            using (ApplyStyle(context, output, BepInExConsoleThemeStyle.Null, ref count))
                output.Write("null");
            return count;
        }

        if (value is string str)
        {
            using (ApplyStyle(context, output, BepInExConsoleThemeStyle.String, ref count))
            {
                if (format != "l")
                    JsonValueFormatter.WriteQuotedJsonString(str, output);
                else
                    output.Write(str);
            }
            return count;
        }

        if (value is ValueType)
        {
            if (value is int || value is uint || value is long || value is ulong ||
                value is decimal || value is byte || value is sbyte || value is short ||
                value is ushort || value is float || value is double)
            {
                using (ApplyStyle(context, output, BepInExConsoleThemeStyle.Number, ref count))
                    scalar.Render(output, format, _formatProvider);
                return count;
            }

            if (value is bool b)
            {
                using (ApplyStyle(context, output, BepInExConsoleThemeStyle.Boolean, ref count))
                    output.Write(b);

                return count;
            }

            if (value is char ch)
            {
                using (ApplyStyle(context, output, BepInExConsoleThemeStyle.Scalar, ref count))
                {
                    output.Write('\'');
                    output.Write(ch);
                    output.Write('\'');
                }
                return count;
            }
        }

        using (ApplyStyle(context, output, BepInExConsoleThemeStyle.Scalar, ref count))
            scalar.Render(output, format, _formatProvider);

        return count;
    }
}
