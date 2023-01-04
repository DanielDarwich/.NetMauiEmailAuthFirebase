﻿using Firebase.Auth;
using System.ComponentModel;

namespace EmailAuth.ViewModels
{
    internal class RegisterViewModel : INotifyPropertyChanged
    {
        public string webApiKey = "AIzaSyBZ6cvFQ_qtGoLwsaEInE3ueL1e4ZDEfaA";

        private INavigation _navigation;
        private string name;
        private string email;
        private string password;

        public event PropertyChangedEventHandler PropertyChanged;

        public string Name
        {
            get => name;
            set { name = value; RaisePropertyChanged("Name"); }
        }

        public string Email {
            get => email;
            set { email = value; RaisePropertyChanged("Email"); }
        }

        public string Password {
            get => password;
            set { password = value; RaisePropertyChanged("Password"); }
        }

        public Command RegisterUser { get; }

        private void RaisePropertyChanged(string v)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(v));
        }

        public RegisterViewModel(INavigation navigation)
        {
            this._navigation = navigation;

            RegisterUser = new Command(RegisterUserTappedAsync);
        }

        private async void RegisterUserTappedAsync(object obj)
        {
            try
            {
                var authProvider = new FirebaseAuthProvider(new FirebaseConfig(webApiKey));
                var auth = await authProvider.CreateUserWithEmailAndPasswordAsync(Email, Password);
                string token = auth.FirebaseToken;
                if (token != null)
                    await App.Current.MainPage.DisplayAlert("Alert", "User Registered successfully", "OK");
                await this._navigation.PopAsync();
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Alert", ex.Message, "OK");
                throw;
            }
        }
    }
}
