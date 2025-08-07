# User Management Technical Exercise

This is my submission for the User Management Technical Exercise. The goal of this exercise is to extend a simple user management application with additional functionality and improvements.

Below is a brief overview of the tasks I have completed, along with instructions on how to run the application and test the features.

Using the existing solution (UserManagement.Web) - I have implemented the following features:

### 1. Filters Section (Standard)
### 2. User Model Properties (Standard)
### 3. Actions Section (Standard)
### 4. Data Logging (Advanced)

In order to run the application and test these features, please set the UserManagement.Web project as the startup project in Visual Studio and run it.

### 5. Extend the Application (Expert)

I have implement (In the Modern UI solution folder) a Blazor based version of the front end application supported by an API based backend.

In order to run the Blazor application, please set the UserManagement.UI project and the UserManagement.Api projects as the startup projects in Visual Studio.

Ensure that the base URL for the API is set correctly in the `appsettings.json` file of the UserManagement.UI project. (example  "BaseUrl": "https://localhost:7097") and run the application.

This should allow you to access the Blazor application in your web browser, and also expose the swagger documentation for the API.

