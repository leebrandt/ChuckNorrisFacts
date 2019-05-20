using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Okta.Auth.Sdk;
using Okta.Sdk.Abstractions.Configuration;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ChuckNorrisFacts
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginView : ContentView
    {
        public event EventHandler<bool> LoginChanged;
        public LoginView()
        {
            InitializeComponent();
        }

        private void SignupClicked(object sender, EventArgs e)
        {
            Device.OpenUri(new Uri("https://dev-846291.oktapreview.com/signin/register"));
        }

        private async void LoginClicked(object sender, EventArgs e)
        {
            var success = await Login(EmailEntry.Text, PasswordEntry.Text);
            if (LoginChanged != null) LoginChanged(this, success);
            if (success)
            {
                LoginPanel.IsVisible = false;
                LogoutPanel.IsVisible = true;
                ErrorLabel.Text = "";
            }
            else
            {
                ErrorLabel.Text = "Login failed.";
            }
        }

        private void LogoutClicked(object sender, EventArgs e)
        {
            LoginPanel.IsVisible = true;
            LogoutPanel.IsVisible = false;
            if (LoginChanged != null) LoginChanged(this, false);
        }


        public static async Task<bool> Login(string email, string password)
        {
            var config = new OktaClientConfiguration { OktaDomain = "https://dev-846291.oktapreview.com", };
            var authClient = new AuthenticationClient(config);

            var authnOptions = new AuthenticateOptions { Username = email, Password = password };
            try
            {
                var authnResponse = await authClient.AuthenticateAsync(authnOptions);
                return authnResponse.AuthenticationStatus == AuthenticationStatus.Success;
            }
            catch
            {
                return false;
            }
        }


    }
}