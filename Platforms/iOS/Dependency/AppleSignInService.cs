using System.Threading.Tasks;
using AuthenticationServices;
using EmailAuth.Dependency;
using EmailAuth.Model;
using Foundation;
using Plugin.GoogleClient;
using Plugin.GoogleClient.Shared;
using UIKit;
using Nemiro.OAuth.LoginForms;
using Nemiro.OAuth;

namespace EmailAuth
{
    public partial class MainPage
    {
        public async partial Task<GoogleResponse<GoogleUser>> GoogleSignIn()
        {
            //var google = new Nemiro.OAuth.LoginForms.LoginForms.GoogleLogin("978513722395-3r8do7b08hjicepp0s14m07krmu5uj16.apps.googleusercontent.com", "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");
            var a = await Plugin.GoogleClient.CrossGoogleClient.Current.LoginAsync();
            return a;
        }
    }
}

namespace EmailAuth.Dependency
{
    public partial class AppleSignInService : NSObject, IASAuthorizationControllerDelegate, IASAuthorizationControllerPresentationContextProviding
    {
        #region IAppleSignInService

        public partial bool IsAvailable()
        {
            return UIDevice.CurrentDevice.CheckSystemVersion(13, 0);
        }
        

        TaskCompletionSource<ASAuthorizationAppleIdCredential> tcsCredential;

        public async partial Task<AppleSignInCredentialState> GetCredentialStateAsync(string userId)
        {
            var appleIdProvider = new ASAuthorizationAppleIdProvider();
            var credentialState = await appleIdProvider.GetCredentialStateAsync(userId);

            switch (credentialState)
            {
                case ASAuthorizationAppleIdProviderCredentialState.Authorized:
                    return AppleSignInCredentialState.Authorized;
                case ASAuthorizationAppleIdProviderCredentialState.Revoked:
                    return AppleSignInCredentialState.Revoked;
                case ASAuthorizationAppleIdProviderCredentialState.NotFound:
                    return AppleSignInCredentialState.NotFound;
                default:
                    return AppleSignInCredentialState.Unknown;
            }
        }


        public async partial Task<AppleAccount> SignInAsync()
        {
            Console.WriteLine("AAAAAAAAAAAABBBBBBBBBBCCCCCCCCCCCC");
            var appleIdProvider = new ASAuthorizationAppleIdProvider();
            var request = appleIdProvider.CreateRequest();

            request.RequestedScopes = new[] { ASAuthorizationScope.Email, ASAuthorizationScope.FullName };
            Console.WriteLine("DDDDDDD");

            var authorizationController = new ASAuthorizationController(new[] { request });
            authorizationController.Delegate = this;
            authorizationController.PresentationContextProvider = this;
            authorizationController.PerformRequests();

            tcsCredential = new TaskCompletionSource<ASAuthorizationAppleIdCredential>();

            var creds = await tcsCredential.Task;
            Console.WriteLine("EEEEEEEEEE");

            if (creds == null)
                return null;

            var appleAccount = new AppleAccount
            {
                Email = creds.Email,
                Name = NSPersonNameComponentsFormatter.GetLocalizedString(creds.FullName,
                                                NSPersonNameComponentsFormatterStyle.Default,
                                                NSPersonNameComponentsFormatterOptions.Phonetic),
                RealUserStatus = creds.RealUserStatus.ToString(),
                Token = new NSString(creds.IdentityToken, NSStringEncoding.UTF8).ToString(),
                UserId = creds.User
            };
            Console.WriteLine("FFFFFFFFFFFFF");

            return appleAccount;
        }

        #endregion

        #region IASAuthorizationControllerDelegate

        [Export("authorizationController:didCompleteWithAuthorization:")]
        public void DidComplete(ASAuthorizationController controller, ASAuthorization authorization)
        {
            var creds = authorization.GetCredential<ASAuthorizationAppleIdCredential>();
            tcsCredential?.TrySetResult(creds);
        }

        [Export("authorizationController:didCompleteWithError:")]
        public void DidComplete(ASAuthorizationController controller, NSError error)
        {
            tcsCredential?.TrySetResult(null);
            System.Console.WriteLine(error);
        }

        #endregion

        #region IASAuthorizationControllerPresentation Context Providing

        public UIWindow GetPresentationAnchor(ASAuthorizationController controller)
        {
            return UIApplication.SharedApplication.KeyWindow;
        }

        #endregion
    }
}