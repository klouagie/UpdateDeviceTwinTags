namespace UpdateDeviceTwinTags;

using System.Configuration;
using System.Data;
using Microsoft.Azure.Devices;
using Microsoft.Azure.Devices.Shared;

public class DeviceTwinTagUpdater
{
    private string? _connectionString;
    private readonly RegistryManager _registryManager;

    public DeviceTwinTagUpdater()
    {
        _connectionString = ConfigurationManager.AppSettings["IOT_HUB"] ?? throw new MissingPrimaryKeyException("Missing key in configuration file: IOT_HUB");
        _registryManager = RegistryManager.CreateFromConnectionString(_connectionString);
    }

    public async Task<Twin> GetDeviceTwinAsync(string deviceId)
    {
        return await _registryManager
            .GetTwinAsync(deviceId)
            .ConfigureAwait(false);
    }

    public async Task<ICollection<string>> GetAllDeviceIdsAsync()
    {
        IQuery query = _registryManager
            .CreateQuery("select * from devices");
        IEnumerable<Twin> devices = await query.GetNextAsTwinAsync()
            .ConfigureAwait(false);

        return [.. devices.Select(d => d.DeviceId)];
    }

    public async Task<Twin> UpdateTwinAsync(string deviceId, string tagValue, string tagKey = "version")
    {
        Twin twin = await _registryManager
            .GetTwinAsync(deviceId)
            .ConfigureAwait(false);

        string twinPatch =
            "{" +
                "\"tags\": {" +
                    $"{tagKey}: {tagValue}" +
                "}" +
            "}";

        return await _registryManager.UpdateTwinAsync(deviceId, twinPatch, twin.ETag)
            .ConfigureAwait(false);
    }

    public async Task UpdateTwinsAsync(ICollection<string> deviceIds, string tagValue, string tagKey = "version")
    {
        List<Task> tasks = [];
        List<string> ids = [.. deviceIds];

        foreach (string id in ids)
        {
            string? deviceId;
            if (id == "0")
            {
                deviceId = "test";
            }
            else
            {
                deviceId = $"test{id}";
            }

            tasks.Add(UpdateTwinAsync(deviceId, tagValue, tagKey));
        }

        await Task.WhenAll(tasks)
            .ConfigureAwait(false);

        for (int i = 0; i < tasks.Count; i++)
        {
            var result = tasks[i];
            var id = ids[i];

            if (result.IsFaulted)
            {
                Console.WriteLine($"No device found with id: test{id}. Error: {result.Exception}");
            }
            else
            {
                Console.WriteLine($"Device test{id} updated successfully.");
            }
        }
    }

    public ICollection<int> GetIdRange(int firstId, int lastId)
    {
        ArgumentOutOfRangeException.ThrowIfGreaterThan(firstId, lastId);

        return [.. Enumerable.Range(firstId, lastId - firstId + 1)];
    }
}
