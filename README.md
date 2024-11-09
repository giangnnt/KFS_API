# KFS_API

KFS_API is a RESTful API built with C# and the .NET Framework to support data management and operations for Koi Farm Shop management.

## Table of Contents
- [Getting Started](#getting-started)

- [Prerequisites](#Prerequisites)

- [Installation](#Installation)

- [Configuration](#Configuration)

- [Usage](#Usage)

- [API Documentation](#api-documentation)

- [Contributing](#contributing)

- [License](#license)

## Getting Started
Follow these instructions to set up the project locally for development and testing purposes.

## Prerequisites
- .NET Framework (version compatible with the project).

- SQL Server (or compatible database).

## Installation
1. Clone the repository:

```
git clone https://github.com/giangnnt/KFS_API.git
```
2. Open the project in Visual Studio.
```
cd KFS
```

3. Restore NuGet packages:
```
dotnet restore
```
4. Update database:
```
dotnet ef database update
```
## Configuration
1. Set up a database and update the connection string in `appsettings.json`:

```bash
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "ConnectionStrings": {
    "DatabaseConnection": "Server=your-database;Database=KFS;User=username;Password=password;MultipleActiveResultSets=true;TrustServerCertificate=true",
    "RedisConnection": "cache-database,password=password,abortConnect=false"
  },
  "VNPay": {
    "TmnCode": "3URINRGX",
    "HashSecret": "L4KXST7FQEIWCNCB4B3U8BZ75QTSL74P",
    "BaseUrl": "http://sandbox.vnpayment.vn/paymentv2/vpcpay.html",
    "Version": "2.1.0",
    "Command": "pay",
    "CurrCode": "VND",
    "Locale": "vn",
    "PaymentBackReturnUrlOrder": "http://localhost:5000/api/order/vnpayment-return",
    "PaymentBackReturnUrlConsignment": "http://localhost:5000/api/consignment/vnpayment-return",
    "RedirectUrl": "http://localhost:5000/swagger"
  },
  "GHN": {
    "Token": "00fb2e9d-94cb-11ef-8e53-0a00184fe694",
    "ShopId": "195170",
    "BaseUrl": "https://dev-online-gateway.ghn.vn"
  },
  "JWT_SECRET": "your-jwt-secret-code"
}
```

2. Configure any other necessary settings, such as JWT tokens or API keys, in `appsettings.json`.

## Usage
1. Run the application:
```
dotnet watch run
```

2. The API should be accessible at `http://localhost:5000/api`.

## API Documentation
The API provides the following key endpoints:

- /api/register - Manage participant registrations.
- /api/login - Handle login operations.


For detailed documentation, see the Swagger API documentation.

## Contributing
1. Fork the repository.
2. Create a new branch for your feature (git checkout -b feature/NewFeature).
3. Commit your changes (git commit -am 'Add new feature').
4. Push to the branch (git push origin feature/NewFeature).
5. Open a Pull Request.
## License
This project is licensed under the [MIT](https://choosealicense.com/licenses/mit/) License.