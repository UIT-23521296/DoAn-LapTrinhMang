using MonopolyWinForms.Login_Signup;
using MonopolyWinForms.Home;
using MonopolyWinForms.Play_area;
namespace MonopolyWinForms
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            //Application.Run(new Main_login_signup());
            Application.Run(new Draw_playarea());
        }
    }
}