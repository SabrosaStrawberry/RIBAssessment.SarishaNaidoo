# Employee Portal
This project is a full-stack web application built with .NET Core (backend) and Angular (frontend). The backend provides RESTful APIs, and the Angular frontend consumes these APIs to deliver a responsive user experience.

### Prerequisites
Before running the project, ensure you have the following installed:

- .NET 8 SDK\
  =>Download .NET SDK
- Node.js (and npm)\
  =>Download Node.js
- Angular CLI (if not installed globally)\
  =>npm install -g @angular/cli
- Ms SQLServer/ MS SQLSERVEREXPRESS

### Project Structure
 - /../RIBAssessment.SarishaNaidoo-master: Contains the .NET Core Web API project.
 - /../RIBAssessment.SarishaNaidoo-master/RIBAssessment.SarishaNaidoo/ClientApp: Contains the Frontend Angular project

### Running the project 
1. Navigate to appsettings.json to set up your connection string to SQL SERVER
2. Update the environment file in the angular project with the URL and port number that the API is running on
3. EF Core Code First was used to create the db, and the db should be created for you when you run the project
   for the first time, but the dacpac and SQL scripts that can be used to create the db have been provided as well.
4. JWT auth has been used for this simple application, register your email (or any string with the correct email format as there is no verification step)
   and use these credentials to login thereafter.

