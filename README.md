# Schema Editor Core - Blazor Server Application

A fully functional .NET 8 Blazor Server application for managing complex database schema design with hierarchical tables, slots (fields), choices, and relationships.

## Features

✅ **Hierarchical Schema Tree** - Expandable/collapsible domain and table tree structure
✅ **Slots Management** - Full CRUD operations on table fields/columns  
✅ **Choices/Fixed Values** - Manage predefined choice values for fields  
✅ **References** - View table relationships and foreign keys  
✅ **Denormalization** - Convert relationships to denormalized fields (String, Numeric, Choice)  
✅ **Context Menus** - Right-click operations throughout the interface  
✅ **Data Grids** - Responsive tabular views with row selection  
✅ **Tab-based Interface** - Organized Slots and References tabs  
✅ **Responsive Design** - Desktop, tablet, and mobile support  

## Project Structure

```
SchemaEditorCore/
├── Models/
│   └── SchemaModels.cs                 # Domain models (Domain, Table, Slot, Choice, etc.)
├── Services/
│   └── SchemaDataService.cs            # In-memory data management service
├── Pages/
│   └── ApplicationSchema.razor          # Main editor page with full UI
├── Components/
│   ├── TreeNodeComponent.razor         # Hierarchical tree node
│   └── DataGridComponent.razor         # Reusable data grid
├── Layouts/
│   └── MainLayout.razor                # Main layout wrapper
├── Styles/
│   └── ApplicationSchema.css            # Complete styling (1000+ lines)
├── App.razor                           # Root Blazor component
├── Program.cs                          # Startup configuration
├── _Host.cshtml                        # Host HTML page
├── SchemaEditorCore.csproj             # Project file
└── appsettings.json                    # Configuration
```

## Installation

### Prerequisites
- .NET 8.0 SDK or later
- Visual Studio 2022 or Visual Studio Code

### Setup
```bash
cd SchemaEditorCore
dotnet restore
dotnet watch run
# Navigate to https://localhost:5001
```

## Usage

1. **Browse Schema** - Expand/collapse tree nodes to explore domains and tables
2. **View Slots** - Click on a table to see its slots in the grid
3. **Manage Choices** - Select a slot with choices, add/remove choice values
4. **View References** - Click References tab to see table relationships
5. **Edit Operations** - Right-click for context menus on any element
6. **Denormalize** - Right-click table → De-Normalize to convert relationships

## License
Proprietary - Schema Editor Core
