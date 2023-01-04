namespace EmailAuth.Control
{
    public enum AppleSignInButtonStyle
    {
        Black,
        White,
        WhiteOutline
    }

    public class AppleSignInButton : Button
    {
        public AppleSignInButton()
        {
            Clicked += AppleSignInButton_Clicked;
            Text = "Sign in with Apple";
            BorderWidth = 1;

            switch (ButtonStyle)
            {
                case AppleSignInButtonStyle.Black:
                    BackgroundColor = Color.FromArgb("000000");
                    TextColor = Color.FromArgb("FFFFFF");
                    BorderColor = Color.FromArgb("000000");
                    break;
                case AppleSignInButtonStyle.White:
                    BackgroundColor = Color.FromArgb("FFFFFF");
                    TextColor = Color.FromArgb("000000");
                    BorderColor = Color.FromArgb("FFFFFF");
                    break;

                case AppleSignInButtonStyle.WhiteOutline:
                    BackgroundColor = Color.FromArgb("FFFFFF");
                    TextColor = Color.FromArgb("000000");
                    BorderColor = Color.FromArgb("000000");
                    break;
            }
        }

        public AppleSignInButtonStyle ButtonStyle { get; set; } = AppleSignInButtonStyle.Black;

        private void AppleSignInButton_Clicked(object sender, EventArgs args)
        {
            Console.WriteLine("UUUUUUUUU");
            SignIn?.Invoke(sender, args);
            Command?.Execute(CommandParameter);
        }

        public event EventHandler SignIn;

        public void InvokeSignInEvent(object sender, EventArgs e) => SignIn?.Invoke(sender, e);

        public void Dispose() => Clicked -= AppleSignInButton_Clicked;
    }
}
