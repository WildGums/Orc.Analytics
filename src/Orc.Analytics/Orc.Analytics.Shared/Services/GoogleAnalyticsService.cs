// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GoogleAnalyticsService.cs" company="CatenaLogic">
//   Copyright (c) 2008 - 2014 CatenaLogic. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Analytics
{
    using System;
    using System.Collections.Concurrent;
    using System.Globalization;
    using System.Threading.Tasks;
    using Auditors;
    using Catel;
    using Catel.Logging;
    using Catel.MVVM.Auditing;
    using Catel.Reflection;
    using GoogleAnalytics.Core;

    public class GoogleAnalyticsService : IGoogleAnalyticsService
    {
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        // ReSharper disable once NotAccessedField.Local
        private readonly AnalyticsAuditor _analyticsAuditor;
        private readonly IUserIdService _userIdService;

        private readonly ConcurrentQueue<Action> _queue = new ConcurrentQueue<Action>();

        private bool _isTrackerInitialized;

        private Tracker _tracker;
        private string _accountId;
        private string _userId;
        private string _appName;
        private string _appVersion;

        public GoogleAnalyticsService(IUserIdService userIdService)
        {
            Argument.IsNotNull(() => userIdService);

            _userIdService = userIdService;
            _analyticsAuditor = new AnalyticsAuditor(this);

            AuditingManager.RegisterAuditor(_analyticsAuditor);

            var entryAssembly = AssemblyHelper.GetEntryAssembly();
            AppName = entryAssembly.Product();
            AppVersion = entryAssembly.Version();

            IsEnabled = true;
        }

        public string AccountId
        {
            get { return _accountId; }
            set
            {
                _accountId = value;

                Tracker = null;
            }
        }

        public string UserId
        {
            get { return _userId; }
            set
            {
                _userId = value;

                Tracker = null;
            }
        }

        public string AppName
        {
            get { return _appName; }
            set
            {
                _appName = value;

                Tracker = null;
            }
        }

        public string AppVersion
        {
            get { return _appVersion; }
            set
            {
                _appVersion = value;

                Tracker = null;
            }
        }

        private Tracker Tracker
        {
            get { return _tracker; }
            set
            {
                _tracker = value;

                if (_tracker == null)
                {
                    _isTrackerInitialized = false;
                }
            }
        }

        public bool IsEnabled { get; set; }

        public async Task SendView(string viewName)
        {
            if (!IsEnabled)
            {
                return;
            }

            await Invoke(() =>
            {
                Log.Debug("Tracking view: {0}", viewName);

                _tracker.SendView(viewName);
            });
        }

        public async Task SendEvent(string category, string action, string label = null, long value = 0)
        {
            if (!IsEnabled)
            {
                return;
            }

            await Invoke(() =>
            {
                Log.Debug("Tracking event: {0} | {1} | {2} | {3}", category, action, label, value);

                _tracker.SendEvent(category, action, label, value);
            });
        }

        public async Task SendTransaction(string sku, string name, string transactionId, long costPerProduct, int quantity = 1)
        {
            if (!IsEnabled)
            {
                return;
            }

            var transaction = new Transaction(transactionId, costPerProduct * quantity);
            var item = new TransactionItem(sku, name, costPerProduct, quantity);
            transaction.Items.Add(item);

            await Invoke(() =>
            {
                Log.Debug("Tracking transaction: {0} | {1} | {2} | {3} | {4}", sku, name, transactionId, costPerProduct, quantity);

                _tracker.SendTransaction(transaction);
            });
        }

        public async Task SendTiming(TimeSpan time, string category, string variable, string label = "")
        {
            if (!IsEnabled)
            {
                return;
            }

            await Invoke(() =>
            {
                Log.Debug("Tracking timing: {0} | {1} | {2} | {3}", time, category, variable, label);

                _tracker.SendTiming(time, category, variable, label);
            });
        }

        public async Task Invoke(Action action)
        {
            _queue.Enqueue(action);

            if (!_isTrackerInitialized)
            {
                _isTrackerInitialized = true;

#pragma warning disable 4014
                InitializeTracker();
                return;
#pragma warning restore 4014
            }

            await Task.Factory.StartNew(() => SendTrackingsFromQueue());
        }

        private async Task InitializeTracker()
        {
            Log.Debug("Initializing tracker");

            if (string.IsNullOrWhiteSpace(AccountId))
            {
                Log.Warning("Account Id is null or whitespace, cannot create tracker");
                return;
            }

            if (string.IsNullOrWhiteSpace(UserId))
            {
                Log.Debug("User Id is null or whitespace, using the IUserIdService to retrieve the user id");

                _userId = await _userIdService.GetUserId();
            }

            var resolution = new Dimensions((int)System.Windows.SystemParameters.PrimaryScreenWidth,
                (int)System.Windows.SystemParameters.PrimaryScreenHeight);

            var trackerManager = new TrackerManager(new PlatformInfoProvider
            {
                AnonymousClientId = UserId,
                UserAgent = "Mozilla/5.0 (Windows NT 6.3; WOW64; Trident/7.0; rv:11.0) like Gecko",
                UserLanguage = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName,
                ScreenResolution = resolution,
                ViewPortResolution = resolution
            });

            var tracker = trackerManager.GetTracker(AccountId);
            tracker.AppName = AppName;
            tracker.AppVersion = AppVersion;

            _tracker = tracker;

            Log.Info("Initialized tracker, starting to empty the existing queue now");

            await Task.Factory.StartNew(() => SendTrackingsFromQueue());
        }

        private async Task SendTrackingsFromQueue()
        {
            var tracker = _tracker;
            if (tracker == null)
            {
                Log.Warning("No tracker available, cannot submit analytics. It might be possible the tracker is still initializing.");
                return;
            }

            while (_queue.Count > 0)
            {
                Action action;
                if (_queue.TryDequeue(out action))
                {
                    action();
                }
            }
        }
    }
}