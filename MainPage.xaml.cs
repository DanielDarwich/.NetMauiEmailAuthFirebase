using EmailAuth.Dependency;
using EmailAuth.ViewModels;
using Firebase.Auth;
using Nemiro.OAuth.LoginForms;
using Newtonsoft.Json;
using Plugin.GoogleClient;
using Plugin.GoogleClient.Shared;

namespace EmailAuth;

public partial class MainPage : ContentPage
{

    private AppleSignInService appleSignInService;
    public string webApiKey = "AIzaSyBZ6cvFQ_qtGoLwsaEInE3ueL1e4ZDEfaA";

    public MainPage()
	{
		InitializeComponent();
        appleSignInService = new AppleSignInService();
        if (appleSignInService == null)
        {
            Console.WriteLine("Apple Sign in Service is null");

        }
        BindingContext = new LoginViewModel(Navigation);
	}


    async void AppleSignInButton_SignIn(System.Object sender, System.EventArgs e)
    {
        Console.WriteLine("IIII");

        if (appleSignInService.IsAvailable())
        {
            Console.WriteLine("Apple Sign in Service is not null");

            var account = await appleSignInService.SignInAsync();

            if (account != null)
            {

                var authProvider = new FirebaseAuthProvider(new FirebaseConfig(webApiKey));
                try
                {
                    
                    var auth = await authProvider.SignInWithOAuthAsync(FirebaseAuthType.Apple, account.Token);
                    var content = await auth.GetFreshAuthAsync();
                    var serializedContent = JsonConvert.SerializeObject(content);

                    Preferences.Set("FreshFirebaseToken", serializedContent);
                    await this.Navigation.PushAsync(new Dashboard());
                }
                catch (Exception ex)
                {
                    await App.Current.MainPage.DisplayAlert("Alert", ex.Message, "OK");
                }
            }
        }
    }
    

    public partial Task<GoogleResponse<GoogleUser>> GoogleSignIn();

    async void Google_Clicked(System.Object sender, System.EventArgs e)
    {
        var login = await GoogleSignIn();
        var account = login.Data;

        if (account != null)
        {

            var authProvider = new FirebaseAuthProvider(new FirebaseConfig(webApiKey));
            try
            {

                var auth = await authProvider.SignInWithOAuthAsync(FirebaseAuthType.Google, account.Id);
                var content = await auth.GetFreshAuthAsync();
                var serializedContent = JsonConvert.SerializeObject(content);

                Preferences.Set("FreshFirebaseToken", serializedContent);
                await this.Navigation.PushAsync(new Dashboard());
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Alert", ex.Message, "OK");
            }
        }
    }



    async void Facebook_Clicked(System.Object sender, System.EventArgs e)
    {
        var fb = new FacebookLogin("SpareIt", "SpareIt123");
        var account = fb.AccessToken;

        if (account != null)
        {

            var authProvider = new FirebaseAuthProvider(new FirebaseConfig(webApiKey));
            try
            {

                var auth = await authProvider.SignInWithOAuthAsync(FirebaseAuthType.Facebook, fb.AccessTokenValue);
                var content = await auth.GetFreshAuthAsync();
                var serializedContent = JsonConvert.SerializeObject(content);

                Preferences.Set("FreshFirebaseToken", serializedContent);
                await this.Navigation.PushAsync(new Dashboard());
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Alert", ex.Message, "OK");
            }
        }
    }
}

