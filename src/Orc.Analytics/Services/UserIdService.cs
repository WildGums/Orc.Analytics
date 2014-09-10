// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UserIdService.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2014 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Analytics
{
    using System;
    using System.Management;
    using System.Text;
    using System.Threading.Tasks;
    using Catel.Logging;

    public class UserIdService : IUserIdService
    {
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        private string _userId;

        /// <summary>
        /// Gets the user identifier.
        /// </summary>
        /// <returns>System.String.</returns>
        public async Task<string> GetUserId()
        {
            if (!string.IsNullOrWhiteSpace(_userId))
            {
                return _userId;
            }

            return await Task.Factory.StartNew(() =>
            {
                Log.Debug("Calculating user id");

                var cpuId = GetCpuId();
                var hddId = GetHddId();

                var uniqueIdPreValue = string.Format("{0}_{1}", cpuId, hddId);
                var uniqueId = GetMd5Hash(uniqueIdPreValue);

                Log.Debug("Calculated user id '{0}'", uniqueId);

                _userId = uniqueId;

                return uniqueId;
            });
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

        private string GetCpuId()
        {
            Log.Debug("Retrieving CPU id");

            var cpuId = string.Empty;

            try
            {
                var managementClass = new ManagementClass("win32_processor");
                var managementObjectCollection = managementClass.GetInstances();

                foreach (var managementObject in managementObjectCollection)
                {
                    cpuId = managementObject.Properties["processorID"].Value.ToString();
                    break;
                }

                Log.Debug("Retrieved CPU id '{0}'", cpuId);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Failed to retrieve CPU id");
            }

            return cpuId;
        }

        private string GetHddId()
        {
            Log.Debug("Retrieving HDD id");

            var hddId = string.Empty;

            try
            {
                const string drive = "C";
                var disk = new ManagementObject(@"win32_logicaldisk.deviceid=""" + drive + @":""");
                disk.Get();

                hddId = disk["VolumeSerialNumber"].ToString();

                Log.Debug("Retrieved HDD id '{0}'", hddId);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Failed to retrieve HDD id");
            }

            return hddId;
        }
    }
}