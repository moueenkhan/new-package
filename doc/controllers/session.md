# Session

Start and end Connectivity Management sessions.

```csharp
SessionController sessionController = client.SessionController;
```

## Class Name

`SessionController`


# Login Using POST

Initiates a Connectivity Management session and returns a VZ-M2M session token that is required in subsequent API requests.

```csharp
LoginUsingPOSTAsync(
    Models.LogInRequest body = null)
```

## Parameters

| Parameter | Type | Tags | Description |
|  --- | --- | --- | --- |
| `body` | [`Models.LogInRequest`](../../doc/models/log-in-request.md) | Body, Optional | request |

## Response Type

[`Task<Models.LogInResponse>`](../../doc/models/log-in-response.md)

## Example Usage

```csharp
var body = new LogInRequest();
body.Username = "zbeeblebrox";
body.Password = "IMgr8";

try
{
    LogInResponse result = await sessionController.LoginUsingPOSTAsync(body);
}
catch (ApiException e){};
```

## Example Response *(as JSON)*

```json
{
  "sessionToken": "bcce3ea6-fe4f-4952-bacf-eadd80718e83"
}
```

## Errors

| HTTP Status Code | Error Description | Exception Class |
|  --- | --- | --- |
| 400 | Error Response | [`RestErrorResponseException`](../../doc/models/rest-error-response-exception.md) |

