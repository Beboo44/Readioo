# Readioo - Book Tracking & Discovery Platform

## 📖 Overview

Readioo is a full-stack web application designed for book lovers to discover new titles, track their reading journey, and manage personalized bookshelves. Built with **ASP.NET Core 8**, the platform follows a clean 3-tier architecture, includes secure user authentication, and offers smart book recommendations.

Whether you're tracking reading progress, saving favorites, or looking for your next great read — **Readioo makes the experience simple, interactive, and enjoyable**.

---

## ✨ Key Features

### 📚 Book Management

* **Browse & Search:** Explore a rich library of books with dynamic filtering by genre.
* **Detailed Book Pages:** Includes author info, ratings, community reviews, and description.
* **Autocomplete Search:** Instant suggestions with a responsive search bar.

### 🔖 Smart Bookshelves

* **Personalized Shelves:** Default lists include *Want to Read*, *Currently Reading*, *Read*, and *Favorites*.
* **Quick Organization:** Move books between shelves with a simple dropdown — no page reload.
* **Visual Tracking:** View reading progress and bookshelf statistics.

### 🤖 Intelligent Recommendations

* **Personalized Engine:** Suggests books based on your 4+ star rated titles.
* **Genre & Author Matching:** Recommends unread books from your favorite genres and authors.
* **Cold Start-Friendly:** New users receive trending, top-rated book suggestions.

---

## 🛠️ Technology Stack

### 🔧 Backend

* **Framework:** ASP.NET Core 8 (MVC)
* **Language:** C#
* **Database:** SQL Server
* **ORM:** Entity Framework Core (Code-First)
* **Authentication:** ASP.NET Core Identity

### 🎨 Frontend

* **Views:** Razor Pages
* **Tech:** HTML5, CSS3
* **Styling:** Bootstrap 5, custom CSS
* **Interactivity:** jQuery, Fetch API (AJAX)
* **Notifications:** Toastr.js

### 🧱 Architecture

* **Pattern:** 3-Tier Architecture (Presentation, Business, Data Access)
* **Design Patterns:** Repository Pattern, Unit of Work, Dependency Injection

---

## 🚀 Getting Started

### ✔️ Prerequisites

* .NET 8 SDK
* SQL Server (LocalDB or Express)
* Visual Studio 2022 or VS Code

### 📦 Installation

1. Clone the repository:

   ```bash
   git clone https://github.com/your-repo/Readioo.git
   ```
2. Navigate to the main project directory:

   ```bash
   cd Readioo
   ```
3. Update the database connection string in `appsettings.json`.
4. Run EF Core migrations:

   ```bash
   update-database
   ```
5. Run the application:

   ```bash
   dotnet run
   ```

---

## 📂 Project Structure

```
Readioo/
├── Readioo.Web (Presentation Layer)
│   ├── Controllers/       # MVC Controllers (Book, Home, Shelf)
│   ├── Views/             # Razor Views
│   └── wwwroot/           # Static assets (CSS, JS, Images)
│
├── Readioo.Business (Business Logic Layer)
│   ├── Services/          # Core Logic (BookService, ShelfService)
│   ├── DTOs/              # Data Transfer Objects
│   └── Interfaces/        # Service Contracts
│
└── Readioo.Data (Data Access Layer)
    ├── Contexts/          # EF Core DbContext
    ├── Models/            # Database Entities
    └── Repositories/      # Data Access Logic
```

---

## ❤️ Built with Love by the Team

* Abanoub Osama
* Shorouk Aboelela
* Rawan Mohamed
* Marina Bebawy
* Karim Mohamed

---

## 📜 License

This project is for educational and portfolio purposes. You may extend or modify it as needed.
