# 🏭 FoodOps ERP - Food Manufacturing Suite

![.NET](https://img.shields.io/badge/.NET-10.0-512BD4?style=flat-square&logo=dotnet)
![EF Core](https://img.shields.io/badge/EF_Core-10.0-0078D4?style=flat-square)
![SQL Server](https://img.shields.io/badge/SQL_Server-2022-CC2927?style=flat-square&logo=microsoft-sql-server)
![Bootstrap](https://img.shields.io/badge/Bootstrap-5.3.2-7952B3?style=flat-square&logo=bootstrap)
![Vanilla JS](https://img.shields.io/badge/Vanilla_JS-SPA_Engine-F7DF1E?style=flat-square&logo=javascript)

A robust, enterprise-grade Food Manufacturing ERP system built using **ASP.NET Core MVC (.NET 10)** with **Razor Views (Server-Side Rendering)** on the backend, integrated with **EF Core 10 & SQL Server**. The frontend features a highly customized **Fluent Design SPA Dashboard** powered by **Vanilla JS, Bootstrap 5, and Custom CSS**. The system also integrates **Google Gemini AI** for real-time database querying and operational insights.

---
### 📂 Backend Project Breakdown

The backend is built as a **Single ASP.NET Core MVC Project** with strict folder-level separation of concerns. This eliminates cross-project dependency issues while maintaining Clean Architecture principles.

*   **`Controllers/` (Presentation):** Handles HTTP requests, routing, and returns Razor Views. Organized into sub-folders like `Executive`, `Manufacturing`, `SupplyChain`, etc., for modular routing.
*   **`Services/` (Business Logic):** The core engine of the ERP. Contains interfaces and implementations for Inventory, Production, HR, and **AI Integration**. Decoupled from the database.
*   **`Data/` (Data Access):** Houses the `ApplicationDbContext` and EF Core configurations. Handles all database migrations and relationships.
*   **`Models/` (Domain Entities):** Contains database Entities (Database tables) and ViewModels (Data transfer objects for the UI).
*   **`Infrastructure/`:** Contains cross-cutting concerns like `AppController` (Base controller), `GridHtml.cs` (Custom Razor helpers for dynamic tables), and custom View Location Expanders.
*   **`Middlewares/`:** Custom Global Exception Handling Middleware to catch and log unhandled errors gracefully.

---

### 🎨 Frontend Integration

The frontend is not a separate SPA project (like React or Angular), but rather a highly customized **Server-Side Rendered (SSR) Application** using **Razor Views (.cshtml)** and **Vanilla JavaScript**.

*   **Razor Views:** The UI is rendered on the server and delivered as complete HTML. However, our custom `spa-router.js` intercepts link clicks and fetches only the inner HTML, providing a seamless Single Page Application (SPA) feel without any full-page reloads.
*   **Custom UI Engine (`wwwroot/`):** 
    *   `site.css`: Contains our custom Fluent Design System tokens, overriding standard Bootstrap.
    *   `ui.js` / `shell.js`: Handles interactive elements like dynamic modals, toasts, grid searching/sorting, and sidebar state management.
*   **Zero-Build Step:** Unlike React (which requires Webpack/Vite build steps), our frontend runs directly from the `wwwroot` directory, making deployment incredibly fast and lightweight.




---

## 🗄️ Database Entity-Relationship (ER) Diagrams

To keep the diagrams readable and comprehensive, the database model is categorized into logical modules. These diagrams are written in **Mermaid.js** syntax and render natively.

### 1. Master Data & Catalog (Core)
Tracks the basic master records that populate the rest of the business documents.

```mermaid
erDiagram
    Plants ||--o{ Users : "has"
    Plants ||--o{ Warehouses : "has"
    Categories ||--o{ Products : "contains"
    UnitsOfMeasure ||--o{ Products : "measures"
    Products ||--o{ PackagingConfigs : "has"
    
    Plants {
        int PlantID PK
        string PlantName
        string Location
    }
    Products {
        int ProductID PK
        string ProductName
        string ProductCode
        int CategoryID FK
    }
    Categories {
        int CategoryID PK
        string CategoryName
    }
```

### 2. Supply Chain & Inventory (Procurement)
Manages trade transactions with Suppliers and tracks real-time stock batches.

```mermaid
erDiagram
    Suppliers ||--o{ PurchaseOrders : "receives"
    Plants ||--o{ PurchaseOrders : "requests"
    PurchaseOrders ||--|{ PurchaseOrderItems : "contains"
    Products ||--o{ PurchaseOrderItems : "ordered in"
    
    Products ||--o{ InventoryBatches : "stored as"
    Suppliers ||--o{ InventoryBatches : "supplied"
    WarehouseBins ||--o{ InventoryBatches : "located at"
    InventoryBatches ||--o{ InventoryTransactions : "logs"
    InventoryBatches ||--o{ StockAdjustments : "adjusts"

    InventoryBatches {
        bigint BatchID PK
        string BatchNumber
        decimal CurrentQuantity
        string QCStatus
        datetime ExpiryDate
    }
```

### 3. Manufacturing & Quality Control
Handles Recipes (BOM), Production Orders, and strict Quality Inspections.

```mermaid
erDiagram
    Products ||--o{ Recipes : "has BOM"
    Recipes ||--|{ RecipeItems : "contains"
    
    Demands ||--o{ ProductionPlanLines : "triggers"
    ProductionPlans ||--|{ ProductionPlanLines : "contains"
    ProductionPlanLines ||--o{ ProductionOrders : "creates"
    
    ProductionOrders ||--o{ BatchMaterialIssues : "consumes"
    ProductionOrders ||--o{ BatchOutputLogs : "produces"
    
    InventoryBatches ||--o{ QCInspections : "inspected in"
    QCInspections ||--o{ QCInspectionResults : "has"
    QCInspections ||--o{ NonConformanceReports : "fails as"
```

### 4. Workforce & AI Engine
Manages HR, Payroll, and logs AI interactions.

```mermaid
erDiagram
    Departments ||--o{ Employees : "has"
    Shifts ||--o{ Employees : "assigned to"
    Employees ||--o{ AttendanceLogs : "logs"
    Employees ||--o{ PayrollLines : "gets paid via"
    PayrollRuns ||--|{ PayrollLines : "contains"
    
    Users ||--o{ AIAssistantLogs : "asks"
    
    Employees {
        int EmployeeID PK
        string FirstName
        string EmployeeCode
        string EmploymentStatus
    }
    AIAssistantLogs {
        bigint AILogID PK
        string DomainContext
        string QuestionSummary
        datetime CreatedAt
    }
```

---

## 🛡️ Security, Policies, and Audit Trails

### Authentication & Authorization
The application is secured using **Cookie-Based Authentication** and strict **Role-Based Access Control (RBAC)** at the Controller level.

*   **RBAC Enforcement:** Controllers are decorated with `[Authorize(Roles="...")]`. For example, only users with the `HR Manager` or `Administrator` role can access the `Workforce` module.
*   **Anti-Forgery Protection:** All POST requests (Forms/AJAX) are protected using `@Html.AntiForgeryToken()` and verified via the `RequestVerificationToken` header to prevent CSRF attacks.

### Automated Auditing Mechanism
The system features an automated, transactional auditing design at the database level using EF Core:

*   **Audit Columns:** Almost all critical tables contain `CreatedAt`, `CreatedByUserID`, `ModifiedAt`, and `ModifiedByUserID` columns to track *who* did *what* and *when*.
*   **Concurrency Control:** `RowVersion` (Timestamp) is applied on all tables to prevent data overwrites during simultaneous user edits.
*   **AI Audit Logs:** Every AI interaction is tracked in the `AIAssistantLogs` table, storing the `DomainContext`, `QuestionSummary`, and `ExecutionTimeMs` for performance and usage monitoring.
*   **Append-Only Logs:** Tables like `ProductionStatusHistory` and `InventoryTransactions` are designed as immutable logs (no updates or deletes allowed) for strict traceability.
```



