// <copyright file="ThingSpaceConnectivityManagementAPIClient.cs" company="APIMatic">
// Copyright (c) APIMatic. All rights reserved.
// </copyright>
namespace ThingSpaceConnectivityManagementAPI.Standard
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Text;
    using ThingSpaceConnectivityManagementAPI.Standard.Authentication;
    using ThingSpaceConnectivityManagementAPI.Standard.Controllers;
    using ThingSpaceConnectivityManagementAPI.Standard.Http.Client;
    using ThingSpaceConnectivityManagementAPI.Standard.Utilities;

    /// <summary>
    /// The gateway for the SDK. This class acts as a factory for Controller and
    /// holds the configuration of the SDK.
    /// </summary>
    public sealed class ThingSpaceConnectivityManagementAPIClient : IConfiguration
    {
        // A map of environments and their corresponding servers/baseurls
        private static readonly Dictionary<Environment, Dictionary<Server, string>> EnvironmentsMap =
            new Dictionary<Environment, Dictionary<Server, string>>
        {
            {
                Environment.Production, new Dictionary<Server, string>
                {
                    { Server.Default, "https://thingspace.verizon.com/api/m2m/v1" },
                    { Server.OauthServer, "https://thingspace.verizon.com/api/ts/v1" },
                }
            },
            {
                Environment.Staging, new Dictionary<Server, string>
                {
                    { Server.Default, "https://staging.thingspace.verizon.com/api/m2m/v1" },
                    { Server.OauthServer, "https://staging.thingspace.verizon.com/api/ts/v1" },
                }
            },
        };

        private readonly IDictionary<string, IAuthManager> authManagers;
        private readonly IHttpClient httpClient;
        private readonly HttpCallBack httpCallBack;
        private readonly ClientCredentialsAuthManager clientCredentialsAuthManager;

        private readonly Lazy<SessionController> session;
        private readonly Lazy<OAuthAuthorizationController> oAuthAuthorization;

        private ThingSpaceConnectivityManagementAPIClient(
            string vZM2MToken,
            Environment environment,
            string oAuthClientId,
            string oAuthClientSecret,
            Models.OAuthToken oAuthToken,
            IDictionary<string, IAuthManager> authManagers,
            IHttpClient httpClient,
            HttpCallBack httpCallBack,
            IHttpClientConfiguration httpClientConfiguration)
        {
            this.VZM2MToken = vZM2MToken;
            this.Environment = environment;
            this.httpCallBack = httpCallBack;
            this.httpClient = httpClient;
            this.authManagers = (authManagers == null) ? new Dictionary<string, IAuthManager>() : new Dictionary<string, IAuthManager>(authManagers);
            this.HttpClientConfiguration = httpClientConfiguration;

            this.session = new Lazy<SessionController>(
                () => new SessionController(this, this.httpClient, this.authManagers, this.httpCallBack));
            this.oAuthAuthorization = new Lazy<OAuthAuthorizationController>(
                () => new OAuthAuthorizationController(this, this.httpClient, this.authManagers, this.httpCallBack));

            if (this.authManagers.ContainsKey("global"))
            {
                this.clientCredentialsAuthManager = (ClientCredentialsAuthManager)this.authManagers["global"];
            }

            if (!this.authManagers.ContainsKey("global")
                || !this.ClientCredentialsAuth.Equals(oAuthClientId, oAuthClientSecret, oAuthToken))
            {
                this.clientCredentialsAuthManager = new ClientCredentialsAuthManager(oAuthClientId, oAuthClientSecret, oAuthToken, this);
                this.authManagers["global"] = this.clientCredentialsAuthManager;
            }
        }

        /// <summary>
        /// Gets SessionController controller.
        /// </summary>
        public SessionController SessionController => this.session.Value;

        /// <summary>
        /// Gets OAuthAuthorizationController controller.
        /// </summary>
        public OAuthAuthorizationController OAuthAuthorizationController => this.oAuthAuthorization.Value;

        /// <summary>
        /// Gets the configuration of the Http Client associated with this client.
        /// </summary>
        public IHttpClientConfiguration HttpClientConfiguration { get; }

        /// <summary>
        /// Gets VZM2MToken.
        /// M2M Session Token.
        /// </summary>
        public string VZM2MToken { get; }

        /// <summary>
        /// Gets Environment.
        /// Current API environment.
        /// </summary>
        public Environment Environment { get; }

        /// <summary>
        /// Gets auth managers.
        /// </summary>
        internal IDictionary<string, IAuthManager> AuthManagers => this.authManagers;

        /// <summary>
        /// Gets http client.
        /// </summary>
        internal IHttpClient HttpClient => this.httpClient;

        /// <summary>
        /// Gets http callback.
        /// </summary>
        internal HttpCallBack HttpCallBack => this.httpCallBack;

        /// <summary>
        /// Gets the credentials to use with ClientCredentialsAuth.
        /// </summary>
        public IClientCredentialsAuth ClientCredentialsAuth => this.clientCredentialsAuthManager;

        /// <summary>
        /// Gets the URL for a particular alias in the current environment and appends
        /// it with template parameters.
        /// </summary>
        /// <param name="alias">Default value:DEFAULT.</param>
        /// <returns>Returns the baseurl.</returns>
        public string GetBaseUri(Server alias = Server.Default)
        {
            StringBuilder url = new StringBuilder(EnvironmentsMap[this.Environment][alias]);
            ApiHelper.AppendUrlWithTemplateParameters(url, this.GetBaseUriParameters());

            return url.ToString();
        }

        /// <summary>
        /// Creates an object of the ThingSpaceConnectivityManagementAPIClient using the values provided for the builder.
        /// </summary>
        /// <returns>Builder.</returns>
        public Builder ToBuilder()
        {
            Builder builder = new Builder()
                .VZM2MToken(this.VZM2MToken)
                .Environment(this.Environment)
                .OAuthToken(this.clientCredentialsAuthManager.OAuthToken)
                .ClientCredentialsAuth(this.clientCredentialsAuthManager.OAuthClientId, this.clientCredentialsAuthManager.OAuthClientSecret)
                .HttpCallBack(this.httpCallBack)
                .HttpClient(this.httpClient)
                .AuthManagers(this.authManagers)
                .HttpClientConfig(config => config.Build());

            return builder;
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            return
                $"VZM2MToken = {this.VZM2MToken}, " +
                $"Environment = {this.Environment}, " +
                $"HttpClientConfiguration = {this.HttpClientConfiguration}, ";
        }

        /// <summary>
        /// Creates the client using builder.
        /// </summary>
        /// <returns> ThingSpaceConnectivityManagementAPIClient.</returns>
        internal static ThingSpaceConnectivityManagementAPIClient CreateFromEnvironment()
        {
            var builder = new Builder();

            string vZM2MToken = System.Environment.GetEnvironmentVariable("THING_SPACE_CONNECTIVITY_MANAGEMENT_API_STANDARD_VZM_2_M_TOKEN");
            string environment = System.Environment.GetEnvironmentVariable("THING_SPACE_CONNECTIVITY_MANAGEMENT_API_STANDARD_ENVIRONMENT");
            string oAuthClientId = System.Environment.GetEnvironmentVariable("THING_SPACE_CONNECTIVITY_MANAGEMENT_API_STANDARD_O_AUTH_CLIENT_ID");
            string oAuthClientSecret = System.Environment.GetEnvironmentVariable("THING_SPACE_CONNECTIVITY_MANAGEMENT_API_STANDARD_O_AUTH_CLIENT_SECRET");

            if (vZM2MToken != null)
            {
                builder.VZM2MToken(vZM2MToken);
            }

            if (environment != null)
            {
                builder.Environment(ApiHelper.JsonDeserialize<Environment>($"\"{environment}\""));
            }

            if (oAuthClientId != null && oAuthClientSecret != null)
            {
                builder.ClientCredentialsAuth(oAuthClientId, oAuthClientSecret);
            }

            return builder.Build();
        }

        /// <summary>
        /// Makes a list of the BaseURL parameters.
        /// </summary>
        /// <returns>Returns the parameters list.</returns>
        private List<KeyValuePair<string, object>> GetBaseUriParameters()
        {
            List<KeyValuePair<string, object>> kvpList = new List<KeyValuePair<string, object>>()
            {
            };
            return kvpList;
        }

        /// <summary>
        /// Builder class.
        /// </summary>
        public class Builder
        {
            private string vZM2MToken = String.Empty;
            private Environment environment = ThingSpaceConnectivityManagementAPI.Standard.Environment.Production;
            private string oAuthClientId = "";
            private string oAuthClientSecret = "";
            private Models.OAuthToken oAuthToken = null;
            private IDictionary<string, IAuthManager> authManagers = new Dictionary<string, IAuthManager>();
            private HttpClientConfiguration.Builder httpClientConfig = new HttpClientConfiguration.Builder();
            private IHttpClient httpClient;
            private HttpCallBack httpCallBack;

            /// <summary>
            /// Sets credentials for ClientCredentialsAuth.
            /// </summary>
            /// <param name="oAuthClientId">OAuthClientId.</param>
            /// <param name="oAuthClientSecret">OAuthClientSecret.</param>
            /// <returns>Builder.</returns>
            public Builder ClientCredentialsAuth(string oAuthClientId, string oAuthClientSecret)
            {
                this.oAuthClientId = oAuthClientId ?? throw new ArgumentNullException(nameof(oAuthClientId));
                this.oAuthClientSecret = oAuthClientSecret ?? throw new ArgumentNullException(nameof(oAuthClientSecret));
                return this;
            }

            /// <summary>
            /// Sets OAuthToken.
            /// </summary>
            /// <param name="oAuthToken">OAuthToken.</param>
            /// <returns>Builder.</returns>
            public Builder OAuthToken(Models.OAuthToken oAuthToken)
            {
                this.oAuthToken = oAuthToken;
                return this;
            }

            /// <summary>
            /// Sets VZM2MToken.
            /// </summary>
            /// <param name="vZM2MToken"> VZM2MToken. </param>
            /// <returns> Builder. </returns>
            public Builder VZM2MToken(string vZM2MToken)
            {
                this.vZM2MToken = vZM2MToken ?? throw new ArgumentNullException(nameof(vZM2MToken));
                return this;
            }

            /// <summary>
            /// Sets Environment.
            /// </summary>
            /// <param name="environment"> Environment. </param>
            /// <returns> Builder. </returns>
            public Builder Environment(Environment environment)
            {
                this.environment = environment;
                return this;
            }

            /// <summary>
            /// Sets HttpClientConfig.
            /// </summary>
            /// <param name="action"> Action. </param>
            /// <returns>Builder.</returns>
            public Builder HttpClientConfig(Action<HttpClientConfiguration.Builder> action)
            {
                if (action is null)
                {
                    throw new ArgumentNullException(nameof(action));
                }

                action(this.httpClientConfig);
                return this;
            }

            /// <summary>
            /// Sets the IHttpClient for the Builder.
            /// </summary>
            /// <param name="httpClient"> http client. </param>
            /// <returns>Builder.</returns>
            internal Builder HttpClient(IHttpClient httpClient)
            {
                this.httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
                return this;
            }

            /// <summary>
            /// Sets the authentication managers for the Builder.
            /// </summary>
            /// <param name="authManagers"> auth managers. </param>
            /// <returns>Builder.</returns>
            internal Builder AuthManagers(IDictionary<string, IAuthManager> authManagers)
            {
                this.authManagers = authManagers ?? throw new ArgumentNullException(nameof(authManagers));
                return this;
            }

            /// <summary>
            /// Sets the HttpCallBack for the Builder.
            /// </summary>
            /// <param name="httpCallBack"> http callback. </param>
            /// <returns>Builder.</returns>
            internal Builder HttpCallBack(HttpCallBack httpCallBack)
            {
                this.httpCallBack = httpCallBack;
                return this;
            }

            /// <summary>
            /// Creates an object of the ThingSpaceConnectivityManagementAPIClient using the values provided for the builder.
            /// </summary>
            /// <returns>ThingSpaceConnectivityManagementAPIClient.</returns>
            public ThingSpaceConnectivityManagementAPIClient Build()
            {
                this.httpClient = new HttpClientWrapper(this.httpClientConfig.Build());

                return new ThingSpaceConnectivityManagementAPIClient(
                    this.vZM2MToken,
                    this.environment,
                    this.oAuthClientId,
                    this.oAuthClientSecret,
                    this.oAuthToken,
                    this.authManagers,
                    this.httpClient,
                    this.httpCallBack,
                    this.httpClientConfig.Build());
            }
        }
    }
}
