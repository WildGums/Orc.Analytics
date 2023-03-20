namespace Orc.Analytics.Example.Commands;

using System.Threading.Tasks;
using Catel.Logging;
using Catel.MVVM;

public class TestCommandsAppWideTestCommandContainer : CommandContainerBase
{
    private static readonly ILog Log = LogManager.GetCurrentClassLogger();

    public TestCommandsAppWideTestCommandContainer(ICommandManager commandManager)
        : base(TestCommands.AppWideTest, commandManager)
    {
    }

    protected override async Task ExecuteAsync(object? parameter)
    {
        Log.Info("Executing application-wide command");
    }
}
