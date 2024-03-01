/*
 * This file is largely based upon
 * https://github.com/Lordfirespeed/serilog-sinks-msbuild/blob/4745c56974edeccaabda8e2982b6950f3076a326/Serilog.Sinks.MSBuild/Sinks/MSBuild/Extensions/ExceptionExtensions.cs
 * Copyright (c) 2024 Joe Clack
 * Joe Clack licenses the referenced file to the Sigurd Team under the LGPL-3.0-OR-LATER license.
 *
 * Copyright (c) 2024 Sigurd Team
 * The Sigurd Team licenses this file to you under the LGPL-3.0-OR-LATER license.
 */

using System;
using System.Linq;
using System.Text;

namespace Serilog.Sinks.BepInEx.Extensions;

/// <summary>
/// Contains extension methods that add functionality to <see cref="Exception"/>.
/// </summary>
public static class ExceptionExtensions
{
    /// <summary>
    /// Render an <see cref="Exception"/> to a 'pretty' string.
    /// </summary>
    /// <param name="exception">The <see cref="Exception"/> to render.</param>
    /// <param name="style"><see cref="ExceptionRenderStyleFlags"/> to use to render the topmost exception(s).</param>
    /// <param name="recursiveStyle"><see cref="ExceptionRenderStyleFlags"/> to use when rendering inner exceptions.</param>
    /// <returns><paramref name="exception"/> rendered to a <see cref="string"/>.</returns>
    /// <remarks><paramref name="recursiveStyle"/> defaults to <paramref name="style"/> when omitted or <see langword="null"/>.</remarks>
    /// <exception cref="ArgumentNullException"><paramref name="exception"/> is <see langword="null"/></exception>
    public static string Render(this Exception exception, ExceptionRenderStyleFlags style, ExceptionRenderStyleFlags? recursiveStyle = null)
    {
        recursiveStyle ??= style;

        return exception switch {
            null => throw new ArgumentNullException(nameof(Exception), $"{nameof(Exception)} cannot be null"),
            AggregateException aggregateException => RenderAggregate(aggregateException, style, recursiveStyle.Value),
            _ => RenderRecursively(exception, style, recursiveStyle.Value),
        };
    }

    /// <summary>
    /// Delimiter that is used to separate inner exceptions of an <see cref="AggregateException"/>.
    /// </summary>
    private static string InnerExceptionDelimiter =>
        $"{Environment.NewLine}{String.Concat(Enumerable.Repeat("\u2500", 10))}{Environment.NewLine}{Environment.NewLine}";

    private static string RenderAggregate(AggregateException aggregateException, ExceptionRenderStyleFlags style, ExceptionRenderStyleFlags recursiveStyle)
    {
        var innerExceptionStrings = aggregateException
            .Flatten()
            .InnerExceptions
            .Select(innerException => innerException.Render(style, recursiveStyle))
            .Select(rendered => rendered.MultilineIndent(4));

        var shouldIncludeType = style.Has(ExceptionRenderStyleFlags.IncludeType);

        return new StringBuilder(200)
            .AppendLine(RenderKernel(aggregateException, shouldIncludeType) ?? "Issues:")
            .Append(shouldIncludeType ? $"{{{Environment.NewLine}" : String.Empty)
            .AppendLine(String.Join(InnerExceptionDelimiter, innerExceptionStrings))
            .Append(shouldIncludeType ? "}" : String.Empty)
            .ToString()
            .TrimEnd();
    }

    private static string? RenderKernel(Exception exception, bool shouldIncludeType)
    {
        var messageHasContent = !string.IsNullOrWhiteSpace(exception.Message);

        if (messageHasContent && shouldIncludeType) return $"{exception.GetType()}: {exception.Message}";

        if (messageHasContent) return exception.Message;

        if (shouldIncludeType) return exception
            .GetType()
            .ToString();

        return null;
    }

    private static string RenderDefault(Exception exception, ExceptionRenderStyleFlags style)
    {
        if (style.Has(ExceptionRenderStyleFlags.IncludeInner))
            throw new NotSupportedException($"{nameof(RenderDefault)} cannot include inner exceptions. Use {nameof(RenderRecursively)}.");

        var builder = new StringBuilder(100);

        builder.AppendLine(RenderKernel(exception, style.Has(ExceptionRenderStyleFlags.IncludeType)) ?? "[no message]");

        if (style.Has(ExceptionRenderStyleFlags.IncludeStackTrace) && exception.StackTrace is not null)
            builder.AppendLine(exception.StackTrace);

        return builder
            .ToString()
            .TrimEnd();
    }

    private static string RenderRecursively(Exception exception, ExceptionRenderStyleFlags style, ExceptionRenderStyleFlags recursiveStyle)
    {
        if (!style.Has(ExceptionRenderStyleFlags.IncludeInner))
            return RenderDefault(exception, style);

        var builder = new StringBuilder(100);

        builder.AppendLine(RenderKernel(exception, style.Has(ExceptionRenderStyleFlags.IncludeType)) ?? "[no message]");

        if (style.Has(ExceptionRenderStyleFlags.IncludeStackTrace) && exception.StackTrace is not null)
            builder.AppendLine(exception.StackTrace);

        if (style.Has(ExceptionRenderStyleFlags.IncludeInner) && exception.InnerException is not null) {
            var indentedInnerExceptionRender = exception.InnerException
                .Render(recursiveStyle)
                .Trim();
            builder.AppendLine($"Caused by: {indentedInnerExceptionRender}");
        }

        return builder
            .ToString()
            .TrimEnd();
    }
}
