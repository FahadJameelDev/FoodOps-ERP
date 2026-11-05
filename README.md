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



