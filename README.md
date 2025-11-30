# Readioo

Readioo - Book Tracking & Discovery Platform

📖 Overview

Readioo is a full-stack web application designed for book lovers to discover new titles, track their reading journey, and manage their personal bookshelves. Built with ASP.NET Core 8, it features a clean 3-tier architecture, robust user management, and an intelligent recommendation system.

Whether you want to organize your library into "Want to Read" or "Currently Reading" lists, rate books, or get personalized suggestions based on your taste, Readioo handles it all.

✨ Key Features

📚 Book Management

Browse & Search: Explore a vast library of books with dynamic filtering by genre.

Detailed View: View comprehensive book details including author info, ratings, and community reviews.

Search with Autocomplete: Fast and responsive search bar with instant suggestions.

🔖 Smart Bookshelves

Personalized Shelves: Every user gets default shelves: Want to Read, Currently Reading, Read, and Favorites.

Dynamic Organization: Easily move books between shelves using a dropdown menu without refreshing the page.

Visual Tracking: See your reading progress and collection stats at a glance.

🤖 Intelligent Recommendations

Personalized Engine: Suggests new books based on your highly-rated reads (4+ stars).

Genre & Author Matching: Finds hidden gems from your favorite genres or authors you haven't read yet.

Cold Start Handling: Shows top-rated trending books for new users.

🛠️ Technology Stack

Backend

Framework: ASP.NET Core 8 (MVC)

Language: C#

Database: SQL Server

ORM: Entity Framework Core (Code-First)

Authentication: ASP.NET Core Identity

Frontend

Tech: Razor Views, HTML5, CSS3

Styling: Bootstrap 5, Custom CSS

Interactivity: jQuery, AJAX (Fetch API)

Notifications: Toastr.js

Architecture

Pattern: 3-Tier Architecture (Presentation, Business, Data Access)

Design Patterns: Repository Pattern, Unit of Work, Dependency Injection

🚀 Getting Started

Prerequisites

.NET 8 SDK

SQL Server (LocalDB or Express)

Visual Studio 2022 or VS Code

📂 Project Structure

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
    ├── Contexts/          # Ef Core DbContext
    ├── Models/            # Database Entities
    └── Repositories/      # Data Access Logic


Built with ❤️ with my Team:

Abanoub Osama

Shorouk Aboelela

Rawan Mohamed 

Marina Bebawy

Karim Mohamed
