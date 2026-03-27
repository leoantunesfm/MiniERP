graph TD
    subgraph Frontend ["Front-end (Angular SPA)"]
        UI["Components (Tailwind CSS v4)"] -->|Data Binding| TS["TypeScript Logic (Reactive Forms)"]
        TS -->|Method Call| SV["Services (HttpClient)"]
        SV -->|Intercepts| INT["JWT Interceptor"]
    end

    subgraph Backend ["Back-end (.NET Web API)"]
        INT -->|REST / JSON + Bearer Token| API["API Layer (Controllers)"]
        
        API -->|DTOs| APP["Application Layer (Use Cases)"]
        
        APP -->|Business Rules| DOM["Domain Layer (Entities & Interfaces)"]
        
        APP -->|Interfaces| INFRA["Infrastructure Layer"]
        INFRA -.->|Implements| DOM
        
        INFRA -->|EF Core (ORM)| REPO["Repositories"]
        INFRA -->|Security| JWT["JwtTokenGenerator"]
    end

    subgraph Database ["Data Store (Docker)"]
        REPO -->|Npgsql| PG[("PostgreSQL DB (Multi-tenant)")]
    end