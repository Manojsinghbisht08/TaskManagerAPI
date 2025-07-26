Task Manager API

A secure and role-based task management REST API built using ASP.NET Core Web API. Users can register, login, and manage their tasks, while Admins have full control over all users and their tasks.

🚀 Features
🔐 JWT-based Authentication

👤 Role-based Authorization (Admin & User)

📝 Task CRUD (Create, Read, Update, Delete)

👨‍💼 Admin can manage all tasks

🙋 Users can manage only their own tasks

🔒 Passwords are securely hashed using BCrypt

🛠️ Technologies Used
ASP.NET Core 7

Entity Framework Core
SQL Server
JWT (JSON Web Token)
BCrypt.Net-Next


⚙️ Getting Started
1️) Clone the repository:
bash
Copy
Edit
git clone https://github.com/your-username/TaskManagerAPI.git


2️) Open the project in Visual Studio or VS Code
3️) Add your connection string in appsettings.json:
json
Copy
Edit
"ConnectionStrings": {
  "DefaultConnection": "Server=.;Database=TaskDb;Trusted_Connection=True;"
}


4️) Install dependencies:
bash
Copy
Edit
dotnet restore

5️) Run database migration:
bash
Copy
Edit
dotnet ef migrations add InitialCreate
dotnet ef database update


6️) Run the application:
bash
Copy
Edit
dotnet run
📬 API Endpoints
Method	Endpoint	Access	Description
POST	/api/auth/register	Public	Register a new user
POST	/api/auth/login	Public	Login and get JWT token
GET	/api/task/get-all	Admin/User	Get all tasks (or own tasks)
POST	/api/task/create	Admin/User	Create a new task
PUT	/api/task/update/{id}	Admin/Owner	Update task by ID
DELETE	/api/task/delete/{id}	Admin Only	Delete task by ID

👥 Roles

👑 Admin:

Can view, update, and delete all users’ tasks

👤 User:

Can only manage their own tasks

🧪 Testing
You can test this API using:

Postman

Swagger (if enabled in Program.cs)

Thunder Client (VS Code extension)