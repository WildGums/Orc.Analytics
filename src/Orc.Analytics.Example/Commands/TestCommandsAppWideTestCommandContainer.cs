// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TestCommandsAppWideTestCommandCommandContainer.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2015 Wild Gums. All rights reserved.
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