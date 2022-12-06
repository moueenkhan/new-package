// <copyright file="SessionControllerTest.cs" company="APIMatic">
// Copyright (c) APIMatic. All rights reserved.
// </copyright>
namespace ThingSpaceConnectivityManagementAPI.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Threading.Tasks;
    using Newtonsoft.Json.Converters;
    using NUnit.Framework;
    using ThingSpaceConnectivityManagementAPI.Standard;
    using ThingSpaceConnectivityManagementAPI.Standard.Controllers;
    using ThingSpaceConnectivityManagementAPI.Standard.Exceptions;
    using ThingSpaceConnectivityManagementAPI.Standard.Http.Client;
    using ThingSpaceConnectivityManagementAPI.Standard.Http.Response;
    using ThingSpaceConnectivityManagementAPI.Standard.Utilities;
    using ThingSpaceConnectivityManagementAPI.Tests.Helpers;

    /// <summary>
    /// SessionControllerTest.
    /// </summary>
    [TestFixture]
    public class SessionControllerTest : ControllerTestBase
    {
        /// <summary>
        /// Controller instance (for all tests).
        /// </summary>
        private SessionController controller;

        /// <summary>
        /// Setup test class.
        /// </summary>
        [OneTimeSetUp]
        public void SetUpDerived()
        {
            this.controller = this.Client.SessionController;
        }

        /// <summary>
        /// Initiates a Connectivity Management session and returns a VZ-M2M session token that is required in subsequent API requests..
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Test]
        public async Task TestTestLoginUsingPOST()
        {
            // Parameters for the API call
            Standard.Models.LogInRequest body = ApiHelper.JsonDeserialize<Standard.Models.LogInRequest>("{\"username\":\"zbeeblebrox\",\"password\":\"IMgr8\"}");

            // Perform API call
            Standard.Models.LogInResponse result = null;
            try
            {
                result = await this.controller.LoginUsingPOSTAsync(body);
            }
            catch (ApiException)
            {
            }

            // Test response code
            Assert.AreEqual(200, this.HttpCallBackHandler.Response.StatusCode, "Status should be 200");

            // Test headers
            Dictionary<string, string> headers = new Dictionary<string, string>();
            headers.Add("Content-Type", "application/json");

            Assert.IsTrue(
                    TestHelper.AreHeadersProperSubsetOf (
                    headers,
                    this.HttpCallBackHandler.Response.Headers),
                    "Headers should match");

            // Test whether the captured response is as we expected
            Assert.IsNotNull(result, "Result should exist");
            Assert.IsTrue(
                    TestHelper.IsJsonObjectProperSubsetOf(
                    "{\"sessionToken\":\"bcce3ea6-fe4f-4952-bacf-eadd80718e83\"}",
                    TestHelper.ConvertStreamToString(this.HttpCallBackHandler.Response.RawBody),
                    false,
                    true,
                    false),
                    "Response body should have matching keys");
        }
    }
}