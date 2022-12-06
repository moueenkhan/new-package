
# Client Class Documentation

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

## ThingSpace Connectivity Management APIClient Class

The gateway for the SDK. This class acts as a factory for the Controllers and also holds the configuration of the SDK.

### Controllers

| Name | Description |
|  --- | --- |
| SessionController | Gets SessionController controller. |
| OAuthAuthorizationController | Gets OAuthAuthorizationController controller. |

### Properties

| Name | Description | Type |
|  --- | --- | --- |
| Auth | Gets the AuthManager. | `AuthManager` |
| HttpClientConfiguration | Gets the configuration of the Http Client associated with this client. | [`IHttpClientConfiguration`](http-client-configuration.md) |
| Timeout | Http client timeout. | `TimeSpan` |
| VZM2MToken | M2M Session Token | `string` |
| Environment | Current API environment. | `Environment` |

### Methods

| Name | Description | Return Type |
|  --- | --- | --- |
| `GetBaseUri(Server alias = Server.Default)` | Gets the URL for a particular alias in the current environment and appends it with template parameters. | `string` |
| `ToBuilder()` | Creates an object of the ThingSpace Connectivity Management APIClient using the values provided for the builder. | `Builder` |

## ThingSpace Connectivity Management APIClient Builder Class

Class to build instances of ThingSpace Connectivity Management APIClient.

### Methods

| Name | Description | Return Type |
|  --- | --- | --- |
| `Auth(AuthManager auth)` | Gets the AuthManager. | `Builder` |
| `HttpClientConfiguration(Action<`[`HttpClientConfiguration.Builder`](http-client-configuration-builder.md)`> action)` | Gets the configuration of the Http Client associated with this client. | `Builder` |
| `Timeout(TimeSpan timeout)` | Http client timeout. | `Builder` |
| `VZM2MToken(string vZM2MToken)` | M2M Session Token | `Builder` |
| `Environment(Environment environment)` | Current API environment. | `Builder` |

