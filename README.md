# UpdateDeviceTwinTags
API to update a set of Azure IOT Hub device twin tags

# File structure
### Requirements
requirements.txt holds all required libraries. Install the libraries with the following terminal command.
> pip install -r requirements.txt

### Configuration file
Scipt requires the creation of a config file in the root folder: config.ini

```python
[CONNECTIONS]
iot_hub = <iot-hub-connection-string>
```
## Python script usage

| **Params** | **occurence** | **values** | **comment**                                                                                               |
|------------|---------------|------------|-----------------------------------------------------------------------------------------------------------|
| first_id   | required      | int        | The first id number of the range to be changed.                                                           |
| last_id    | optional      | int        | The last id number of the range to be changed. If ommitted, only the twin tag of the first_id is updated. |
| tag_key    | optional      | str        | The tag key to be targeted. If ommitted, default value is 'version'.                                      |
| tag_value  | required      | str        | The new tag value.                                                                                        |

#### Example
> python .\src\main.py --first_id 0 --last_id 50 --tag_key version --tag_value 0