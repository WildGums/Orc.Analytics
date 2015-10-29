// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UserIdService.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2014 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Analytics
{
    using System;
    using System.Text;
    using SystemInfo;
    using Catel;
    using Catel.Logging;
    using Catel.Threading;

    public class UserIdService : IUserIdService
    {
        private readonly ISystemIdentificationService _systemIdentificationService;
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        private readonly object _lock = new object();
        private string _userId;

        public UserIdService(ISystemIdentificationService systemIdentificationService)
        {
            Argument.IsNotNull(() => systemIdentificationService);

            _systemIdentificationService = systemIdentificationService;
        }

        /// <summary>
        /// Gets the user identifier.
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

                var cpuId = string.Empty;
                var hddId = string.Empty;
                var macId = string.Empty;

                TaskHelper.RunAndWait(new Action[]
                {
                    // ORCOMP-203: Disable cpu because of performance
                    //() => cpuId = _systemIdentificationService.GetCpuId(),
                    () => hddId = _systemIdentificationService.GetHardDriveId(),
                    () => macId = _systemIdentificationService.GetMacId()
                });

                var uniqueIdPreValue = string.Format("{0}_{1}_{2}", cpuId, hddId, macId);
                var uniqueId = GetMd5Hash(uniqueIdPreValue);

                Log.Debug("Calculated user id '{0}'", uniqueId);

                _userId = uniqueId;

                return uniqueId;
            }
        }

        private string GetMd5Hash(string value)
        {
            var md5 = System.Security.Cryptography.MD5.Create();
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