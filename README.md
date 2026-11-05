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



