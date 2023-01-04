using EmailAuth.Model;

namespace EmailAuth.Dependency
{
    public partial class AppleSignInService
    {
        public partial bool IsAvailable();

        public partial Task<AppleSignInCredentialState> GetCredentialStateAsync(string userId);

        public partial Task<AppleAccount> SignInAsync();
    }
}
