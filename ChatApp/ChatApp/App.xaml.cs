namespace ChatApp;

public partial class App : Application
{
	public App()
	{
		InitializeComponent();
		MainPage = new AppShell();
	}
     protected override void OnStart()
      {
          base.OnStart();
          NavigateBasedOnLoginStatus();
      }

      private async void NavigateBasedOnLoginStatus()
      {
          bool isLoggedIn = Preferences.Get("IsLoggedIn", false);
          var route = isLoggedIn ? "//ChatsListPage" : "//LoginPage";

          // Use `MainPage` as `Shell` directly if `Current` is not ready
          await (MainPage as Shell)?.GoToAsync(route);
      }
}

