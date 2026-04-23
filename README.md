🛒 E-Commerce API

A scalable and cleanly structured E-Commerce Backend API built using modern backend practices. This project provides core e-commerce functionalities including product browsing, cart management, and order processing, with secure authentication and authorization.

# 📌 Features
🔐 Authentication & Authorization
JWT Authentication
Policy-Based Authorization
Microsoft Identity Integration

# 🛍️ Product Management
Browse products with filtering, search, and pagination
CRUD operations for products

# 📂 Category Management
Manage product categories
Assign images to categories

# 🛒 Cart Management
Add, update, and remove items
Retrieve user cart

# 📦 Order Management
Place orders
View order history and details

# 🖼️ File Upload
Upload images for products and categories

# 🏗️ Architecture
The project follows Clean Architecture principles with:

N-Tier Architecture
Presentation Layer (API)
Application Layer
Infrastructure Layer
Common Layer
Design Patterns
Repository Pattern (Generic & Specific)
Unit of Work Pattern
Result Pattern (Standard API Response)
Best Practices
DTOs for data transfer
Fluent Validation
Async/Await for performance
Dependency Injection

# ⚙️ Technologies Used
ASP.NET Core Web API
Entity Framework Core
Microsoft Identity
JWT Authentication
FluentValidation
SQL Server (or your DB)

# 🔐 Authentication Notes
Users authenticate via JWT tokens.
UserId is NOT passed in requests.
User identity is extracted from JWT Claims.

# 📡 API Endpoints
# 🔑 Authentication
Method	Endpoint	Description
POST	/api/Auth/Register
POST	/api/Auth/Login	

# 📂 Categories
Method	Endpoint
GET	/api/categories
GET	/api/Category/{id}
POST	/api/Category
PUT	/api/Category/{id}
DELETE	/api/Category/{id}

# 🛍️ Products
Method	Endpoint
GET	/api/Product
GET	/api/Product/{id}
POST	/api/Product
PUT	/api/Product/{id}
DELETE	/api/Product/{id}

# Query Params Example:
-categoryId
-name
-pageNumber
-pageSize
/api/products?categoryId=1&name=phone&pageNumber=1&pageSize=10

# 🛒 Cart
Method	Endpoint
POST	/api/Cart
PUT	/api/Cart
DELETE	/api/Cart/{productId}
GET	/api/Cart

# 📦 Orders
Method	Endpoint
POST	/api/Order
GET	/api/Order
GET	/api/Order/{id}

# 🖼️ File Upload
Method	Endpoint
POST	/api/Image/upload
POST	/api/Product/{id}/image
POST	/api/Category/{id}/image

# 🚀 Getting Started
Prerequisites
.NET SDK
SQL Server
Visual Studio / VS Code
Setup Steps

# Clone repo
git clone https://github.com/YoussefKassab1/E-Commerce-API-Project.git

# Navigate to project
cd ecommerce-api

# Apply migrations
dotnet ef database update

# Run project
dotnet run

# 🧪 Testing
API tested using Postman
A Postman collection can be included for easier testing
Demo video included in the Readme file

# 📁 Project Structure
ECommerceAPI/
│
├── Common                  # Shared Utilities
├── APIs Layer              # Controllers & Endpoints
├── Business Logic Layer    # DTOs & Services & Validations
├── Data Access Layer       # Entities & Core Models & Repos

👉 Postman Testing Video: (Add your video link here)

👨‍💻 Author
Youssef Kassab
