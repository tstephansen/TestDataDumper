using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;
using Serilog.Exceptions;
using Serilog.Exceptions.Core;
using Serilog.Exceptions.Destructurers;
using ILogger = Serilog.ILogger;

namespace TestDataDumper;

public static class TestDataDumperLoggerFactory
{
    private static ILogger CreateLogger()
    {
        var logFileLocation = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TestDataDumper.log");
        var loggerConfiguration = new LoggerConfiguration()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
            .Enrich.FromLogContext()
            .Enrich.WithExceptionDetails(new DestructuringOptionsBuilder()
                .WithDefaultDestructurers())
            .WriteTo.Console()
            .WriteTo.Debug()
            .WriteTo.File(logFileLocation, rollingInterval: RollingInterval.Day,
                retainedFileCountLimit: 60,
                restrictedToMinimumLevel: LogEventLevel.Verbose, shared: true,
                outputTemplate:
                "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}{NewLine}{Properties}{NewLine}{NewLine}");
        return loggerConfiguration.CreateLogger();
    }
    
    public static ILogger<T> CreateLogger<T>()
    {
        Logger ??= CreateLogger();
        using var loggerFactory = LoggerFactory.Create(builder =>
        {
            builder.AddSerilog(Logger);
        });
        return loggerFactory.CreateLogger<T>();
    }
    
    public static ILogger? Logger { get; set; }
}