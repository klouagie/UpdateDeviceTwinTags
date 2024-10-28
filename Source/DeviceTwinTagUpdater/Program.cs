//// See https://aka.ms/new-console-template for more information
//Console.WriteLine("Hello, World!");
namespace DeviceTwinTagUpdater
{
    using System;
    using System.CommandLine;
    using UpdateDeviceTwinTags;

    public class Program
    {
        public static async Task Main(string[] args)
        {
            // Set up command-line arguments
            var firstIdOption = new Option<int>("--first_id", "The first device ID in the range") { IsRequired = true };
            var lastIdOption = new Option<int?>("--last_id", "The last device ID in the range");
            var tagKeyOption = new Option<string>("--tag_key", () => "version", "The key to set for the tag");
            var tagValueOption = new Option<string>("--tag_value", "The value to set for the tag") { IsRequired = true };


            var rootCommand = new RootCommand("Update device twin tags in Azure IoT Hub")
            {
                firstIdOption,
                lastIdOption,
                tagKeyOption,
                tagValueOption
            };

            rootCommand.SetHandler(async (int firstId, int? lastId, string tagKey, string tagValue) =>
            {
                await MainAsync(firstId, tagValue, lastId, tagKey)
                    .ConfigureAwait(false);
            },
            firstIdOption, lastIdOption, tagKeyOption, tagValueOption);

            await rootCommand.InvokeAsync(args);
        }

        public static async Task MainAsync(int firstId, string tagValue, int? lastId = null, string tagKey = "version")
        {
            var updater = new DeviceTwinTagUpdater();

            if (lastId == null)
            {
                // Update a single device twin
                await updater
                    .UpdateTwinAsync(firstId.ToString(), tagValue)
                    .ConfigureAwait(false);
            }
            else
            {
                // Get range of IDs and update device twins asynchronously
                ICollection<int> idRange = updater.GetIdRange(firstId, lastId.Value);
                Console.WriteLine("ID Range: " + string.Join(", ", idRange));

                await updater
                    .UpdateTwinsAsync([.. idRange.Select(id => id.ToString())], tagValue: tagValue)
                    .ConfigureAwait(false);
            }

            Console.WriteLine("Finished");
        }
    }
}
