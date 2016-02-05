// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TestCommandsAppWideTestCommandCommandContainer.cs" company="WildGums">
//   Copyright (c) 2008 - 2015 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Analytics.Example.Commands
{
    using System.Threading.Tasks;
    using Catel.Logging;
    using Catel.MVVM;

    public class TestCommandsAppWideTestCommandContainer : CommandContainerBase
    {
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        #region Constructors
        public TestCommandsAppWideTestCommandContainer(ICommandManager commandManager)
            : base(TestCommands.AppWideTest, commandManager)
        {
        }

        protected override async Task ExecuteAsync(object parameter)
        {
            Log.Info("Executing application-wide command");
        }
        #endregion
    }
}