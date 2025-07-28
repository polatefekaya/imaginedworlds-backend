# Imagined Worlds - Backend

This is the backend for Imagined Worlds. It's not just an API; it's a powerful, AI-native engine designed to interpret human imagination and orchestrate the creation of a digital world. The architecture is built on modern .NET 9 using enterprise-grade patterns for robustness, scalability, and maintainability.

---
## Key Features & Architecture

The backend is engineered to be a resilient, high-performance, and truly intelligent system.

### 1. **AI-Native "Coordinator/Executor" Loop**
This is the core of the project. The system doesn't just *use* an AI; its entire logic *is* an AI feedback loop.
* **The Architector:** It's an architector, it makes the first call and creates the plan and stages.
* **The Coordinator:** A coordinator :D, it has memory, it manages the focuser and the executor. It gives abilities to focuser and executor. 
* **The Focuser:** A strategic AI call analyzes a low-resolution, priority-based summary of the world to decide *where* to focus next. This is a highly efficient way to make intelligent, high-level decisions without processing massive amounts of data.
* **The Executor:** A tactical AI call takes the Coordinator's decision, zooms into a high-resolution 10x10 area, and generates the specific, pixel-level actions to advance the construction plan.
This two-tier system mimics human creativity—alternating between big-picture strategy and focused, detailed work.

### 2. **Clean, Decoupled Architecture (DDD & CQRS)**
The system is built on a strict separation of concerns, ensuring it's testable and easy to reason about.
* **Domain-Driven Design (DDD):** Core business logic and entities (like the `Agent`) are pure and isolated in the Domain layer, with no knowledge of databases or APIs.
* **CQRS with Mediator:** We use `Mediator.SourceGenerator` for a lightning-fast, reflection-free implementation of the Command Query Responsibility Segregation pattern. "Write" operations (Commands) are fully separated from "read" operations (Queries), leading to a cleaner, more scalable codebase.
* **Unit of Work Pattern:** Guarantees that all database changes within a single operation are atomic. The command handler controls the transaction, ensuring data consistency without being tightly coupled to EF Core.

### 3. **Resilient & Asynchronous by Design**
The world generation is a long-running task. The architecture is built to handle this gracefully.
* **CancellationToken Propagation:** Cancellation tokens are passed through the entire call stack, from the initial API request down to the AI calls. This allows us to instantly kill a simulation if a user disconnects or cancels, saving server resources.
* **Robust Error Handling:** The core command handlers use `try/catch/finally` blocks to ensure that even if the AI fails or an exception is thrown, the client is notified via SignalR and server resources (like the `SimulationManager`) are properly cleaned up.

### 4. **High-Performance & Modern Stack**
* **.NET 9 Minimal APIs:** Provides a low-ceremony, high-performance foundation for our web endpoints.
* **Entity Framework Core & SQLite:** A fast, file-based database perfect for a hackathon, with production-ready entity configurations (like `jsonb` mapping for PostgreSQL).
* **SignalR:** The real-time communication backbone, pushing live updates to the frontend with minimal latency.

---
## Getting Started

1.  **Install Dependencies:**
    ```bash
    dotnet restore
    ```
2.  **Configure Secrets:**
    Open `Properties/launchSettings.json`. Under the `https` profile's `environmentVariables` section, add your Gemini API key:
    ```json
    "Gemini-Prod-Api-Key": "YOUR_GEMINI_API_KEY_HERE"
    ```
3.  **Setup Database:**
    Run the EF Core migrations to create the local `imaginedworlds.db` SQLite database.
    ```bash
    dotnet ef database update
    ```
4.  **Run the Application:**
    ```bash
    dotnet run --launch-profile https
    ```
    The API will be available at `https://localhost:7236`.