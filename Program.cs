using System.Net;
using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.SystemConsole.Themes;
using ExplorJobAPI.Auth.Cert;

namespace ExplorJobAPI
{
    public class Program
    {
        public static int Main(
            string[] args
        ) {
            try {
                Log.Information("Starting host...");
                CreateHostBuilder(args).Build().Run();
                return 0;
            }
            catch (Exception e) {
                Log.Fatal(e, "Host terminated unexpectedly.");
                return 1;
            }
            finally {
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(
            string[] args
        ) {
            IHostBuilder builder = Host.CreateDefaultBuilder(args);

            builder.ConfigureAppConfiguration((
                hostingContext,
                configureHost
            ) => {
                configureHost.AddJsonFile(
                    "appsettings.json",
                    optional: true,
                    reloadOnChange: true
                );

                configureHost.AddJsonFile(
                    $"appsettings.{ hostingContext.HostingEnvironment.EnvironmentName }.json",
                    optional: false,
                    reloadOnChange: true
                );

                configureHost.AddEnvironmentVariables();
            });

            builder.ConfigureWebHostDefaults((webBuilder) => {
                webBuilder.UseStartup<Startup>();

                webBuilder.UseSerilog((
                    context,
                    configuration
                ) => {
                    configuration
                        .MinimumLevel.Debug()
                        .MinimumLevel.Override(
                            "Microsoft",
                            LogEventLevel.Warning
                        )
                        .MinimumLevel.Override(
                            "System",
                            LogEventLevel.Warning
                        )
                        .MinimumLevel.Override(
                            "Microsoft.AspNetCore.Authentication",
                            LogEventLevel.Information
                        )
                        .Enrich.FromLogContext()
                        .WriteTo.File(@"./report.log")
                        .WriteTo.Console(
                            outputTemplate: "[{Timestamp:HH:mm:ss} {Level}] {SourceContext}{NewLine}{Message:lj}{NewLine}{Exception}{NewLine}",
                            theme: AnsiConsoleTheme.Literate
                        );
                });

                webBuilder.UseKestrel((
                    context,
                    options
                ) => {
                    options.AddServerHeader = false;
                    options.Listen(
                        IPAddress.Any,
                        int.Parse(Config.Port),
                        (listenOptions) => {
                            if (!context.HostingEnvironment.EnvironmentName.Equals(
                                "Production"
                            )) {
                                listenOptions.UseHttps(
                                    AuthCert.Get()
                                );
                            }
                        }
                    );
                });
            });

            return builder;
        }
    }
}
