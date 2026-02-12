# 🧬 RPD_API --- Pokémon Database REST API

**RPD_API** is a scalable and modular **RESTful Pokémon Database API**
built with **ASP.NET Core 6**, **Entity Framework Core**, and
**AutoMapper**.\
It provides full CRUD operations over complex Pokémon domain data,
including **stats, abilities, types, moves, evolutions**, and game
versions, secured with **JWT authentication via Firebase**.

This project follows **clean architecture principles**, using
**Repository, Unit of Work, and Service layers** to ensure
maintainability, testability, and scalability.

------------------------------------------------------------------------

## 🚀 Key Features

-   **Full Pokémon CRUD**
    -   Includes Stats, Abilities, Types, Moves, Egg Groups, Game
        Versions, Images, and Effort Values
-   **Complex Relationships**
    -   Many-to-many mappings (Pokémon ↔ Abilities, Types, Moves, etc.)
    -   Evolution chains with self-referencing relationships
-   **Authentication & Security**
    -   JWT authentication integrated with **Firebase**
    -   Google Sign-In support
-   **Clean Architecture**
    -   Repository pattern
    -   Unit of Work pattern
    -   Application Service orchestration layer
-   **DTO-Driven API**
    -   AutoMapper for safe entity-to-DTO transformation
    -   Separate DTOs for Create / Update / Read
-   **Transaction-Safe Operations**
    -   Cascade delete handling
    -   Atomic create/update flows
-   **Extensible & Maintainable**
    -   Clear separation of concerns
    -   Designed for future feature expansion

------------------------------------------------------------------------

## 🧩 Technology Stack

-   **Framework:** ASP.NET Core 6
-   **ORM:** Entity Framework Core
-   **Database:** SQL Server
-   **Mapping:** AutoMapper
-   **Authentication:** JWT + Firebase
-   **Role-based authorization** Admin + Trainer
-   **Architecture Patterns:**
    -   Repository
    -   Unit of Work
    -   Service & Application Service Layers
-   **Pagination & filtering**
-   **Caching** : Redis + Docker
-   **File Upload Input (.cvs)**
------------------------------------------------------------------------

## 🏗 Architecture Overview

    Controller
       ↓
    Application Service
       ↓
    Domain Services
       ↓
    Repositories
       ↓
    Entity Framework Core
       ↓
    SQL Server

------------------------------------------------------------------------

## 🔐 Authentication

-   Firebase Authentication integration
-   JWT token generation & validation
-   Google OAuth login support

------------------------------------------------------------------------

## 📦 Domain Model Highlights

-   Pokémon
-   Stats & Effort Values
-   Abilities
-   Types
-   Moves
-   Egg Groups
-   Game Versions
-   Evolution Chains
-   Image Links

All relationships are handled using **EF Core Fluent API** with proper
cascade and restrict delete behaviors.

------------------------------------------------------------------------

## 📌 Future Improvements
-   API versioning
-   Swagger / OpenAPI enhancements
-   Unit & integration testing

------------------------------------------------------------------------

## 📄 License

This project is for educational and portfolio purposes.
