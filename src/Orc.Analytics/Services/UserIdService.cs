// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UserIdService.cs" company="WildGums">
//   Copyright (c) 2008 - 2014 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Analytics
{
    using System;
    using System.Security.Cryptography;
    using System.Text;
    using Catel.Logging;

    public class UserIdService : IUserIdService
    {
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        private readonly object _lock = new object();
        private string _userId;

        /// <summary>
        ///     Gets the user identifier.
        /// </summary>
        /// <returns>System.String.</returns>
        public string GetUserId()
        {
            lock (_lock)
            {
                if (!string.IsNullOrWhiteSpace(_userId))
                {
                    return _userId;
                }

                Log.Debug("Calculating user id");

                _userId = Guid.NewGuid().ToString();

                Log.Debug("Calculated user id '{0}'", _userId);


                return _userId;
            }
        }

        private string GetMd5Hash(string value)
        {
            using (var md5 = MD5.Create())
            {
                var inputBytes = Encoding.ASCII.GetBytes(value);
                var hash = md5.ComputeHash(inputBytes);

                var sb = new StringBuilder();

                for (var i = 0; i < hash.Length; i++)
                {
                    sb.Append(hash[i].ToString("X2"));
                }

                return sb.ToString();
            }
        }
    }
}
