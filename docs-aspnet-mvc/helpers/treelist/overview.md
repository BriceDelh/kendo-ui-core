---
title: Overview
page_title: TreeList | Telerik UI for ASP.NET MVC HtmlHelpers
description: "Get started with the server-side wrapper for the Kendo UI TreeList widget for ASP.NET MVC."
slug: overview_treelisthelper_aspnetmvc
position: 1
---

# TreeList HtmlHelper Overview

The TreeList HtmlHelper extension is a server-side wrapper for the [Kendo UI TreeList](https://demos.telerik.com/kendo-ui/treelist/index) widget. It allows you to configure the Kendo UI TreeList from server-side code and helps you bind it to data and edit it.

## Getting Started

### Configuration

Below are listed the steps for you to follow when configuring the Kendo UI TreeList to do server binding to the Northwind database, the **Employees** table.

1. Create a new ASP.NET MVC 4 application. If you have installed the [Telerik UI for ASP.NET MVC Visual Studio Extensions]({% slug overview_aspnetmvc %}#requirements), create a Telerik UI for ASP.NET MVC application. Name the application `KendoGridServerBinding`. If you decided not to use the Telerik UI for ASP.NET MVC Visual Studio Extensions, follow the steps from the [introductory article]({% slug overview_aspnetmvc %}) to add Telerik UI for ASP.NET MVC to the application.

1. Add a new `Entity Framework Data Model`. Right-click the `~/Models` folder in the solution explorer and pick **Add new item**. Choose **Data** > **ADO.NET Entity Data Model** in the **Add New Item** dialog. Name the model `Northwind.edmx` and click **Next**. This starts the **Entity Data Model Wizard**.

    **Figure 1. A new entity model**

    ![New entity data model](images/treelist-new-entity-data-model.png)

1. Pick the **Generate from database** option and click **Next**. Configure a connection to the Northwind database. Click **Next**.

    **Figure 2. Choose the connection**

    ![Choose the connection](images/treelist-entity-data-model.png)

1. Choose the **Employees** table from the `Which database objects do you want to include in your model?`. Leave all other options as they are set by default. Click **Finish**.

    **Figure 3. Choose the Employees table**

    ![Choose the Employees table](images/treelist-database-objects.png)

1. Right-click the `~/Models` folder in the solution explorer and add a new `EmployeeViewModel` class.

    ###### Example

        public class EmployeeViewModel
        {
            public int? EmployeeID { get; set; }

            public string FirstName { get; set; }

            public string LastName { get; set; }

            public int? ReportsTo { get; set; }

            public string Address { get; set; }

            public bool hasChildren { get; set; }
        }

1. Open `HomeController.cs` and create new `TreeList_Read` action method.

    ###### Example

        public JsonResult TreeList_Read([DataSourceRequest] DataSourceRequest request)
        {
            var northwind = new NortwindEntities();

            var result = northwind.Employees.ToTreeDataSourceResult(request,
                employee => employee.EmployeeID,
                employee => employee.ReportsTo,
                employee => new EmployeeViewModel
                {
                    EmployeeID = employee.EmployeeID,
                    ReportsTo = employee.ReportsTo,
                    FirstName = employee.FirstName,
                    LastName = employee.LastName,
                    Address = employee.Address,
                    hasChildren = employee.Employees1.Any()
                }
            );

            return Json(result, JsonRequestBehavior.AllowGet);
        }

    > **Important**
    >
    > As of the Kendo UI R1 2017 SP1 release, you can use the `ToTreeDataSourceResultAsync` extension method to provide the asynchronous functionality of `ToTreeDataSourceResult` by leveraging the `async` and `await` features of the .NET Framework.

    The following example demonstrates how to implement the `ToTreeDataSourceResultAsync` extension method in your project.

    ###### Example

        public async Task<JsonResult> TreeList_Read([DataSourceRequest] DataSourceRequest request)
        {
            var northwind = new NortwindEntities();

            var result = await northwind.Employees.ToTreeDataSourceResultAsync(request,
                employee => employee.EmployeeID,
                employee => employee.ReportsTo,
                employee => new EmployeeViewModel
                {
                    EmployeeID = employee.EmployeeID,
                    ReportsTo = employee.ReportsTo,
                    FirstName = employee.FirstName,
                    LastName = employee.LastName,
                    Address = employee.Address,
                    hasChildren = employee.Employees1.Any()
                }
            );

            return Json(result, JsonRequestBehavior.AllowGet);
        }

1. Add a Kendo UI TreeList to the `Index` view.

    ```ASPX
        <%:Html.Kendo().TreeList<KendoTreeListBinding.Models.EmployeeViewModel>()
            .Name("treelist")
            .Columns(columns =>
                {
                columns.Add().Field(e => e.FirstName).Width(220);
                columns.Add().Field(e => e.LastName).Width(160);
                columns.Add().Field(e => e.Address);
                })
            .Sortable()
            .DataSource(dataSource => dataSource
                .Read(read => read.Action("TreeList_Read", "Home"))
                .Model(m => {
                    m.Id(f => f.EmployeeID);
                    m.ParentId(f => f.ReportsTo);
                    })
                )
            .Height(540)
        %>
    ```
    ```Razor
        @(Html.Kendo().TreeList<KendoTreeListBinding.Models.EmployeeViewModel>()
            .Name("treelist")
            .Columns(columns =>
                {
                columns.Add().Field(e => e.FirstName).Width(220);
                columns.Add().Field(e => e.LastName).Width(160);
                columns.Add().Field(e => e.Address);
                })
            .Sortable()
            .DataSource(dataSource => dataSource
                .Read(read => read.Action("TreeList_Read", "Home"))
                .Model(m => {
                    m.Id(f => f.EmployeeID);
                    m.ParentId(f => f.ReportsTo);
                    })
                )
            .Height(540)
        )
    ```

1. Build and run the application.

    **Figure 4. The final result**

    ![Final result](images/treelist-bound.png)

## Event Handling

You can subscribe to all TreeList [events](http://docs.telerik.com/kendo-ui/api/javascript/ui/treelist#events).

### By Handler Name

The following example demonstrates how to subscribe to events by a handler name.

```ASPX
    <%:Html.Kendo().TreeList<KendoTreeListBinding.Models.EmployeeViewModel>()
        .Name("treelist")
        .Columns(columns =>
            {
            columns.Add().Field(e => e.FirstName).Width(220);
            columns.Add().Field(e => e.LastName).Width(160);
            columns.Add().Field(e => e.Address);
            })
        .Sortable()
        .DataSource(dataSource => dataSource
            .Read(read => read.Action("TreeList_Read", "Home"))
            .Model(m => {
                m.Id(f => f.EmployeeID);
                m.ParentId(f => f.ReportsTo);
                })
            )
        .Height(540)
        .Events(e => e
            .DataBound("treelist_dataBound")
            .Change("treelist_change")
        )
    %>
    <script>
        function treelist_dataBound() {
            //Handle the dataBound event.
        }

        function treelist_change() {
            //Handle the change event.
        }
    </script>
```
```Razor
    @(Html.Kendo().TreeList<KendoTreeListBinding.Models.EmployeeViewModel>()
        .Name("treelist")
        .Columns(columns =>
            {
            columns.Add().Field(e => e.FirstName).Width(220);
            columns.Add().Field(e => e.LastName).Width(160);
            columns.Add().Field(e => e.Address);
            })
        .Sortable()
        .DataSource(dataSource => dataSource
            .Read(read => read.Action("TreeList_Read", "Home"))
            .Model(m => {
                m.Id(f => f.EmployeeID);
                m.ParentId(f => f.ReportsTo);
                })
            )
        .Height(540)
        .Events(e => e
            .DataBound("treelist_dataBound")
            .Change("treelist_change")
        )
    )
    <script>
        function treelist_dataBound() {
            //Handle the dataBound event.
        }

        function treelist_change() {
            //Handle the change event.
        }
    </script>
```

### By Template Delegate

The following example demonstrates how to subscribe to events by a template delegate.

###### Example

    @(Html.Kendo().TreeList<KendoTreeListBinding.Models.EmployeeViewModel>()
        .Name("treelist")
        .Columns(columns =>
            {
            columns.Add().Field(e => e.FirstName).Width(220);
            columns.Add().Field(e => e.LastName).Width(160);
            columns.Add().Field(e => e.Address);
            })
        .Sortable()
        .DataSource(dataSource => dataSource
            .Read(read => read.Action("TreeList_Read", "Home"))
            .Model(m => {
                m.Id(f => f.EmployeeID);
                m.ParentId(f => f.ReportsTo);
            })
        )
        .Events(e => e
            .DataBound(@<text>
                function() {
                    //Handle the dataBound event inline.
                }
            </text>)
            .Change(@<text>
                function() {
                    //Handle the change event inline.
                }
            </text>)
        )
        .Height(540)
    )

## Reference

### Existing Instances

To reference an existing Kendo UI TreeList instance, use the [`jQuery.data()`](http://api.jquery.com/jQuery.data/) configuration option. Once a reference is established, use the [TreeList API](http://docs.telerik.com/kendo-ui/api/javascript/ui/treelist#methods) to control its behavior.

###### Example

    @(Html.Kendo().TreeList<KendoTreeListBinding.Models.EmployeeViewModel>()
        .Name("treelist")
        .Columns(columns =>
            {
            columns.Add().Field(e => e.FirstName).Width(220);
            columns.Add().Field(e => e.LastName).Width(160);
            columns.Add().Field(e => e.Address);
            })
        .Sortable()
        .DataSource(dataSource => dataSource
            .Read(read => read.Action("TreeList_Read", "Home"))
            .Model(m => {
                m.Id(f => f.EmployeeID);
                m.ParentId(f => f.ReportsTo);
                })
            )
        .Height(540)
    )
    <script>
        $(function() {
            //Notice that the Name() of the TreeList is used to get its client-side instance.
            var treelist = $("#treelist").data("kendoTreeList");
        });
    </script>

## See Also

* [Telerik UI for ASP.NET MVC API Reference: TreeListBuilder](http://docs.telerik.com/aspnet-mvc/api/Kendo.Mvc.UI.Fluent/TreeListBuilder)
* [How to Export Multiple TreeLists to Excel]({% slug howto_exportmultipletoexcel_treelistaspnetmvc %})
* [Overview of the Kendo UI TreeList Widget](http://docs.telerik.com/kendo-ui/controls/data-management/treelist/overview)
* [Overview of Telerik UI for ASP.NET MVC]({% slug overview_aspnetmvc %})
* [Fundamentals of Telerik UI for ASP.NET MVC]({% slug fundamentals_aspnetmvc %})
* [Scaffolding in Telerik UI for ASP.NET MVC]({% slug scaffolding_aspnetmvc %})
* [Telerik UI for ASP.NET MVC API Reference Folder](http://docs.telerik.com/aspnet-mvc/api/Kendo.Mvc/AggregateFunction)
* [Telerik UI for ASP.NET MVC HtmlHelpers Folder]({% slug overview_barcodehelper_aspnetmvc %})
* [Tutorials on Telerik UI for ASP.NET MVC]({% slug overview_timeefficiencyapp_aspnetmvc6 %})
* [Telerik UI for ASP.NET MVC Troubleshooting]({% slug troubleshooting_aspnetmvc %})
