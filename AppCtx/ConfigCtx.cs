using System;
using System.IO;

using Microsoft.Extensions.PlatformAbstractions;

using Mono.Options;

namespace Note.AppCtx
{
    public class ConfigCtx
    {
        public string BaseDirectory { get; internal set; } = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), $".{nameof(Note)}");

        public string AppDbDirectory
        {
            get
            {
                return Path.Combine(BaseDirectory, "AppDb");
            }
        }

        public string AppDbFile
        {
            get
            {
                return Path.Combine(AppDbDirectory, $"{nameof(Note)}.sqlite");
            }
        }

        public string AppDbConnectionString
        {
            get
            {
                return $"Data Source={AppDbFile};";
            }
        }

        public string LogDirectory
        {
            get
            {
                return Path.Combine(BaseDirectory, "Log");
            }
        }

        public void ParseOptions(string[] args)
        {
            bool helpRequested = false;

            OptionSet optionSet = new OptionSet
            { 
                { "baseDirectory=", "App BaseDirectory", baseDirectory => BaseDirectory = baseDirectory },                

                { "help", "Show this message and exit", help => helpRequested = help != null }
            };            

            try {
                optionSet.Parse(args);

                if (helpRequested) {
                    Help(optionSet);

                    Environment.Exit(0);
                }
            } catch (OptionException e) {
                Console.WriteLine($"Error: {e.Message}");
                Console.WriteLine($"Try '{PlatformServices.Default.Application.ApplicationName} --help' for more information.");

                Environment.Exit(1);
            }   
        }

        public void Help(OptionSet optionSet)
        {
            Console.WriteLine($"Usage: {PlatformServices.Default.Application.ApplicationName} [OPTIONS]");

            Console.WriteLine("Options:");
            optionSet.WriteOptionDescriptions(Console.Out);
        }
    }
}