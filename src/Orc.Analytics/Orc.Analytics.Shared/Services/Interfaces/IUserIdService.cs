// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IUserIdService.cs" company="WildGums">
//   Copyright (c) 2008 - 2014 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Analytics
{
    public interface IUserIdService
    {
        /// <summary>
        /// Gets the user identifier.
        /// </summary>
        /// <returns>System.String.</returns>
        string GetUserId();
    }
}