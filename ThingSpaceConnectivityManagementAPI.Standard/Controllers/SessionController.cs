// <copyright file="SessionController.cs" company="APIMatic">
// Copyright (c) APIMatic. All rights reserved.
// </copyright>
namespace ThingSpaceConnectivityManagementAPI.Standard.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Dynamic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using Newtonsoft.Json.Converters;
    using ThingSpaceConnectivityManagementAPI.Standard;
    using ThingSpaceConnectivityManagementAPI.Standard.Authentication;
    using ThingSpaceConnectivityManagementAPI.Standard.Exceptions;
    using ThingSpaceConnectivityManagementAPI.Standard.Http.Client;
    using ThingSpaceConnectivityManagementAPI.Standard.Http.Request;
    using ThingSpaceConnectivityManagementAPI.Standard.Http.Request.Configuration;
    using ThingSpaceConnectivityManagementAPI.Standard.Http.Response;
    using ThingSpaceConnectivityManagementAPI.Standard.Utilities;

    /// <summary>
    /// SessionController.
    /// </summary>
    public class SessionController : BaseController
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SessionController"/> class.
        /// </summary>
        /// <param name="config"> config instance. </param>
        /// <param name="httpClient"> httpClient. </param>
        /// <param name="authManagers"> authManager. </param>
        /// <param name="httpCallBack"> httpCallBack. </param>
        internal SessionController(IConfiguration config, IHttpClient httpClient, IDictionary<string, IAuthManager> authManagers, HttpCallBack httpCallBack = null)
            : base(config, httpClient, authManagers, httpCallBack)
        {
        }

        /// <summary>
        /// Initiates a Connectivity Management session and returns a VZ-M2M session token that is required in subsequent API requests.
        /// </summary>
        /// <param name="body">Optional parameter: request.</param>
        /// <returns>Returns the Models.LogInResponse response from the API call.</returns>
        public Models.LogInResponse LoginUsingPOST(
                Models.LogInRequest body = null)
        {
            Task<Models.LogInResponse> t = this.LoginUsingPOSTAsync(body);
            ApiHelper.RunTaskSynchronously(t);
            return t.Result;
        }

        /// <summary>
        /// Initiates a Connectivity Management session and returns a VZ-M2M session token that is required in subsequent API requests.
        /// </summary>
        /// <param name="body">Optional parameter: request.</param>
        /// <param name="cancellationToken"> cancellationToken. </param>
        /// <returns>Returns the Models.LogInResponse response from the API call.</returns>
        public async Task<Models.LogInResponse> LoginUsingPOSTAsync(
                Models.LogInRequest body = null,
                CancellationToken cancellationToken = default)
        {
            // the base uri for api requests.
            string baseUri = this.Config.GetBaseUri();

            // prepare query string for API call.
            StringBuilder queryBuilder = new StringBuilder(baseUri);
            queryBuilder.Append("/session/login");

            // append request with appropriate headers and parameters
            var headers = new Dictionary<string, string>()
            {
                { "user-agent", this.UserAgent },
                { "accept", "application/json" },
                { "Content-Type", "application/json" },
                { "VZ-M2M-Token", this.Config.VZM2MToken },
            };

            // append body params.
            var bodyText = ApiHelper.JsonSerialize(body);

            // prepare the API call request to fetch the response.
            HttpRequest httpRequest = this.GetClientInstance().PostBody(queryBuilder.ToString(), headers, bodyText);

            if (this.HttpCallBack != null)
            {
                this.HttpCallBack.OnBeforeHttpRequestEventHandler(this.GetClientInstance(), httpRequest);
            }

            httpRequest = await this.AuthManagers["global"].ApplyAsync(httpRequest).ConfigureAwait(false);

            // invoke request and get response.
            HttpStringResponse response = await this.GetClientInstance().ExecuteAsStringAsync(httpRequest, cancellationToken: cancellationToken).ConfigureAwait(false);
            HttpContext context = new HttpContext(httpRequest, response);
            if (this.HttpCallBack != null)
            {
                this.HttpCallBack.OnAfterHttpResponseEventHandler(this.GetClientInstance(), response);
            }

            if (response.StatusCode == 400)
            {
                throw new RestErrorResponseException("Error Response", context);
            }

            // handle errors defined at the API level.
            this.ValidateResponse(response, context);

            return ApiHelper.JsonDeserialize<Models.LogInResponse>(response.Body);
        }
    }
}