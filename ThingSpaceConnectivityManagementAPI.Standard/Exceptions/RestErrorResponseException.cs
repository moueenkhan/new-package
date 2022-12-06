// <copyright file="RestErrorResponseException.cs" company="APIMatic">
// Copyright (c) APIMatic. All rights reserved.
// </copyright>
namespace ThingSpaceConnectivityManagementAPI.Standard.Exceptions
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;
    using ThingSpaceConnectivityManagementAPI.Standard;
    using ThingSpaceConnectivityManagementAPI.Standard.Http.Client;
    using ThingSpaceConnectivityManagementAPI.Standard.Models;
    using ThingSpaceConnectivityManagementAPI.Standard.Utilities;

    /// <summary>
    /// RestErrorResponseException.
    /// </summary>
    public class RestErrorResponseException : ApiException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RestErrorResponseException"/> class.
        /// </summary>
        /// <param name="reason"> The reason for throwing exception.</param>
        /// <param name="context"> The HTTP context that encapsulates request and response objects.</param>
        public RestErrorResponseException(string reason, HttpContext context)
            : base(reason, context)
        {
        }

        /// <summary>
        /// Code of the error.
        /// </summary>
        [JsonProperty("errorCode", NullValueHandling = NullValueHandling.Ignore)]
        public string ErrorCode { get; set; }

        /// <summary>
        /// Details of the error.
        /// </summary>
        [JsonProperty("errorMessage", NullValueHandling = NullValueHandling.Ignore)]
        public string ErrorMessage { get; set; }
    }
}