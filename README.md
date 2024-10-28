# UpdateDeviceTwinTags
API to update a set of Azure IOT Hub device twin tags

# File structure
### Library
DeviceTwinTagUpdaterLibrary holds the required library file(s).

### Console Application
DeviceTwinTagUpdater holds the console application.

## Requirements
Script requires the creation of a App.config file on location "Source\DeviceTwinTagUpdater\App.config"

```xml
<?xml version="1.0" encoding="utf-8" ?>
<configuration>
	<appSettings>
		<add key="IOT_HUB" value="connectionString"/>
	</appSettings>
</configuration>
```
## Script usage

| **Params** | **occurence** | **values** | **comment**                                                                                               |
|------------|---------------|------------|-----------------------------------------------------------------------------------------------------------|
| first_id   | required      | int        | The first id number of the range to be changed.                                                           |
| last_id    | optional      | int        | The last id number of the range to be changed. If ommitted, only the twin tag of the first_id is updated. |
| tag_key    | optional      | str        | The tag key to be targeted. If ommitted, default value is 'version'.                                      |
| tag_value  | required      | str        | The new tag value.                                                                                        |

#### Example
> dotnet run --project .\Source\DeviceTwinTagUpdater\DeviceTwinTagUpdater.csproj --first_id 0 --last_id 50 --tag_key version --tag_value 0