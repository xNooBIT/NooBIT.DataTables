# NooBIT.DataTables

NooBIT.DataTables provides functionality to process ajax requests from https://datatables.net javascript framework v1.10.x

---

## Project Health

| Service | Status |
| --- | --- |
| AppVeyor | [![Build status](https://ci.appveyor.com/api/projects/status/vdu0r2ijk9xbsyev/branch/master?svg=true)](https://ci.appveyor.com/project/cmxl/noobit-datatables-6h5la/branch/master) |

## Packages

| Package | NuGet |
| --- | --- |
| NooBIT.DataTables | [![NuGet](https://buildstats.info/nuget/NooBIT.DataTables)](https://www.nuget.org/packages/NooBIT.DataTables/) |
| NooBIT.DataTables.AspNetCore.Mvc | [![NuGet](https://buildstats.info/nuget/NooBIT.DataTables.AspNetCore.Mvc)](https://www.nuget.org/packages/NooBIT.DataTables.AspNetCore.Mvc/) |

---

## Usage

Implement your custom table like this:

```csharp
    public class EmployeeTable : DataTable<Employee>
    {
        public EmployeeTable(IQueryableService<Employee> queryableService) : base(queryableService)
        {
        }
    }
```

You need some columnDefs for datatables options.
You can override the default:

```csharp
    public class EmployeeTable : DataTable<Employee>
    {
        // [...]

        protected override Column GetColumnTemplate(PropertyInfo x, int index)
        {
            return new Column(this)
            {
                Header = new Header {DisplayName = x.GetCustomAttribute<DisplayNameAttribute>()?.DisplayName ?? x.Name},
                Name = x.Name,
                Orderable = true,
                Orders = new[]
                {
                    new Column.Order
                    {
                        ColumnName = x.Name
                    }
                },
                Render = (o, item) => o,
                Searchable = true,
                Target = index
            };
        }
    }
```

Or if you want complete control over column generation:

```csharp
    public class EmployeeTable : DataTable<Employee>
    {
        // [...]
        protected override Column[] GetColumnsInternal() =>
            typeof(Employee).GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .OrderBy(x => x.GetCustomAttribute<ColumnOrderAttribute>()?.Order ?? int.MaxValue)
                .Select(GetColumnTemplate)
                .ToArray();
    }
```

_TODO language documentation_
