// <copyright file="LogInResponse.cs" company="APIMatic">
// Copyright (c) APIMatic. All rights reserved.
// </copyright>
namespace ThingSpaceConnectivityManagementAPI.Standard.Models
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
    using ThingSpaceConnectivityManagementAPI.Standard.Utilities;

    /// <summary>
    /// LogInResponse.
    /// </summary>
    public class LogInResponse
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LogInResponse"/> class.
        /// </summary>
        public LogInResponse()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LogInResponse"/> class.
        /// </summary>
        /// <param name="sessionToken">sessionToken.</param>
        public LogInResponse(
            string sessionToken = null)
        {
            this.SessionToken = sessionToken;
        }

        /// <summary>
        /// The identifier for the session that was created by the request. Store the sessionToken for use in the header of all other API requests.
        /// </summary>
        [JsonProperty("sessionToken", NullValueHandling = NullValueHandling.Ignore)]
        public string SessionToken { get; set; }

        /// <inheritdoc/>
        public override string ToString()
        {
            var toStringOutput = new List<string>();

            this.ToString(toStringOutput);

            return $"LogInResponse : ({string.Join(", ", toStringOutput)})";
        }

        /// <inheritdoc/>
        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            if (obj == this)
            {
                return true;
            }

            return obj is LogInResponse other &&
                ((this.SessionToken == null && other.SessionToken == null) || (this.SessionToken?.Equals(other.SessionToken) == true));
        }
        

        /// <summary>
        /// ToString overload.
        /// </summary>
        /// <param name="toStringOutput">List of strings.</param>
        protected void ToString(List<string> toStringOutput)
        {
            toStringOutput.Add($"this.SessionToken = {(this.SessionToken == null ? "null" : this.SessionToken == string.Empty ? "" : this.SessionToken)}");
        }
    }
}