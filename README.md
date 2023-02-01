# Booking-App-With-gRPC
an example of a simple application with two rest APIs that communicates with each other using gRPC

## Prerequisites
PostgreSQL is used for the database of both of the APIs. You will need to execute the **migrations** before starting the project, so that the databases are generated.

## Grpc servers and clients examples:
 
1. **BookingApp.Users.Service**
- **UsersService:**
  - **GetUserById** - an example for `GET` request that returns a **single** result and Authorization with a particular role
  - **GetAllUsers** - an example for `GET` request that returns a **collection** of results. In gRPC it is implemented with streams
- **FilesService:**
  - **UploadFile** - an example for **uploading** of a file - the endpoint receives a stream as a parameter
  - **DownloadFile** - an example for **downloading** of a file  
**NB:** both files are declared as **servers** in the .csproj of the project
  
2. **BookingApp.Users.Client**  
The client that is installed in the Rooms API through which the service can make calls to the Users gRPC service. The client is created as a NuGet package. It has the definitions of the both services declared in the **BookingApp.Users.Service** and is declared as a *client* in the .csproj of the project 

## Grpc server and http client examples:

1. **BookingApp.Rooms.Service**  
This service is an example how to call a gRPC endpoint with HTTP request and JSON transcoding.
- **BookingsService:**
  - **GetLastUserBookingAsync** - an example for `GET` request that returns a **single** result
- **Required dependencies and prerequisites to make possible gRPC to be called with HTTP request**
  - `google/api` folder and its content
  - `option (google.api.http)` in the proto file
  - `Microsoft.AspNetCore.Grpc.JsonTranscoding` NuGet package
  - `services.AddGrpc().AddJsonTranscoding()` in Program.cs
 
2. **BookingApp.Rooms.Client**  
This is a HTTP client implemented using Refit library. It is created as a NuGet package that is installed in the Users API and is used to send a **HTTP** (not protobuf) request to the gRPC BookingApp.Rooms.Service that will be handled by the service and will return a JSON response.

## Health checks examples
In the BookingApp.Users.API is implemented a basic example of two health checks:

**1. SampleHealthCheck**
  
  SampleHealthCheck implements IHealthCheck and is registered in the Program.cs with the name *testSample*.    
  You can test it by sending a request to the */healthz/ready* - it will be handled by SampleHealthCheck.
  
**2. /healthz/live**  
  It will always return true (Healthy status) since it has a separate HealthCheckOptions.
  
## Polly examples
There are example of **retry**, **fallback**, and **circuit breaker** polly in the BookingApp.Rooms.DomainService. They are used for the sending of the requests to the Users API and when these requests time out or fail with some exceptions.
