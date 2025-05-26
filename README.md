# Corner Shop

A modern corner shop management system built with .NET 8 and MongoDB, featuring a clean architecture and comprehensive test coverage.

## Features

- Product Management
  - Search products by name
  - Get product details
  - Update product stock
  - View all products

- Sales Management
  - Create new sales
  - View recent sales
  - Cancel sales
  - Track sales history

- Database Integration
  - MongoDB for data persistence
  - Efficient querying and indexing
  - Real-time stock updates

## Prerequisites

- .NET 8 SDK
- MongoDB
- Docker (optional, for containerized deployment)

## Getting Started

### Local Development

1. Clone the repository:
   ```bash
   git clone https://github.com/yourusername/corner-shop.git
   cd corner-shop
   ```

2. Start MongoDB:
   ```bash
   docker-compose up -d mongodb
   ```

3. Run the application:
   ```bash
   dotnet run --project CornerShop
   ```

### Running Tests

```bash
dotnet test
```

### Docker Deployment

1. Build and run with Docker Compose:
   ```bash
   docker-compose up --build
   ```

2. Access the application at `http://localhost:8080`

## Project Structure

```
CornerShop/
├── CornerShop/             # Main application project
│   ├── Models/             # Data models
│   ├── Services/           # Business logic and services
│   └── Program.cs          # Application entry point
├── CornerShop.Tests/       # Unit tests
└── docker-compose.yml      # Docker configuration
```

## API Endpoints

### Products
- `GET /api/products` - Get all products
- `GET /api/products/search?term={term}` - Search products
- `GET /api/products/{name}` - Get product by name
- `PUT /api/products/{name}/stock` - Update product stock

### Sales
- `POST /api/sales` - Create new sale
- `GET /api/sales` - Get recent sales
- `GET /api/sales/{id}` - Get sale by ID
- `DELETE /api/sales/{id}` - Cancel sale

## Development

### Code Style

This project uses:
- C# 8.0 features
- Async/await for asynchronous operations
- Dependency injection
- Interface-based design

### Testing

The project includes:
- Unit tests using xUnit
- Mocking with Moq
- Code coverage reporting

### CI/CD

GitHub Actions workflow includes:
- Code linting
- Unit testing
- Code coverage
- Docker image building and publishing

## Contributing

1. Fork the repository
2. Create a feature branch
3. Commit your changes
4. Push to the branch
5. Create a Pull Request

## License

This project is created for educational purposes.

## Author

- Course: LOG430 - Architecture Logicielle
- Student: Minjae Lee [LEEM29379701]

## Acknowledgments

- MongoDB for the database
- .NET team for the framework
- xUnit for testing framework
