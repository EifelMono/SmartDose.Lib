# WcfLinq

Create a ServiceClient

```csharp
var serviceClient = new ServiceClientMasterData100000("net.tcp://localhost:10000/MasterData/"))
```

Select the model with a CRUD action
```csharp
serviceClient.ModelCreate<Model>()
serviceClient.ModelDelete<Model>()
serviceClient.ModelRead<Model>()
serviceClient.ModelUpdate<Model>()

or

serviceClient.Model<Model>().Create()
serviceClient.Model<Model>().Delete()
serviceClient.Model<Model>().Read()
serviceClient.Model<Model>().Update()
```

Execute with Model as result
* ExceuteFirstOrDefault()
* ExceuteFirstOrDefaultAsync()

Execute with List<Model> as result
* ExceuteToList()
* ExceuteToList()












## Info
Read Works
Create, Delete, Update throw a not implement exception
