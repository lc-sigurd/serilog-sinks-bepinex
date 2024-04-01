/*
 * Copyright (c) 2024 Sigurd Team
 * The Sigurd Team licenses this file to you under the LGPL-3.0-OR-LATER license.
 */

using System;
using System.Reflection;
using System.Reflection.Emit;
using BepInEx.Logging;

namespace Serilog.Sinks.BepInEx;

/// <inheritdoc />
public class SerilogLogSource : ILogSource
{
    /// <summary>
    /// Creates a <see cref="SerilogLogSource"/> with the specified <see langword="string"/> source name.
    /// </summary>
    public SerilogLogSource(string sourceName)
    {
        SourceName = sourceName;
    }

    static SerilogLogSource()
    {
        var avaloniaBepinexConsoleCommonAssemblyNameString = "com.sigurd.avalonia_bepinex_console.common";
        var avaloniaBepinexConsoleCommonAssemblyName = new AssemblyName(avaloniaBepinexConsoleCommonAssemblyNameString);

        var assemblyHolder = new Lazy<Assembly>(() => {
            var assembly = AssemblyBuilder.DefineDynamicAssembly(avaloniaBepinexConsoleCommonAssemblyName, AssemblyBuilderAccess.Run);
            var module = assembly.DefineDynamicModule(avaloniaBepinexConsoleCommonAssemblyName.Name);

            var iAnsiFormattableTypeBuilder = module.DefineType("Sigurd.AvaloniaBepInExConsole.Common.IAnsiFormattable", TypeAttributes.Interface | TypeAttributes.Abstract | TypeAttributes.Public);
            iAnsiFormattableTypeBuilder.CreateType();
            return assembly;
        });

        AppDomain.CurrentDomain.AssemblyResolve += (_, args) => {
            var assemblyName = new AssemblyName(args.Name);
            if (assemblyName.Name != avaloniaBepinexConsoleCommonAssemblyName.Name) return null;
            return assemblyHolder.Value;
        };
    }

    /// <inheritdoc />
    public void Dispose() { }

    /// <inheritdoc />
    public string SourceName { get; }

    /// <inheritdoc />
    public event EventHandler<LogEventArgs>? LogEvent;

    /// <summary>
    /// Log a message of the specified level.
    /// </summary>
    /// <param name="level"><see cref="LogLevel"/> to log at.</param>
    /// <param name="plainMessage"><see langword="string"/> message.</param>
    /// <param name="ansiFormattedMessage">ANSI formatted <see langword="string"/> message.</param>
    public void Log(LogLevel level, string plainMessage, string ansiFormattedMessage) => LogEvent?.Invoke(this, new SerilogLogEventArgs(plainMessage, ansiFormattedMessage, level, this));
}
