# ETherapist
## Overview
Etherapist is a web app created with ASP.NETcore framework written in C#.
The ETherapist as the name depicts is a platform that allows users to set up meeting sessions with therapists to address several mental health conditions. The app offers two major services:
- Cameron Diagnostics
- Sessions

#### Cameron:
Cameron is a free tool that allows users to get a quick diagnoses of their mental condition by answering a few questions. In the backend, these answers are compared against answers in the database, results are computed and returned to the user once the test is submitted.

#### Sessions:
The Etherapist app also allows users to create sessions after which therapists will be assigned by an administrator. To create sessions, a user has to create a paid subscription plan (using stripe payment API).

## Technologies Used
- ASP.NET Core
- C#
- Microsoft SQL Server
- Docker & Docker Compose

## Prerequisites
- [Dotnet CLI tool](https://docs.microsoft.com/en-us/dotnet/core/install/windows?tabs=net60)
- [Microsoft SQL Server](https://docs.microsoft.com/en-us/sql/ssms/download-sql-server-management-studio-ssms?view=sql-server-ver16) (Optional)

## Installation and Usage
1.  Clone the repo: ```git clone  https://github.com/SaheedLawanson/MyETherapist.git```
2.  Install the project dependencies: ```dotnet restore```
3.  Setup the database:
    **Option 1** (Use MSSQL Server locally):
    -  Open MSSQL Server, select the "Server name" dropdown to see a list of available servers (In my case "SAHEED" and authentication type is "Windows Authentication")
    
    ![sqlserver_illustration](https://user-images.githubusercontent.com/92232710/186526667-3dc0b8ef-5df9-4870-9579-f92fd2d3c670.png)
    -  In the etherapist project folder, open appsettings.development.json. Replace the value of server in the ConnectionStrings.DefaultConnection with the server name you want to use 
    
    ![appsettingJson](https://user-images.githubusercontent.com/92232710/186527271-c2fa5bf9-70eb-4c1b-a949-92a138fd6a21.png)

    **Option 2** (Use MSSQL with container):
    - In the command line, run: ```docker-compose up``` and wait till the container is up
    - Replace the ConnectionStrings.DefaultConnection with "Initial Catalog=etherapist; Data Source=localhost,1433; Persist Security Info=True;User ID=SA;Password=ExamplePassword123"
4.  Create a free stripe account and get an API key [here](https://paymentsplugin.com/blog/stripe-api-keys), then populate the appsettings.Development.json file with the Secret and publishable key.
5.  Finally, in the command line, run: ```dotnet run ```
6.  A default admin user is created with the following details:
    **Email**: etherapistAdmin@gmail.com
    **Password**: Admin123*
    only an admin can create a therapist and another admin account
