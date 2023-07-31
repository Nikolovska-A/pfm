# Personal Finance Management

### The Personal Finance Management application is a web-based application built using .NET that allows users to start tracking their financial activities. Users can add expenses, income, view reports, and manage their financial transactions efficiently.

## Getting started

### 1.1 Clone this repository to your local machine: https://github.com/Nikolovska-A/pfm.git .
    1.2 Open the project in Visual Studio.

## Docker Compose Setup

### 2.1 Make sure you have Docker and Docker Compose installed and running.
    2.2 In Visual Studion set docker-compose as start up project and click run.
    2.3 After the containers are build you can start the project.
    2.4 The app should now be accessible on http://localhost:80 .
    
## Postman Tests with Newman Reports    

### 3.1 Import the provided Postman collection and environment files located in the Test/PostmanTests directory.
    3.2 In the terminal run the following commands to execute the Postman tests and generate a Newman report:
    
        newman run Tests/PostmanTests/TestPFMBackendAPI.postman_collection.json -e Tests/PostmanTests/PFMBackendAPIEnvironment.postman_environment.json -r json --reporter-json-export newman_results.json

        newman run Tests/PostmanTests/TestPFMBackendAPI.postman_collection.json -e Tests/PostmanTests/PFMBackendAPIEnvironment.postman_environment.json  -r html --reporter-html-export newman_report.html
        
## Executing Stored Procedures

### 4.1 Connect to your SQL Server instance.
    4.2 Locate the SQL scripts containing the stored procedures in the database directory.
    4.3 Execute the scripts to create the necessary stored procedures in your database.

    

