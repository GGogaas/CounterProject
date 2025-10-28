using Counter.views;

namespace Counter
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(views.EditCounterPage),typeof(views.EditCounterPage));
        }
    }
}
