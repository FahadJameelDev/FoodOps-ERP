# 🏭 FoodOps ERP - Food Manufacturing Suite

![.NET](https://img.shields.io/badge/.NET-10.0-512BD4?style=flat-square&logo=dotnet)
![EF Core](https://img.shields.io/badge/EF_Core-10.0-0078D4?style=flat-square)
![SQL Server](https://img.shields.io/badge/SQL_Server-2022-CC2927?style=flat-square&logo=microsoft-sql-server)
![Bootstrap](https://img.shields.io/badge/Bootstrap-5.3.2-7952B3?style=flat-square&logo=bootstrap)
![Vanilla JS](https://img.shields.io/badge/Vanilla_JS-SPA_Engine-F7DF1E?style=flat-square&logo=javascript)

A robust, enterprise-grade Food Manufacturing ERP system built using **ASP.NET Core MVC (.NET 10)** with **Razor Views (Server-Side Rendering)** on the backend, integrated with **EF Core 10 & SQL Server**. The frontend features a highly customized **Fluent Design SPA Dashboard** powered by **Vanilla JS, Bootstrap 5, and Custom CSS**. The system also integrates **Google Gemini AI** for real-time database querying and operational insights.

---

## 🏗️ Architecture Overview

The system is designed following **Clean Architecture** principles within a single ASP.NET Core MVC project, enforcing a strict separation of concerns. The architecture decouples the UI, Business Logic, and Data Access layers.

```mermaid
graph TD
    %% Defining the styles for the boxes to look premium
    classDef presentation fill:#0ea5e9,stroke:#0284c7,stroke-width:2px,color:#fff,rx:5,ry:5;
    classDef business fill:#10b981,stroke:#059669,stroke-width:2px,color:#fff,rx:5,ry:5;
    classDef data fill:#8b5cf6,stroke:#7c3aed,stroke-width:2px,color:#fff,rx:5,ry:5;
    classDef domain fill:#f59e0b,stroke:#d97706,stroke-width:2px,color:#fff,rx:5,ry:5;
    classDef infra fill:#64748b,stroke:#475569,stroke-width:2px,color:#fff,rx:5,ry:5;

    %% Nodes
    UI["Presentation Layer<br/>(Views / Razor / SPA)"]:::presentation
    WebRoot["wwwroot<br/>(CSS, Vanilla JS, Bootstrap)"]:::infra
    Controllers["Controllers<br/>(API & MVC Endpoints)"]:::presentation
    
    Services["Services Layer<br/>(Business Logic & AI)"]:::business
    
    Data["Data Access Layer<br/>(ApplicationDbContext / EF Core)"]:::data
    
    Models["Domain Models<br/>(Entities & ViewModels)"]:::domain

    %% Connections
    UI --> Controllers
    WebRoot -.-> UI
    Controllers --> Services
    Services --> Data
    Data --> Models
    Services --> Models
*   **Presentation Layer:** Razor Views and Custom SPA Router (`spa-router.js`) for seamless UI.
*   **Business Logic:** Service classes (`Services/`) handle domain rules and AI integration (`AIAssistantService`).
*   **Data Access:** Entity Framework Core (`Data/`) manages SQL Server migrations and DbContext.
*   **Infrastructure:** Custom middlewares (`GlobalExceptionHandlingMiddleware`) and helper classes (`GridHtml.cs`).
