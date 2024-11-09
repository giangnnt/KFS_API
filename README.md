KFS_API

KFS_API is a RESTful API built with C# and the .NET Framework to support data management and operations for Koi Fish Show competitions. This API facilitates registration, scoring, and results tracking for koi exhibitions.

Table of Contents
Getting Started
Prerequisites
Installation
Configuration
Usage
API Documentation
Contributing
License
Getting Started
Follow these instructions to set up the project locally for development and testing purposes.

Prerequisites
.NET Framework (version compatible with the project)
SQL Server (or compatible database)
Installation
Clone the repository:

bash
Copy code
git clone https://github.com/giangnnt/KFS_API.git
cd KFS_API
Open the project in Visual Studio.

Restore NuGet packages:

Go to Tools > NuGet Package Manager > Manage NuGet Packages for Solution.
Restore any missing packages.
Configuration
Set up a database and update the connection string in appsettings.json or Web.config, depending on your configuration:

{
  "ConnectionStrings": {
    "DefaultConnection": "Server=your_server;Database=KFS_DB;User Id=your_user;Password=your_password;"
  }
}

Configure any other necessary settings, such as JWT tokens or API keys, in appsettings.json.

Usage
Run the application from Visual Studio using Ctrl+F5 or by selecting Debug > Start Without Debugging.

The API should be accessible at http://localhost:port.

API Documentation
The API provides the following key endpoints:

/api/register - Manage participant registrations.
/api/login - Handle login operations.
For detailed documentation, see the Swagger API document.

Contributing
Fork the repository.
Create a new branch for your feature (git checkout -b feature/NewFeature).
Commit your changes (git commit -am 'Add new feature').
Push to the branch (git push origin feature/NewFeature).
Open a Pull Request.
License
This project is licensed under the MIT License.