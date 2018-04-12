// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PublicApiFacts.cs" company="WildGums">
//   Copyright (c) 2008 - 2017 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Analytics.Tests
{
    using ApiApprover;
    using Auditors;
    using NUnit.Framework;

    [TestFixture]
    public class PublicApiFacts
    {
        [Test]
        public void Orc_Analytics_HasNoBreakingChanges()
        {
            var assembly = typeof(AnalyticsAuditor).Assembly;

            PublicApiApprover.ApprovePublicApi(assembly);
        }
    }
}