﻿// // Copyright (c) Microsoft Corporation.
// // Licensed under the MIT License.

using EventLogExpert.Library.EventResolvers;
using EventLogExpert.Library.Helpers;
using EventLogExpert.Services;
using EventLogExpert.Store;
using EventLogExpert.Store.EventLog;
using Fluxor;

namespace EventLogExpert;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        Utils.InitTracing();

        var builder = MauiApp.CreateBuilder();

        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
            });

        builder.Services.AddMauiBlazorWebView();
        builder.Services.AddBlazorWebViewDeveloperTools();

        builder.Services.AddSingleton<ITraceLogger>(new DebugLogger(Utils.Trace));

        builder.Services.AddFluxor(options =>
        {
            options.ScanAssemblies(typeof(MauiProgram).Assembly).WithLifetime(StoreLifetime.Singleton);
            options.AddMiddleware<LoggingMiddleware>();
        });

        Directory.CreateDirectory(Utils.DatabasePath);

        builder.Services.AddSingleton<IDatabaseCollectionProvider, DatabaseCollectionProvider>();

        builder.Services.AddTransient<IEventResolver, VersatileEventResolver>();

        builder.Services.AddSingleton<ILogWatcherService, LiveLogWatcherService>();

        builder.Services.AddSingleton<ICurrentVersionProvider, CurrentVersionProvider>();

        builder.Services.AddSingleton<IAppTitleService, AppTitleService>();

        builder.Services.AddSingleton<IUpdateService, UpdateService>();

        return builder.Build();
    }
}
