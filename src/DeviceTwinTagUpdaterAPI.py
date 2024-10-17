from azure.iot.hub import IoTHubRegistryManager
import configparser
import asyncio

class DeviceTwinTagUpdater:
    def __init__(self):
        # Get connection string from config.ini file
        config = configparser.ConfigParser()
        config.read('config.ini')
        try:
            iot_hub_connection_string = config['CONNECTIONS']['iot_hub']
        except KeyError as e:
            raise KeyError(f"Missing key in configuration file: {e}")

        self.registry_manager = IoTHubRegistryManager(iot_hub_connection_string)


    async def get_device_twin(self, device_id: str):
        return self.registry_manager.get_twin(device_id)
    
    async def get_all_device_ids(self):
        devices = self.registry_manager.get_devices()
        return devices.id
    
    async def update_twin(self, device_id: str, tag_value: str, tag_key: str = 'version'):
        twin = self.registry_manager.get_twin(device_id)

        twin_patch = {
            "tags": {
                tag_key: tag_value
            }
        }

        return self.registry_manager.update_twin(device_id, twin_patch, twin.etag)
    
    async def update_twins(self, device_id_list, tag_value: str, tag_key: str = 'version'):
        tasks = []
        for id in device_id_list:
            if id == 0:
                device_id = 'test'
            else:
                device_id = 'test' + str(id)
            
            tasks.append(self.update_twin(device_id, tag_value, tag_key))
            results = await asyncio.gather(*tasks, return_exceptions=True)

            for result, id in zip(results, device_id_list):
                if isinstance(result, Exception):
                    print(f'No device found with id: test{id}. Error: {result}')
                else:
                    print(f'Device test{id} updated successfully.')

    async def get_all_device_ids(self):
        devices = self.registry_manager.get_devices()
        return [device.device_id for device in devices]
    
    def get_id_range(self, first_id: int, last_id: int):
        if first_id > last_id:
            raise ValueError("first_id should be less than or equal to last_id")
        return list(range(first_id, last_id + 1))

