// <copyright file="ControllerTestBase.cs" company="APIMatic">
// Copyright (c) APIMatic. All rights reserved.
// </copyright>
namespace ThingSpaceConnectivityManagementAPI.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using NUnit.Framework;
    using ThingSpaceConnectivityManagementAPI.Standard;
    using ThingSpaceConnectivityManagementAPI.Standard.Exceptions;
    using ThingSpaceConnectivityManagementAPI.Standard.Http.Client;
    using ThingSpaceConnectivityManagementAPI.Standard.Models;
    using ThingSpaceConnectivityManagementAPI.Tests.Helpers;

    /// <summary>
    /// ControllerTestBase Class.
    /// </summary>
    [TestFixture]
    public class ControllerTestBase
    {
        /// <summary>
        /// Assert precision.
        /// </summary>
        protected const double AssertPrecision = 0.1;

        /// <summary>
        /// Gets HttpCallBackHandler.
        /// </summary>
        internal HttpCallBack HttpCallBackHandler { get; private set; }

        /// <summary>
        /// Gets ThingSpaceConnectivityManagementAPIClient Client.
        /// </summary>
        protected ThingSpaceConnectivityManagementAPIClient Client { get; private set; }

        /// <summary>
        /// Set up the client.
        /// </summary>
        [OneTimeSetUp]
        public void SetUp()
        {
            ThingSpaceConnectivityManagementAPIClient config = ThingSpaceConnectivityManagementAPIClient.CreateFromEnvironment();
            this.HttpCallBackHandler = new HttpCallBack();
            this.Client = config.ToBuilder()
                .HttpCallBack(this.HttpCallBackHandler)
                .Build();

            try
            {
                this.Client = this.Client.ToBuilder().OAuthToken(this.Client.ClientCredentialsAuth.FetchToken())
                        .Build();
            }
            catch (ApiException) 
            {
                // TODO Auto-generated catch block;
            }
        }
    }
}