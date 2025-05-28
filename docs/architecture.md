# Architecture Documentation

## System Overview

The Corner Shop Management System follows a 3-tier architecture with dual database support:

1. **Presentation Layer**
   - Handles user interface and input/output
   - Located in `Program.cs`
   - Manages user interactions and display formatting
   - Provides clear menu navigation and feedback

2. **Business Layer**
   - Contains business logic and rules
   - Located in `Services/` directory
   - Handles data validation and business operations
   - Key components:
     - `IProductService` and `ProductService`: Product-related operations
     - `ISaleService` and `SaleService`: Sale-related operations
     - `ISyncService` and `SyncService`: Database synchronization

3. **Data Layer**
   - Manages data persistence
   - Supports two database implementations:
     - MongoDB: Document-based storage
     - Entity Framework Core: Relational storage with SQLite
   - Implements `IDatabaseService` interface
   - Handles data synchronization between databases

## Database Architecture

### MongoDB Implementation
- Document-based storage
- Collections for Products and Sales
- BSON serialization for data storage
- ObjectId for document identification

### Entity Framework Core Implementation
- Relational storage using SQLite
- Entity models with data annotations
- Automatic database creation and migration
- LINQ queries for data access

### Database Synchronization
- Bidirectional sync between MongoDB and EF Core
- Automatic sync after critical operations
- Manual sync option
- Conflict resolution strategy

## Project Structure

```
CornerShop/
├── Models/
│   ├── Product.cs
│   ├── Sale.cs
│   └── SaleItem.cs
├── Services/
│   ├── IDatabaseService.cs
│   ├── MongoDatabaseService.cs
│   ├── EfDatabaseService.cs
│   ├── IProductService.cs
│   ├── ProductService.cs
│   ├── ISaleService.cs
│   ├── SaleService.cs
│   ├── ISyncService.cs
│   └── SyncService.cs
└── Program.cs
```

## Key Components

### Models
- `Product`: Represents a product with properties like name, price, and stock
- `Sale`: Represents a sale transaction with items and total
- `SaleItem`: Represents an item in a sale with product and quantity

### Services
- `IDatabaseService`: Interface for database operations
- `MongoDatabaseService`: MongoDB implementation
- `EfDatabaseService`: Entity Framework Core implementation
- `IProductService`: Interface for product operations
- `ProductService`: Product business logic
- `ISaleService`: Interface for sale operations
- `SaleService`: Sale business logic
- `ISyncService`: Interface for database synchronization
- `SyncService`: Synchronization logic

## Data Flow

1. **User Input**
   - User interacts with the console interface
   - Input validation and error handling

2. **Business Logic**
   - Service layer processes requests
   - Business rules enforcement
   - Data validation

3. **Data Persistence**
   - Primary database operation
   - Automatic synchronization
   - Error handling and recovery

4. **User Feedback**
   - Operation results
   - Error messages
   - Confirmation prompts 