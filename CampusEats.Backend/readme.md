# 🍽️ CampusEats

CampusEats is a modern food ordering system designed for university campuses, built with a **clean architecture** and **modern technologies** for scalability and maintainability.

---

## 🧩 Overview

CampusEats provides a platform for students to browse menus, place orders, manage favorites, and handle payments — all while following clean backend principles and secure authentication practices.

Built with:
- ⚙️ **.NET 9 (Minimal APIs + Vertical Slice Architecture)**
- 💻 **Blazor WebAssembly** frontend *(planned)*
- 🐘 **PostgreSQL** as the database
- 🔐 **JWT** for authentication
- 🧱 **Entity Framework Core (Npgsql provider)**

---

## ✅ Tech Stack

| Layer        | Technology             |
|---------------|------------------------|
| Backend       | .NET 9 Minimal APIs    |
| Frontend      | Blazor WebAssembly     |
| Database      | PostgreSQL             |
| ORM           | Entity Framework Core  |
| Auth          | JWT                    |
| Architecture  | Vertical Slice Pattern |

---

## 🚀 Getting Started

### 🧱 Prerequisites

Make sure you have the following installed:

- [.NET 9 SDK](https://dotnet.microsoft.com/)
- [PostgreSQL](https://www.postgresql.org/)
- [Postman](https://www.postman.com/)  for API testing

---

## 🐘 PostgreSQL Setup

### 1️⃣ Create the Database

You can use pgAdmin or the PostgreSQL CLI:

```sql
CREATE DATABASE CampusEats;
```

> 🧠 The project expects a database named `campuseats`, but you can change this later in your configuration.

---

## 🔐 Managing Secrets (Development Only)

Instead of storing passwords in `appsettings.json`, CampusEats uses **.NET User Secrets** to safely store local development credentials.

### 1️⃣ Initialize User Secrets

Inside the backend project directory (`CampusEats.Backend`):

```bash
dotnet user-secrets init
```

This command adds a `UserSecretsId` to your `.csproj` file.

---

### 2️⃣ Set the PostgreSQL Connection String

Replace the placeholders and run:

```bash
dotnet user-secrets set "ConnectionStrings:Default" "Host=localhost;Port=your_password;Database=your_db_name;Username=your_username;Password=your_password"
```

📝 Replace:
- `localhost` → your PostgreSQL host (if remote)
- `your_port` → port (default: 5432)
- `your_db_name` → your database name
- `your_password` → your actual PostgreSQL password

---

### 3️⃣ View or Edit Secrets

To list all secrets:
```bash
dotnet user-secrets list
```

To remove one:
```bash
dotnet user-secrets remove "ConnectionStrings:Default"
```

---

### 📂 Secret File Location

These secrets are stored **outside** your repository and are **never committed** to Git.

- **Windows**:  
  `%APPDATA%\Microsoft\UserSecrets\<UserSecretsId>\secrets.json`

- **macOS/Linux**:  
  `~/.microsoft/usersecrets/<UserSecretsId>/secrets.json`

Example contents of `secrets.json`:

```json
{
  "ConnectionStrings:Default": "Host=localhost;Port=your_port;Database=your_db_name;Username=your_username;Password=your_password"
}
```

---

## ⚙️ Run the Backend

Once secrets are configured, restore and run the app:

```bash
dotnet restore
dotnet run
```

---

### 🧭 Localhost:Port

When the app runs successfully, call the host on postman:

```
http://localhost:5228
```
---

## 📂 Backend Folder Structure

```
CampusEats.Backend/
├── Configuration/
├── Features/
├── Middleware/
├── Persistence/
├── Services/
├── Shared/
├── appsettings.json
└── Program.cs
```

---

## 🧰 Useful Commands

| Command | Description |
|----------|--------------|
| `dotnet restore` | Restore all dependencies |
| `dotnet build` | Build the solution |
| `dotnet run` | Run the API locally |
| `dotnet user-secrets list` | List stored secrets |
| `dotnet ef database update` | Apply EF migrations (if using migrations) |

---

## 🧠 Notes

- `appsettings.json` contains **no sensitive data** — all secrets are stored in User Secrets.
- Use **User Secrets only for local development**.
- For production, configure connection strings via **environment variables** or a **secret manager**.
