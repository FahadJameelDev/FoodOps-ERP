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
