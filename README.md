# 🏭 FoodOps ERP - Food Manufacturing Suite

![.NET](https://img.shields.io/badge/.NET-10.0-512BD4?style=flat-square&logo=dotnet)
![EF Core](https://img.shields.io/badge/EF_Core-10.0-0078D4?style=flat-square)
![SQL Server](https://img.shields.io/badge/SQL_Server-2022-CC2927?style=flat-square&logo=microsoft-sql-server)
![Bootstrap](https://img.shields.io/badge/Bootstrap-5.3.2-7952B3?style=flat-square&logo=bootstrap)
![Vanilla JS](https://img.shields.io/badge/Vanilla_JS-SPA_Engine-F7DF1E?style=flat-square&logo=javascript)

A robust, enterprise-grade Food Manufacturing ERP system built using **ASP.NET Core MVC (.NET 10)** with **Razor Views (Server-Side Rendering)** on the backend, integrated with **EF Core 10 & SQL Server**. The frontend features a highly customized **Fluent Design SPA Dashboard** powered by **Vanilla JS, Bootstrap 5, and Custom CSS**. The system also integrates **Google Gemini AI** for real-time database querying and operational insights.

---

## 🏗️ Architecture Overview

The system is designed following **Clean Architecture** principles within a Single ASP.NET Core MVC project. This ensures a strict separation of concerns, decoupling the UI from the Business Logic and Data Access layers.

```mermaid
graph TD
    classDef ui fill:#d4edda,stroke:#28a745,stroke-width:2px;
    classDef logic fill:#cce5ff,stroke:#007bff,stroke-width:2px;
    classDef data fill:#f8d7da,stroke:#dc3545,stroke-width:2px;
    classDef models fill:#fff3cd,stroke:#ffc107,stroke-width:2px;

    subgraph Presentation Layer
        V[Razor Views .cshtml]:::ui
        JS[wwwroot/ - spa-router.js, ui.js]:::ui
        C[Controllers/]:::ui
    end

    subgraph Business Logic Layer
        S[Services/ - Inventory, AI, HR]:::logic
        I[Infrastructure/ - AppController, Middlewares]:::logic
    end

    subgraph Data Access Layer
        D[Data/ - ApplicationDbContext]:::data
        DB[(SQL Server Database)]:::data
    end

    subgraph Domain Models
        M[Models/ - Entities, ViewModels]:::models
    end

    V --> C
    JS --> C
    C --> S
    S --> D
    D --> DB
    S --> M
    C --> M
    D --> M
    I --> C



### 📂 Backend Project Breakdown

The backend is structured to maintain a clean separation of concerns using standard ASP.NET Core MVC conventions:

*   **`Controllers/`**: The entry point for all HTTP requests. It houses the MVC controllers, organized by domain (e.g., `AI/`, `Manufacturing/`, `SupplyChain/`, `Workforce/`). It handles routing, authorization (`[Authorize]`), and returns Razor Views or JSON responses.
*   **`Services/`**: The core Business Logic Layer. It contains the service classes (e.g., `AIAssistantService`, `InventoryService`) and their interfaces. This layer communicates with the `ApplicationDbContext` to process business rules.
*   **`Data/`**: The Data Access Layer. Contains the `ApplicationDbContext` which manages all Entity Framework Core configurations, relationships, and migrations.
*   **`Models/`**: The Domain Layer. Contains all database Entities (e.g., `Product`, `Employee`, `ProductionOrder`), Enums, and ViewModels used to transfer data securely to the Views.
*   **`Infrastructure/`**: Contains cross-cutting concerns such as the `AppController` base class (for accessing current user context), Custom HTML Helpers (`GridHtml`), and the `ModuleViewLocationExpander` for modular view routing.
*   **`Middlewares/`**: Contains the `GlobalExceptionHandlingMiddleware` that catches unhandled exceptions across the entire application and gracefully displays the custom error page.

### 🎨 Frontend Integration

The frontend is highly customized and built directly into the ASP.NET Core MVC pipeline, eliminating the need for heavy JavaScript frameworks (like React or Angular):

*   **Razor Views (`.cshtml`)**: The presentation layer uses Server-Side Rendering (SSR). Layouts (`_Layout.cshtml`), Partials (`_Sidebar.cshtml`, `_Topbar.cshtml`), and View Components are used extensively to build a modular UI.
*   **Custom CSS (`wwwroot/css/site.css`)**: A custom "Fluent Design" system built on top of **Bootstrap 5**, using CSS Variables (Design Tokens) for colors, spacing, and typography to achieve a premium Microsoft-like look.
*   **Vanilla JS SPA Engine (`wwwroot/js/`)**: We implemented a custom Single Page Application experience using pure JavaScript:
    *   `spa-router.js`: Intercepts link clicks and form submissions, fetches HTML fragments asynchronously, and updates the DOM without reloading the page.
    *   `ui.js`: Handles custom Modals, Toasts, Confirmation Dialogs, and dynamic table operations (search, sort, pagination).
    *   `shell.js`: Manages the collapsible Sidebar, active menu state, and global keyboard shortcuts (like `Ctrl+K`).
*   **AI Integration**: The frontend Chat interface communicates with the `AIAssistantController` via asynchronous `fetch` API calls, streaming real-time database insights from Google Gemini.
