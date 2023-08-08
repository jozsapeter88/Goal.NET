# <span style="font-size: 30px"><b>Goal.NET - Fantasy Football App ⚽🏆</b></span>
<span style="color: blue">ℹ️ Please note that this project is currently in development and may contain bugs. Your suggestions and feedback are highly welcome!</span>

## <span style="font-size: 24px"><b>Introduction 👋</b></span>
Welcome to Goal.NET, a fantasy football-themed application that brings together .NET on the backend and React on the frontend. Please keep in mind that this version is not yet production-ready and serves as a showcase of the potential this project holds for fantasy football enthusiasts.

## <span style="font-size: 24px"><b>Installation Guide 🛠️</b></span>
Follow these steps to run Goal.NET on your local machine:

```bash
# Clone the repository to your local system.
$ git clone https://github.com/your-username/goal-net.git

# Navigate to the project directory.
$ cd goal-net

# Open the appsettings.json file and update the "DefaultConnection" value with your database connection string.

# Navigate to the backend directory.
$ cd backend

# Apply database migrations.
$ dotnet ef database update

# Navigate to the frontend directory.
$ cd ../frontend

# Install frontend dependencies.
$ npm install

# Start the backend server.
$ cd ../backend
$ dotnet run

# Start the frontend development server.
$ cd ../frontend
$ npm start

# Open your web browser and go to http://localhost:3000 to explore the Goal.NET App.
```

🚀 Enjoy your fantasy football experience with Goal.NET!
