// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GoogleAnalyticsService.cs" company="CatenaLogic">
//   Copyright (c) 2008 - 2014 CatenaLogic. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Analytics
{
    using System;
    using System.Globalization;
    using System.Threading.Tasks;
    using Auditors;
    using Catel;
    using Catel.Logging;
    using Catel.Reflection;
    using GoogleAnalytics.Core;

    public class GoogleAnalyticsService : IGoogleAnalyticsService
    {
        private readonly IUserIdService _userIdService;
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        private readonly AnalyticsAuditor _analyticsAuditor;

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
                _tracker = null;
            }
        }

        public string UserId
        {
            get { return _userId; }
            set
            {
                _userId = value;
                _tracker = null;
            }
        }

        public string AppName
        {
            get { return _appName; }
            set
            {
                _appName = value;
                _tracker = null;
            }
        }

        public string AppVersion
        {
            get { return _appVersion; }
            set
            {
                _appVersion = value;
                _tracker = null;
            }
        }

        public bool IsEnabled { get; set; }

        public async Task SendView(string viewName)
        {
            if (!IsEnabled)
            {
                return;
            }

            Log.Debug("Tracking view: {0}", viewName);

            await Invoke(() => _tracker.SendView(viewName));
        }

        public async Task SendEvent(string category, string action, string label = null, long value = 0)
        {
            if (!IsEnabled)
            {
                return;
            }

            Log.Debug("Tracking event: {0} | {1} | {2} | {3}", category, action, label, value);

            Invoke(() => _tracker.SendEvent(category, action, label, value));
        }

        public async Task SendTransaction(string sku, string name, string transactionId, long costPerProduct, int quantity = 1)
        {
            if (!IsEnabled)
            {
                return;
            }

            Log.Debug("Tracking transaction: {0} | {1} | {2} | {3} | {4}", sku, name, transactionId, costPerProduct, quantity);

            var transaction = new Transaction(transactionId, costPerProduct * quantity);
            var item = new TransactionItem(sku, name, costPerProduct, quantity);
            transaction.Items.Add(item);

            Invoke(() => _tracker.SendTransaction(transaction));
        }

        public async Task SendTiming(TimeSpan time, string category, string variable, string label = "")
        {
            if (!IsEnabled)
            {
                return;
            }

            Log.Debug("Tracking timing: {0} | {1} | {2} | {3}", time, category, variable, label);

            Invoke(() => _tracker.SendTiming(time, category, variable, label));
        }

        public async Task Invoke(Action action)
        {
            if (_tracker == null)
            {
                await InitializeTracker();
            }

            if (_tracker == null)
            {
                Log.Error("No tracker available, cannot submit analytics, see previous warnings for details");
                return;
            }

            await Task.Factory.StartNew(action);
        }

        private async Task InitializeTracker()
        {
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

            var resolution = new Dimensions((int) System.Windows.SystemParameters.PrimaryScreenWidth,
                (int) System.Windows.SystemParameters.PrimaryScreenHeight);

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
        }
    }
}