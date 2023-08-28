![image](https://github.com/jozsapeter88/Goal.NET/assets/113460628/1259aef5-f6fe-4359-94de-24502aeeac49)



# <span style="font-size: 30px"><b>Fantasy Football App ⚽🏆</b></span>
<span style="color: blue">ℹ️ Please note that this project is currently in development and may contain bugs. Your suggestions and feedback are highly welcome!</span>

## <span style="font-size: 24px"><b>Introduction 👋</b></span>
Welcome to Goal.NET, a fantasy football-themed application that brings together .NET on the backend and React on the frontend. Please keep in mind that this version is not yet production-ready and serves as a showcase of the potential this project holds for fantasy football enthusiasts.

## <span style="font-size: 24px"><b>Running the Project with Docker Compose 🛠️</b></span>
Follow these steps to run Goal.NET on your local machine:
```bash
# Clone the repository to your local system.
$ git clone git@github.com:jozsapeter88/Goal.NET.git

# Open the solution
# Navigate to the project directory.
$ cd Goal.NET

# Make a copy of the .env_public files
In the project root folder find .env_public_compose, rename it to .env and add your strong password,
(in the frontend folder find .env_public_frontend you can modify the URL as well if you want)

# Open the appsettings.json file and you can update the "DockerCommandsConnectionString" value with your database connection string.

# Build Docker Images:
Navigate to the project directory and build the Docker images using the following command:

$ docker-compose build

# Start the containers in the following order

# Start the database container
$ docker-compose up -d db

# Start backend container
$ docker-compose up -d goaldotnetbackend

# Start frontend container
$ docker-compose up -d goaldotnetfrontend

# Open your web browser and go to http://localhost:3000 to explore the Goal.NET App.
```
🚀 Enjoy your fantasy football experience with Goal.NET!

# Stats
<img alt="GitHub Language Count" src="https://img.shields.io/github/languages/count/jozsapeter88/Goal.NET" /> <img alt="GitHub Top Language" src="https://img.shields.io/github/languages/top/jozsapeter88/Goal.NET" /> <img alt="" src="https://img.shields.io/github/repo-size/jozsapeter88/Goal.NET" /> <img alt="GitHub Contributors" src="https://img.shields.io/github/contributors/jozsapeter88/Goal.NET" /> <img alt="GitHub Last Commit" src="https://img.shields.io/github/last-commit/jozsapeter88/Goal.NET" />
