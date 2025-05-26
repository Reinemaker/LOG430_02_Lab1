# Corner Shop

A modern, console-based shop management system built with .NET 8 and MongoDB, featuring clean architecture, containerization, and comprehensive documentation.

## Features

- Product Management: search, view, and update products
- Sales Management: register, view, and cancel sales
- Inventory tracking and real-time stock updates
- MongoDB integration for data persistence

## Prerequisites

- .NET 8 SDK
- Docker & Docker Compose (for containerized deployment)
- MongoDB (if running locally without Docker)

## Getting Started

### Local Development

1. Clone the repository:
   ```bash
   git clone https://github.com/yourusername/corner-shop.git
   cd corner-shop
   ```
2. Restore dependencies:
   ```bash
   dotnet restore
   ```
3. Build the project:
   ```bash
   dotnet build
   ```
4. Start MongoDB (if not using Docker Compose):
   ```bash
   # Ensure MongoDB is running on localhost:27017
   ```
5. Run the application:
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
2. The application will connect to the MongoDB service defined in `docker-compose.yml`.

## Project Structure

```
CornerShop/
├── CornerShop/             # Main application project
│   ├── Models/             # Data models
│   ├── Services/           # Business logic and services
│   └── Program.cs          # Application entry point
├── CornerShop.Tests/       # Unit tests
├── docker-compose.yml      # Docker configuration
├── Dockerfile              # Application containerization
├── docs/                   # Technical documentation
│   ├── README.md           # Technical documentation & setup
│   ├── ADR/                # Architecture Decision Records
│   └── UML/                # UML diagrams (4+1 views)
```

## Documentation

- **Technical documentation** is located in the [`docs/`](docs/) directory.
  - [docs/README.md](docs/README.md): Setup, usage, technology choices, and justifications
  - [docs/ADR/](docs/ADR/): Architecture Decision Records (ADRs)
  - [docs/UML/](docs/UML/): UML diagrams for all 4+1 architectural views

### UML Diagrams (4+1 Views)
- Logical View: `docs/UML/logical-view.puml`
- Development View: `docs/UML/development-view.puml`
- Process View: `docs/UML/process-view.puml`
- Physical View: `docs/UML/physical-view.puml`
- Use Case/Sequence: `docs/UML/sale-sequence.puml`

### Architecture Decisions
- See `docs/ADR/` for detailed ADRs on database choice, architecture, testing, and error handling.

## Development Practices
- C# 8.0 features, async/await, interface-based design
- Unit tests with xUnit and Moq
- Code formatting enforced via `.editorconfig` and `dotnet format`
- CI/CD with GitHub Actions: linting, testing, coverage, Docker build

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
