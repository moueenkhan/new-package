# O Auth Authorization

```csharp
OAuthAuthorizationController oAuthAuthorizationController = client.OAuthAuthorizationController;
```

## Class Name

`OAuthAuthorizationController`


# Request Token

Create a new OAuth 2 token.

:information_source: **Note** This endpoint does not require authentication.

```csharp
RequestTokenAsync(
    string authorization,
    string scope = null,
    Dictionary<string, object> fieldParameters = null)
```

## Parameters

| Parameter | Type | Tags | Description |
|  --- | --- | --- | --- |
| `authorization` | `string` | Header, Required | Authorization header in Basic auth format |
| `scope` | `string` | Form, Optional | Requested scopes as a space-delimited list. |
| `fieldParameters` | `Dictionary<string, object>` | Optional | Pass additional field parameters. |

## Response Type

[`Task<Models.OAuthToken>`](../../doc/models/o-auth-token.md)

## Example Usage

```csharp
string authorization = "Authorization8";
var fieldParameters = new Dictionary<string, Object>();
fieldParameters.Add("key0", "additionalFieldParams9");

try
{
    OAuthToken result = await oAuthAuthorizationController.RequestTokenAsync(authorization, null, fieldParameters);
}
catch (ApiException e){};
```

## Errors

| HTTP Status Code | Error Description | Exception Class |
|  --- | --- | --- |
| 400 | OAuth 2 provider returned an error. | [`OAuthProviderException`](../../doc/models/o-auth-provider-exception.md) |
| 401 | OAuth 2 provider says client authentication failed. | [`OAuthProviderException`](../../doc/models/o-auth-provider-exception.md) |

