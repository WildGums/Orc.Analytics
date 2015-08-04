// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IUserIdService.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2014 Orchestra development team. All rights reserved.
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