using THEMOOD.Logins;

namespace THEMOOD
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(MainPage), typeof(MainPage));
            Routing.RegisterRoute("main", typeof(MainPage));
            Routing.RegisterRoute("signup", typeof(SignUp));
            Routing.RegisterRoute("login", typeof(Login));
            //Routing.RegisterRoute("moodentry", typeof(MoodEntryPage));
            Shell.SetNavBarIsVisible(this, false);

        }
    }
}
