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

## Filter, Sort, Paging
```csharp
serviceClient. ... .Where(where function)
serviceClient. ... .OrderBy(orderby function)
serviceClient. ... .OrderByDescending(orderbydescending function)
serviceClient. ... .Paging(page: 2, pageSize: 10)
```

## Select 
```csharp
serviceClient. ... .Where(select function)
```
select manipluates the result of the requesrt<br>
see later in this document

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

### ServiceResult 

| Name |  |
|------|--|
| Data |  result value of the execution |
| Status | integer value of the execution |
| Debug  | Contains debug infos of the execution<br> ModelBuilder.DebugInfoAll(on: true) switch's this on for all |
| ....   | |

Exceptions, including connection errors, are handeld by the execution function on setting the status.

## Info
Read Works<br>
Create, Delete, Update throw a not implement exception



## Samples

### ExecuteFirstOrDefault for one item
result.Data = Model here Medicine
```csharp
if (await serviceClient.ModelRead<Medicine>()
    .Where(m => m.Name.Contains("a"))
    .ExecuteFirstOrDefaultAsync() is var result && result.IsOk())
    Console.WriteLine(result.Data.ToJson());
else
    Console.WriteLine(result.ToErrorString());
```

### ExecuteToList for a list
result.Data = List&lt;Model&gt; here List&lt;Medicine&gt;
```csharp
if (await serviceClient.ModelRead<Medicine>()
    .Where(m => m.Name.Contains("a"))
    .ExecuteToListAsync() is var result && result.IsOk())
    Console.WriteLine(result.Data.ToJson());
else
    Console.WriteLine(result.ToErrorString());
```

### ExecuteToList with paging
result.Data = List&lt;Model&gt; here List&lt;Medicine&gt;
```csharp
if (await serviceClient.ModelRead<Medicine>()
    .Where(m => m.Name.Contains("a"))
    .Paging(page: 2, pageSize: 10)
    .ExecuteToListAsync() is var result && result.IsOk())
    Console.WriteLine(result.Data.ToJson());
else
    Console.WriteLine(result.ToErrorString());
```

### Select Id of Identifier
result.Data = long here the Id of Medicine
```csharp
if (await serviceClient.ModelRead<Medicine>()
    .Where(m => m.Identifier == "4711")
    .Select(m => m.Id)
    .ExecuteFirstOrDefaultAsync() is var result && result.IsOk())
    Console.WriteLine(result.Data.ToJson());
else
    Console.WriteLine(result.ToErrorString());
```
```sql
SELECT [Extent1].[Id] AS[Id]
    FROM[Medicine] AS[Extent1]
    WHERE '4711' = [Extent1].[Identifier]
```

### Select Id of Identifier (Tray)
result.Data = long here the Id of Tray
```csharp
if (await serviceClient.ModelRead<Tray>()
    .Where(m => m.Identifier == "4711")
    .Select(m => m.Id)
    .ExecuteFirstOrDefaultAsync() is var result && result.IsOk())
    Console.WriteLine(result.Data.ToJson());
else
    Console.WriteLine(result.ToErrorString());
```
```sql
SELECT [Extent1].[Id] AS[Id]
    FROM[Tray] AS[Extent1]
    WHERE '4711' = [Extent1].[VisibleIdentifier]
```

### select with new object
result.Data = a anonymous object with the properties of the select
```csharp
if (await serviceClient
    .Model<Medicine>().Read()
    .Where(m => m.Name.Contains("a"))
    .OrderByDescending(m => m.Id)
    .Select(m => new
    {
        MedicineName = m.Name,
        MedicineId = m.Id,
        MedicineIdentifier = m.Identifier,
        ManufacturerName = m.Manufacturer.Name
    })
    .ExecuteToListAsync() is var result && result.IsOk())
    Console.WriteLine(result.Data.ToJson());
else
    Console.WriteLine(result.ToErrorString());
```
This is slow because it needs 200ms to create a anonymous object on the server side.

### Enum does not work on the server side
It does not work for Where, OrderBy, Orderby, Select<br>
But on the result.
```csharp
if (await serviceClient.ModelRead<Patient>()
    .Where(p => p.Gender == Gender.Male)
    .OrderBy(p => p.Id)
    .ExecuteFirstOrDefaultAsync() is var result && result.IsOk())
    Console.WriteLine(result.Data.ToJson());
else
    Console.WriteLine(result.ToErrorString());
```
