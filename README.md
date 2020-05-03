# library-management

# Backend desing
 Overall solution has been developed as a clean arthitecture it contains 4 abstract layers: api, application, domain and infrastructure.

 Each api controller inherit from BaseApiController. In BaseApiController it is defined the routing, and how to interpret the 
 response from the handler in terms of HTTP Status.
   
 Each controller action is sent to handler via the MediatR bus, the handler defines the logic to be performed, and validations per
 request. The handler and the api model for each action are located at LibraryManagement.Application.

 The domain layer/core contains the collection of definitions for the app model, repository and configuration.

 In the infrastructure layer contains the persistence logic that uses the repository pattern, each repository inherit from
 BaseRepository. The infrastructure layer is not exclusive for the persistence, but it can be expanded to contain e.g. file operations,
 external api call etc.
 
 # Setup
 To run the application, you need:

 1. Create a MS SQL database schema
 2. Set the connection string in the config file 'app-api/Application/LibraryManagement.Api/appsettings.json'.
 3. In Package Manager Console chose the LibraryManagement.Infrastructure project and run the commands:
     >Add-Migration InitialCreate
     
     >Update-Database

 4. To run unit testing you can use the EF Core in-memory database provider or your testing db, the default configuration is to use in
     memory database, but you can change it at 'app-api/Tests/LibraryManagement.Tests/appsettings.Tests.json' by changing IsTesting to
     false.


 
