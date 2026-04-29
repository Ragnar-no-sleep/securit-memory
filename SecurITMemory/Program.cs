using SecurITMemory.Forms;

namespace SecurITMemory;

internal static class Program
{
    [STAThread]
    static void Main()
    {
        ApplicationConfiguration.Initialize();
        Application.Run(new MenuForm());
    }
}
