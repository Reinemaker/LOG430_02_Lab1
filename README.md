# Corner Shop Management System

A simple console application for managing a corner shop's inventory and sales using MongoDB.

## Architecture

The application follows a 2-tier architecture:

1. **Presentation Layer**
   - Handles user interface and input/output
   - Located in `Program.cs`
   - Manages user interactions and display formatting

2. **Business Layer**
   - Contains business logic and rules
   - Located in `Services/` directory
   - Handles data validation and business operations
   - Key components:
     - `IProductService` and `ProductService`: Product-related operations
     - `ISaleService` and `SaleService`: Sale-related operations

3. **Data Layer**
   - Manages data persistence
   - Located in `Services/DatabaseService.cs`
   - Handles all MongoDB operations
   - Implements `IDatabaseService` interface

## Project Structure

```
CornerShop/
├── Models/
│   ├── Product.cs
│   ├── Sale.cs
│   └── SaleItem.cs
├── Services/
│   ├── IDatabaseService.cs
│   ├── DatabaseService.cs
│   ├── IProductService.cs
│   ├── ProductService.cs
│   ├── ISaleService.cs
│   └── SaleService.cs
└── Program.cs
```

## Features

- Product management
  - Search products
  - Check stock levels
  - Update inventory
- Sales management
  - Register new sales
  - Cancel sales
  - View recent sales
- Automatic stock updates
- Transaction summaries

## Prerequisites

- .NET 6.0 or later
- MongoDB server running locally on port 27017

## Getting Started

1. Clone the repository
2. Ensure MongoDB is running locally
3. Navigate to the project directory
4. Run the application:
   ```bash
   dotnet run
   ```

## Usage

The application provides a menu-driven interface:

1. Search Products
   - Search by product name or category
   - View product details and stock levels

2. Register Sale
   - Add multiple items to a sale
   - Automatic stock updates
   - Transaction summary

3. Cancel Sale
   - View recent sales
   - Select sale to cancel
   - Automatic stock restoration

4. Check Stock
   - View all products and their current stock levels

## Error Handling

The application includes comprehensive error handling:
- Input validation
- Business rule enforcement
- Database operation error handling
- User-friendly error messages

## Dependencies

- MongoDB.Driver (2.22.0)
- MongoDB.Bson (2.22.0)

## License

This project is licensed under the MIT License - see the LICENSE file for details.
