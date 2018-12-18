# WcfLinq

## Create a ServiceClient

```csharp
var serviceClient = new ServiceClientMasterData100000("net.tcp://localhost:10000/MasterData/"))
```

## Choose the model with a CRUD action
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

## Filter, Sort, Padding

```csharp
serviceClient. ... .Where(model => model function)
serviceClient. ... .OrderBy(model => model function)
serviceClient. ... .OrderByDescending(model => model function)
serviceClient. ... .Paging(page: 2, pageSize: 10)
```


## Execute 

### Execute with Model as result
```csharp
serviceClient. ... .ExecuteFirstOrDefault()
serviceClient. ... .ExecuteFirstOrDefaultAsync()
```
### Execute with List&lt;Model&gt; as result
```csharp
serviceClient. ... .ExecuteToList()
serviceClient. ... .ExecuteToListAsync()
```



    












## Info
Read Works
Create, Delete, Update throw a not implement exception
