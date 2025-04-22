using TicketTidyFRONT.Pages;

namespace TicketTidyFRONT
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            var perfil = Preferences.Get("perfil", string.Empty);

            switch (perfil)
            {
                case "tecnico":
                    if (Preferences.Get("idTecnico", (long)0) != (long)0)
                    {
                        MainPage = new PrincipalTecnico();
                        return;
                    }
                    break;

                case "gestor":
                    if (Preferences.Get("idGestor", (long)0) != (long)0)
                    {
                        MainPage = new PrincipalGestor();
                        return;
                    }
                    break;

                case "admin":
                    if (Preferences.Get("idAdmin", (long)0) != (long)0)
                    {
                        MainPage = new PrincipalAdmin();
                        return;
                    }
                    break;

                case "basico":
                    if (Preferences.Get("idBasico", (long)0) != (long)0)
                    {
                        MainPage = new PrincipalBasico();
                        return;
                    }
                    break;
            }

            // Si no se encontró nada, va al login
            MainPage = new LoginPage();
        }

        public static NavigationPage Navigate { get; internal set; }
        public static PrincipalTecnico Menu { get; internal set; }
        public static PrincipalGestor MenuGestor { get; internal set; }
    }
}
