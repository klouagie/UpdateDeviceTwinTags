import argparse
from DeviceTwinTagUpdaterAPI import DeviceTwinTagUpdater
import asyncio

def main(first_id, tag_value, last_id = -1, tag_key = 'version'):
    updater = DeviceTwinTagUpdater()

    if last_id == -1:
        asyncio.run(updater.update_twin(first_id, tag_value))
    else:
        id_range = updater.get_id_range(first_id, last_id)
        print("ID Range:", id_range)

        # Update device twis asynchronously
        asyncio.run(updater.update_twins(id_range, tag_value= 1))

    print("Finished")

if __name__ == "__main__":
    parser = argparse.ArgumentParser(description='Update device twin tags in Azure IoT Hub.')
    parser.add_argument('--first_id', type=int, required=True, help='The first device ID in the range.')
    parser.add_argument('--last_id', type=int, required=False, help='The last device ID in the range.')
    parser.add_argument('--tag_key', type=str, required=False, help='The value to set for the tag.')
    parser.add_argument('--tag_value', type=str, required=True, help='The value to set for the tag.')

    args = parser.parse_args()
    main(args.first_id, args.tag_value, args.last_id, args.tag_key)