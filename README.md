# library-management
 .NetCore + VueJS library management app

 There are two folders 'app-api' containing the aplication api, in .NET Core 3.1 that uses MS SQL as db, and app-ui containing the VueJS
 app created with '@vue/cli 4.3.1'


# Backend desing
 Overall the backed has been developed as a clean arthitecture developed with .NET Core Api 3.1 it contains 4 abstract layers: api,
 application, domain and infrastructure.

 Each api controller inherit from BaseApiController. In BaseApiController it is defined the routing, and how to interpret the 
 response from the handler in terms of HTTP Status.
   
 Each controller action is sent to handler via the MediatR bus, the handler defines the logic to be performed, and validations per
 request. The handler and the api model for each action are located at LibraryManagement.Application.

 The domain layer/core contains the collection of definitions for the app model, repository and configuration.

 In the infrastructure layer contains the persistence logic that uses the repository pattern, each repository inherit from
 BaseRepository. The infrastructure layer is not exclusive for the persistence, but it can be expanded to contain e.g. file operations,
 external api call etc.
 
 
 # Frontend
 The fromtend has been developed with @vue/cli 4.3.1 (https://vuejs.org/) and bootstrap for styling. 
 Since the user stories and the backend have user context, I have created a login page that does jwt sigin in the backend. The
 autentification token is stored in the local storage. The UI in each roution action if the authentication data are present in the
 local storage, if not then the app redirects in the login page. 
 The other part is simple ajax comunication with the backend and refreshing the UI after the user action.
 
 
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


 
