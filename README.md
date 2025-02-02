# Loan Application Backend (C#)

This is the backend for the Loan Application system built with **C# and .NET**. It uses **Firestore** as the database and **Swagger** for API testing.

## 🚀 Project Setup

### Prerequisites
Ensure you have the following installed on your system:
- [.NET SDK](https://dotnet.microsoft.com/)
- [Firestore Emulator or Firebase Console](https://console.firebase.google.com/)

### Installation
1. Clone the repository:
   ```sh
   git clone https://github.com/boogiedml/LoanCRUD.git
   cd LoanCRUD
   ```
2. Install dependencies:
   ```sh
   dotnet restore
   ```

### Running the Project
To start the backend server, run:
```sh
 dotnet run
```
The API will be available at `http://localhost:5058` (or another port if configured).

## 🔧 Environment Variables
Create an `appsettings.json` file in the root directory and configure the following variables:

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*",
   "FirestoreProjectId": "your-firebase-project-id",
   "GoogleCredentialsPath": "path-to-your-firebase-adminsdk.json"
}
```

## 🔥 Firestore Database
The backend interacts with Firestore to store and retrieve loan applications. Collections used:
- `Loans`: Stores loan application data.

## 📌 API Endpoints (Swagger)
To test API endpoints, start the server and visit:
```
http://localhost:5058/swagger/index.html
```

### Example Endpoints
- `GET /api/loans` - Retrieve loan applications.
- `POST /api/loans` - Submit a new loan application.
