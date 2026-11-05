# 🏭 FoodOps ERP - Food Manufacturing Suite

![.NET](https://img.shields.io/badge/.NET-10.0-512BD4?style=flat-square&logo=dotnet)
![EF Core](https://img.shields.io/badge/EF_Core-10.0-0078D4?style=flat-square)
![SQL Server](https://img.shields.io/badge/SQL_Server-2022-CC2927?style=flat-square&logo=microsoft-sql-server)
![Bootstrap](https://img.shields.io/badge/Bootstrap-5.3.2-7952B3?style=flat-square&logo=bootstrap)
![Vanilla JS](https://img.shields.io/badge/Vanilla_JS-SPA_Engine-F7DF1E?style=flat-square&logo=javascript)

A robust, enterprise-grade Food Manufacturing ERP system built using **ASP.NET Core MVC (.NET 10)** with **Razor Views (Server-Side Rendering)** on the backend, integrated with **EF Core 10 & SQL Server**. The frontend features a highly customized **Fluent Design SPA Dashboard** powered by **Vanilla JS, Bootstrap 5, and Custom CSS**. The system also integrates **Google Gemini AI** for real-time database querying and operational insights.

---

## 🏗️ Architecture Overview

The system is designed following **Clean Architecture** principles within a single ASP.NET Core MVC project, enforcing a strict separation of concerns. The architecture cleanly decouples the UI, Business Logic, and Data Access layers.

```mermaid
flowchart TD
    %% Custom Styling for Premium Look
    classDef layer fill:#f8fafc,stroke:#cbd5e1,stroke-width:2px,color:#0f172a,rx:8,ry:8;
    classDef highlight fill:#10b981,stroke:#059669,stroke-width:2px,color:#ffffff,rx:8,ry:8;
    classDef db fill:#3b82f6,stroke:#2563eb,stroke-width:2px,color:#ffffff,rx:8,ry:8;

    %% Nodes
    Client["🌐 Client Browser<br/>(HTML, CSS, Vanilla JS)"]
    UI["🖥️ Presentation Layer<br/>(Razor Views & SPA Router)"]:::layer
    Controllers["🎮 Controllers<br/>(MVC Endpoints & APIs)"]:::layer
    Services["⚙️ Business Logic Layer<br/>(Services & AI Assistant)"]:::highlight
    Data["🗄️ Data Access Layer<br/>(EF Core & DbContext)"]:::layer
    SQL["💾 SQL Server Database"]:::db

    %% Connections
    Client -->|HTTP Request| UI
    UI --> Controllers
    Controllers -->|Method Calls| Services
    Services -->|Entity Operations| Data
    Data -->|Queries| SQL
    SQL -.->|Returns Data| Data
    Data -.->|Objects| Services
    Services -.->|ViewModels| Controllers
    Controllers -.->|Rendered HTML| UI



* **Presentation Layer:** Razor Views and Custom SPA Router (`spa-router.js`) for seamless UI.
* **Business Logic:** Service classes (`Services/`) handle domain rules and AI integration (`AIAssistantService`).
* **Data Access:** Entity Framework Core (`Data/`) manages SQL Server migrations and DbContext.
* **Infrastructure:** Custom middlewares (`GlobalExceptionHandlingMiddleware`) and helper classes (`GridHtml.cs`).
