#AppStorageService

Provides a standard, reusable and tested way of saving data in application isolated storage. Intended use case is application settings or reasonably sized user data

Data is JSON serialized and written to the application's own isolated store for each platform.
The Universal Platform implementation also encrypts data before writing it to disk using Windows's own Data Protection API (DPAPI) using a user scoped key.

All AppStorageService implementations on different platforms implement the IAppStorageService interface, defined in AppStorageService.Core (a portable class library) assembly.
This is to allow easier sharing of business logic in PCLs across different platforms (add the AppStorageService.Core NuGet package to import the interface definition).
It also makes it easier to create mocks for unit testing and IOC usage.

##Installation

###Universal apps

```
Install-Package AppStorageService.Universal
```

###Classic desktop applications

```
Install-Package AppStorageService.Desktop
```

##Usage

Data to be handled needs to be marked as serializable.
For example:

```
[DataContract]
public class Data
{
    [DataMember]
    public string StringProperty { get; set; }
}
```

##Instantiate service

```
var service = new AppStorageService<Data>("File.json");
```

###Save data

```
var dataToSave = new Data { StringProperty = "SomeValue" };
await service.SaveDataAsync(dataToSave);
```

###Load data

```
var loadedData = await service.LoadDataAsync();
```

###Delete data
```
await service.DeleteDataAsync();
```

It is recommended to instantiate the service once per datatype and reuse that instance throughout the application

##Avoiding data corruption

To prevent data corruption when performing IO operations, application closure should be delayed to allow them to complete.

```
//request application closure delay

var pollingTime = TimeSpan.FromMilliseconds(100);
while(service.OperationInProgress)
{
    await Task.Delay(pollingTime);
}

//allow application to close
```
