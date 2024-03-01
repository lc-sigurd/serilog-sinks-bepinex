/*
 * This file is largely based upon
 * https://github.com/Lordfirespeed/serilog-sinks-msbuild/blob/4745c56974edeccaabda8e2982b6950f3076a326/Serilog.Sinks.MSBuild/Sinks/MSBuild/Extensions/LogEventExtensions.cs
 * Copyright (c) 2024 Joe Clack
 * Joe Clack licenses the referenced file to the Sigurd Team under the LGPL-3.0-OR-LATER license.
 *
 * Copyright (c) 2024 Sigurd Team
 * The Sigurd Team licenses this file to you under the LGPL-3.0-OR-LATER license.
 */

using System;
using Serilog.Events;

namespace Serilog.Sinks.BepInEx.Extensions;

/// <summary>
/// Contains extension methods that add functionality to <see cref="LogEvent"/>.
/// </summary>
public static class LogEventExtensions
{
    /// <summary>
    /// Render a <see cref="LogEvent"/>'s <see cref="LogEvent.Exception"/> to a 'pretty' string.
    /// </summary>
    /// <param name="logEvent">The <see cref="LogEvent"/> whose <see cref="LogEvent.Exception"/> will be rendered.</param>
    /// <param name="styleFlags"><see cref="ExceptionRenderStyleFlags"/> to use to render the topmost exception(s).</param>
    /// <param name="recursiveStyleFlags"><see cref="ExceptionRenderStyleFlags"/> to use when rendering inner exceptions.</param>
    /// <returns><paramref name="logEvent"/>'s <see cref="LogEvent.Exception"/> rendered to a <see cref="string"/>.</returns>
    /// <remarks><paramref name="recursiveStyleFlags"/> defaults to <paramref name="styleFlags"/> when omitted or <see langword="null"/>.</remarks>
    /// <exception cref="InvalidOperationException"><paramref name="logEvent"/>'s <see cref="LogEvent.Exception"/> is <see langword="null"/></exception>
    /// <seealso cref="ExceptionExtensions.Render"/>
    public static string RenderException(this LogEvent logEvent, ExceptionRenderStyleFlags styleFlags, ExceptionRenderStyleFlags? recursiveStyleFlags = null)
    {
        if (logEvent.Exception is null)
            throw new InvalidOperationException($"{nameof(logEvent)}.{nameof(logEvent.Exception)} cannot be null");

        return logEvent.Exception.Render(styleFlags, recursiveStyleFlags);
    }
}
