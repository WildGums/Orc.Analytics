namespace Orc.Analytics
{
    using System;
    using Catel.Logging;

    public class UserIdService : IUserIdService
    {
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        private readonly object _lock = new object();
        private string? _userId;

        /// <summary>
        ///     Gets the user identifier.
        /// </summary>
        /// <returns>System.String.</returns>
        public virtual string GetUserId()
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
    }
}
