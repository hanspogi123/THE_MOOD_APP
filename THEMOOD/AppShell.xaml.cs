namespace THEMOOD
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(MainPage), typeof(MainPage));
            Routing.RegisterRoute("main", typeof(MainPage));
            Shell.SetNavBarIsVisible(this, false);

        }
    }
}
