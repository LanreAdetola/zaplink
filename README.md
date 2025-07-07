# ⚡ ZapLink — Personal Link Organizer

ZapLink is a Blazor WebAssembly app backed by an Azure Functions API and Cosmos DB. It allows users to add, edit, delete, and view categorized personal links like GitHub, LinkedIn, portfolios, etc.

---

## 🧱 Project Structure

```
ZaplinkApp/
│
├── ZapLink/           # Blazor WebAssembly frontend
├── ZapLink.Api/       # Azure Functions backend
└── Zaplink.sln        # Solution file
```

---

## 🚀 Prerequisites

- .NET SDK 8 or 9: https://dotnet.microsoft.com/download
- Azure Functions Core Tools: https://learn.microsoft.com/en-us/azure/azure-functions/functions-run-local
- Visual Studio or VS Code

---

## 🔧 Setup Instructions

### 1. Clone the Repo

```bash
git clone https://github.com/LanreAdetola/zaplink.git
cd zaplink
```

### 2. Restore & Build

```bash
dotnet restore
dotnet build
```

---

## 🧩 Run the Backend

```bash
cd ZapLink.Api
func start
```

Make sure `local.settings.json` exists in `ZapLink.Api/`:

```json
{
  "IsEncrypted": false,
  "Values": {
    "AzureWebJobsStorage": "UseDevelopmentStorage=true",
    "FUNCTIONS_WORKER_RUNTIME": "dotnet-isolated",
    "CosmosDbConnection": "<YOUR_COSMOS_DB_CONNECTION_STRING>"
  }
}
```

---

## 🖥️ Run the Frontend

```bash
cd ZapLink
dotnet run
```

Visit: http://localhost:5125

---

## 🔁 API Endpoints

| Method | Endpoint                    | Description        |
|--------|-----------------------------|--------------------|
| POST   | `/api/links`                | Add a new link     |
| GET    | `/api/links/{userId}`       | Get user links     |
| PUT    | `/api/links/{id}`           | Update a link      |
| DELETE | `/api/links/{id}/{userId}`  | Delete a link      |

---

## 📦 Deployment

Deploy to Azure Static Web Apps + Azure Functions + Cosmos DB.

---

## 🧠 Author

[Lanre Adetola](https://github.com/LanreAdetola)

---

## 📝 License

MIT — Free to use, share, and contribute.
