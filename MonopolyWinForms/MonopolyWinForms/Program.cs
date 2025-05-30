using MonopolyWinForms.Login_Signup;
using MonopolyWinForms.Home;
using MonopolyWinForms.Play_area;
using MonopolyWinForms.Room;
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
            Application.Run(new Main_login_signup());
            //Application.Run(new Draw_playarea());
            //Application.Run(new JoinRoom());
            //Application.Run(new Create_Room());
        }
    }
}