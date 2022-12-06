
# Getting Started with ThingSpace Connectivity Management API

## Introduction

The ThingSpace Connectivity Management API allows you to add and activate devices, check their status, monitor their usage, monitor their reachability for Data and SMS communication as well as their connection status, and perform other device connectivity management tasks through a RESTful API. You can use the API to add connectivity management to anything from small apps to enterprise software systems, such as enterprise resource planning (ERP), supply chain management and customer service management. The API provides a secure, standards-compliant REST interface to the web services at the ThingSpace Data Center. You can download the <a href="/content/dam/thingspace-portal/resources/documentation/swagger/m2m-all.json" download = "Connectivity_Management.json">JSON file here</a>.

## Building

The generated code uses the Newtonsoft Json.NET NuGet Package. If the automatic NuGet package restore is enabled, these dependencies will be installed automatically. Therefore, you will need internet access for build.

* Open the solution (ThingSpaceConnectivityManagementAPI.sln) file.

Invoke the build process using Ctrl + Shift + B shortcut key or using the Build menu as shown below.

The build process generates a portable class library, which can be used like a normal class library. More information on how to use can be found at the MSDN Portable Class Libraries documentation.

The supported version is **.NET Standard 2.0**. For checking compatibility of your .NET implementation with the generated library, [click here](https://dotnet.microsoft.com/en-us/platform/dotnet-standard#versions).

## Installation

The following section explains how to use the ThingSpaceConnectivityManagementAPI.Standard library in a new project.

### 1. Starting a new project

For starting a new project, right click on the current solution from the solution explorer and choose `Add -> New Project`.

![Add a new project in Visual Studio](https://apidocs.io/illustration/cs?workspaceFolder=ThingSpace%20Connectivity%20Management%20API-CSharp&workspaceName=ThingSpaceConnectivityManagementAPI&projectName=ThingSpaceConnectivityManagementAPI.Standard&rootNamespace=ThingSpaceConnectivityManagementAPI.Standard&step=addProject)

Next, choose `Console Application`, provide `TestConsoleProject` as the project name and click OK.

![Create a new Console Application in Visual Studio](https://apidocs.io/illustration/cs?workspaceFolder=ThingSpace%20Connectivity%20Management%20API-CSharp&workspaceName=ThingSpaceConnectivityManagementAPI&projectName=ThingSpaceConnectivityManagementAPI.Standard&rootNamespace=ThingSpaceConnectivityManagementAPI.Standard&step=createProject)

### 2. Set as startup project

The new console project is the entry point for the eventual execution. This requires us to set the `TestConsoleProject` as the start-up project. To do this, right-click on the `TestConsoleProject` and choose `Set as StartUp Project` form the context menu.

![Adding a project reference](https://apidocs.io/illustration/cs?workspaceFolder=ThingSpace%20Connectivity%20Management%20API-CSharp&workspaceName=ThingSpaceConnectivityManagementAPI&projectName=ThingSpaceConnectivityManagementAPI.Standard&rootNamespace=ThingSpaceConnectivityManagementAPI.Standard&step=setStartup)

### 3. Add reference of the library project

In order to use the `ThingSpaceConnectivityManagementAPI.Standard` library in the new project, first we must add a project reference to the `TestConsoleProject`. First, right click on the `References` node in the solution explorer and click `Add Reference...`

![Adding a project reference](https://apidocs.io/illustration/cs?workspaceFolder=ThingSpace%20Connectivity%20Management%20API-CSharp&workspaceName=ThingSpaceConnectivityManagementAPI&projectName=ThingSpaceConnectivityManagementAPI.Standard&rootNamespace=ThingSpaceConnectivityManagementAPI.Standard&step=addReference)

Next, a window will be displayed where we must set the `checkbox` on `ThingSpaceConnectivityManagementAPI.Standard` and click `OK`. By doing this, we have added a reference of the `ThingSpaceConnectivityManagementAPI.Standard` project into the new `TestConsoleProject`.

![Creating a project reference](https://apidocs.io/illustration/cs?workspaceFolder=ThingSpace%20Connectivity%20Management%20API-CSharp&workspaceName=ThingSpaceConnectivityManagementAPI&projectName=ThingSpaceConnectivityManagementAPI.Standard&rootNamespace=ThingSpaceConnectivityManagementAPI.Standard&step=createReference)

### 4. Write sample code

Once the `TestConsoleProject` is created, a file named `Program.cs` will be visible in the solution explorer with an empty `Main` method. This is the entry point for the execution of the entire solution. Here, you can add code to initialize the client library and acquire the instance of a Controller class. Sample code to initialize the client library and using Controller methods is given in the subsequent sections.

![Adding a project reference](https://apidocs.io/illustration/cs?workspaceFolder=ThingSpace%20Connectivity%20Management%20API-CSharp&workspaceName=ThingSpaceConnectivityManagementAPI&projectName=ThingSpaceConnectivityManagementAPI.Standard&rootNamespace=ThingSpaceConnectivityManagementAPI.Standard&step=addCode)

## Test the SDK

The generated SDK also contain one or more Tests, which are contained in the Tests project. In order to invoke these test cases, you will need `NUnit 3.0 Test Adapter Extension` for Visual Studio. Once the SDK is complied, the test cases should appear in the Test Explorer window. Here, you can click `Run All` to execute these test cases.

## Initialize the API Client

**_Note:_** Documentation for the client can be found [here.](doc/client.md)

The following parameters are configurable for the API Client:

| Parameter | Type | Description |
|  --- | --- | --- |
| `VZM2MToken` | `string` | M2M Session Token |
| `Environment` | Environment | The API environment. <br> **Default: `Environment.Production`** |
| `Timeout` | `TimeSpan` | Http client timeout.<br>*Default*: `TimeSpan.FromSeconds(100)` |
| `OAuthClientId` | `string` | OAuth 2 Client ID |
| `OAuthClientSecret` | `string` | OAuth 2 Client Secret |
| `OAuthToken` | `Models.OAuthToken` | Object for storing information about the OAuth token |

The API client can be initialized as follows:

```csharp
ThingSpaceConnectivityManagementAPI.Standard.ThingSpaceConnectivityManagementAPIClient client = new ThingSpaceConnectivityManagementAPI.Standard.ThingSpaceConnectivityManagementAPIClient.Builder()
    .ClientCredentialsAuth("OAuthClientId", "OAuthClientSecret")
    .VZM2MToken("VZ-M2M-Token")
    .Environment(ThingSpaceConnectivityManagementAPI.Standard.Environment.Production)
    .HttpClientConfig(config => config.NumberOfRetries(0))
    .Build();
```

## Environments

The SDK can be configured to use a different environment for making API calls. Available environments are:

### Fields

| Name | Description |
|  --- | --- |
| production | **Default** |
| staging | - |

## Authorization

This API uses `OAuth 2 Client Credentials Grant`.

## Client Credentials Grant

Your application must obtain user authorization before it can execute an endpoint call in case this SDK chooses to use *OAuth 2.0 Client Credentials Grant*. This authorization includes the following steps

The `FetchToken()` method will exchange the OAuth client credentials for an *access token*. The access token is an object containing information for authorizing client requests and refreshing the token itself.

```csharp
var authManager = client.ClientCredentialsAuth;

try
{
    OAuthToken token = authManager.FetchToken();
    // re-instantiate the client with OAuth token
    client = client.ToBuilder().OAuthToken(token).Build();
}
catch (ApiException e)
{
    // TODO Handle exception
}
```

The client can now make authorized endpoint calls.

### Storing an access token for reuse

It is recommended that you store the access token for reuse.

```csharp
// store token
SaveTokenToDatabase(client.ClientCredentialsAuth.OAuthToken);
```

### Creating a client from a stored token

To authorize a client from a stored access token, just set the access token in Configuration along with the other configuration parameters before creating the client:

```csharp
// load token later
OAuthToken token = LoadTokenFromDatabase();

// Provide token along with other configuration properties while instantiating the client
ThingSpaceConnectivityManagementAPIClient client = client.ToBuilder().OAuthToken(token).Build();
```

### Complete example

```csharp
using ThingSpaceConnectivityManagementAPI.Standard;
using ThingSpaceConnectivityManagementAPI.Standard.Models;
using ThingSpaceConnectivityManagementAPI.Standard.Exceptions;
using ThingSpaceConnectivityManagementAPI.Standard.Authentication;
using System.Collections.Generic;
namespace OAuthTestApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            ThingSpaceConnectivityManagementAPI.Standard.ThingSpaceConnectivityManagementAPIClient client = new ThingSpaceConnectivityManagementAPI.Standard.ThingSpaceConnectivityManagementAPIClient.Builder()
                .ClientCredentialsAuth("OAuthClientId", "OAuthClientSecret")
                .VZM2MToken("VZ-M2M-Token")
                .Environment(ThingSpaceConnectivityManagementAPI.Standard.Environment.Production)
                .HttpClientConfig(config => config.NumberOfRetries(0))
                .Build();
            try
            {
                OAuthToken token = LoadTokenFromDatabase();

                // Set the token if it is not set before
                if (token == null)
                {
                    var authManager = client.ClientCredentialsAuth;
                    token = authManager.FetchToken();
                }

                SaveTokenToDatabase(token);
                client = client.ToBuilder().OAuthToken(token).Build();
            }
            catch (OAuthProviderException e)
            {
                // TODO Handle exception
            }
        }

        //function for storing token to database
        static void SaveTokenToDatabase(OAuthToken token)
        {
            //Save token here
        }

        //function for loading token from database
        static OAuthToken LoadTokenFromDatabase()
        {
            OAuthToken token = null;
            //token = Get token here
            return token;
        }
    }
}

// the client is now authorized and you can use controllers to make endpoint calls
```

## List of APIs

* [O Auth Authorization](doc/controllers/o-auth-authorization.md)
* [Session](doc/controllers/session.md)

## Classes Documentation

* [Utility Classes](doc/utility-classes.md)
* [HttpRequest](doc/http-request.md)
* [HttpResponse](doc/http-response.md)
* [HttpStringResponse](doc/http-string-response.md)
* [HttpContext](doc/http-context.md)
* [HttpClientConfiguration](doc/http-client-configuration.md)
* [HttpClientConfiguration Builder](doc/http-client-configuration-builder.md)
* [IAuthManager](doc/i-auth-manager.md)
* [ApiException](doc/api-exception.md)

