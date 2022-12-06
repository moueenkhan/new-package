// <copyright file="ClientCredentialsAuthManager.cs" company="APIMatic">
// Copyright (c) APIMatic. All rights reserved.
// </copyright>
namespace ThingSpaceConnectivityManagementAPI.Standard.Authentication
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;
    using ThingSpaceConnectivityManagementAPI.Standard.Controllers;
    using ThingSpaceConnectivityManagementAPI.Standard.Http.Request;
    using ThingSpaceConnectivityManagementAPI.Standard.Models;
    using ThingSpaceConnectivityManagementAPI.Standard.Utilities;
    using ThingSpaceConnectivityManagementAPI.Standard.Exceptions;

    /// <summary>
    /// ClientCredentialsAuthManager Class.
    /// </summary>
    public class ClientCredentialsAuthManager : IClientCredentialsAuth, IAuthManager
    {
        private readonly OAuthAuthorizationController oAuthApi;

        /// <summary>
        /// Initializes a new instance of the <see cref="ClientCredOAuthManager"/> class.
        /// </summary>
        /// <param name="oAuthClientId"> OAuth 2 Client ID.</param>
        /// <param name="oAuthClientSecret"> OAuth 2 Client Secret.</param>
        /// <param name="oAuthToken"> OAuth 2 token.</param>
        /// <param name="client"> Instance of ThingSpaceConnectivityManagementAPIClient.</param>
        public ClientCredentialsAuthManager(
            string oAuthClientId,
            string oAuthClientSecret,
            OAuthToken oAuthToken,
            ThingSpaceConnectivityManagementAPIClient client)
        {
            this.OAuthClientId = oAuthClientId;
            this.OAuthClientSecret = oAuthClientSecret;
            this.OAuthToken = oAuthToken;
            this.oAuthApi = new OAuthAuthorizationController(client, client.HttpClient, client.AuthManagers);
        }

        /// <summary>
        /// Gets string value for oAuthClientId.
        /// </summary>
        public string OAuthClientId { get; }

        /// <summary>
        /// Gets string value for oAuthClientSecret.
        /// </summary>
        public string OAuthClientSecret { get; }

        /// <summary>
        /// Gets Models.OAuthToken value for oAuthToken.
        /// </summary>
        public Models.OAuthToken OAuthToken { get; }

        /// <summary>
        /// Check if credentials match.
        /// </summary>
        /// <param name="oAuthClientId"> The string value for credentials.</param>
        /// <param name="oAuthClientSecret"> The string value for credentials.</param>
        /// <param name="oAuthToken"> The Models.OAuthToken value for credentials.</param>
        /// <returns> True if credentials matched.</returns>
        public bool Equals(string oAuthClientId, string oAuthClientSecret, Models.OAuthToken oAuthToken)
        {
            return oAuthClientId.Equals(this.OAuthClientId)
                    && oAuthClientSecret.Equals(this.OAuthClientSecret)
                    && ((oAuthToken == null && this.OAuthToken == null) || (oAuthToken != null && this.OAuthToken != null && oAuthToken.Equals(this.OAuthToken)));
        }

        /// <summary>
        /// Checks if token is expired.
        /// </summary>
        /// <returns> Returns true if token is expired.</returns>
        public bool IsTokenExpired()
        {
           if (this.OAuthToken == null)
           {
               throw new InvalidOperationException("OAuth token is missing.");
           }
        
           return this.OAuthToken.Expiry != null
               && this.OAuthToken.Expiry < (long)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
        }

        /// <summary>
        /// Add authentication information to the HTTP Request.
        /// </summary>
        /// <param name="httpRequest">The http request object on which authentication will be applied.</param>
        /// <exception cref="ApiException">Thrown when OAuthToken is null or expired.</exception>
        /// <returns>HttpRequest.</returns>
        public HttpRequest Apply(HttpRequest httpRequest)
        {
            Task<HttpRequest> t = this.ApplyAsync(httpRequest);
            ApiHelper.RunTaskSynchronously(t);
            return t.Result;
        }

        /// <summary>
        /// Asynchronously add authentication information to the HTTP Request.
        /// </summary>
        /// <param name="httpRequest">The http request object on which authentication will be applied.</param>
        /// <exception cref="ApiException">Thrown when OAuthToken is null or expired.</exception>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public Task<HttpRequest> ApplyAsync(HttpRequest httpRequest)
        {
            return Task<HttpRequest>.Factory.StartNew(() =>
            {
                CheckAuthorization();
                httpRequest.Headers["Authorization"] = $"Bearer {this.OAuthToken.AccessToken}";
                return httpRequest;
            });
        }

        /// <summary>
        /// Fetch the OAuth token.
        /// </summary>
        /// <param name="additionalParameters">Dictionary of additional parameters.</param>
        /// <returns>Models.OAuthToken.</returns>
        public Models.OAuthToken FetchToken(Dictionary<string, object> additionalParameters = null)
        {
            Task<Models.OAuthToken> t = this.FetchTokenAsync(additionalParameters);
            ApiHelper.RunTaskSynchronously(t);
            return t.Result;
        }

        /// <summary>
        /// Fetch the OAuth token asynchronously.
        /// </summary>
        /// <param name="additionalParameters">Dictionary of additional parameters.</param>
        /// <returns>Models.OAuthToken.</returns>
        public async Task<Models.OAuthToken> FetchTokenAsync(Dictionary<string, object> additionalParameters = null)
        {
            Models.OAuthToken token = await oAuthApi.RequestTokenAsync(BuildBasicAuthheader(),
                fieldParameters: additionalParameters);

            if (token.ExpiresIn != null && token.ExpiresIn != 0)
            {
                token.Expiry = (long)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds + token.ExpiresIn;
            }

            return token;
        }

        /// <summary>
        /// Build basic auth header.
        /// </summary>
        /// <returns> string. </returns>
        private string BuildBasicAuthheader()
        {
            var plainTextBytes = Encoding.UTF8.GetBytes(this.OAuthClientId + ':' + this.OAuthClientSecret);
            return "Basic " + Convert.ToBase64String(plainTextBytes);
        }

        /// <summary>
        /// Checks if client is authorized.
        /// </summary>
        private void CheckAuthorization()
        {
            if (this.OAuthToken == null)
            {
                throw new ApiException(
                        "Client is not authorized. An OAuth token is needed to make API calls.");
            }

            if (IsTokenExpired())
            {
                throw new ApiException(
                        "OAuth token is expired. A valid token is needed to make API calls.");
            }
        }
    }
}