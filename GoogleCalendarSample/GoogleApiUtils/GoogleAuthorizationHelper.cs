using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Web;
using DotNetOpenAuth.OAuth2;
using Google.Apis.Authentication.OAuth2;
using Google.Apis.Authentication.OAuth2.DotNetOpenAuth;
using Google.Apis.Calendar.v3;
using Google.Apis.Util;

namespace GoogleApiUtils
{
    public static class GoogleAuthorizationHelper
    {
        private static string _clientId = ConfigurationManager.AppSettings["ClientId"];
        private static string _clientSecret = ConfigurationManager.AppSettings["ClientSecret"];
        private static string _redirectUri = ConfigurationManager.AppSettings["RedirectUri"];
        private static string[] _scopes = new[] { CalendarService.Scopes.Calendar.GetStringValue() };

        public static string GetAuthorizationUrl(string emailAddress)
        {
            var provider = new NativeApplicationClient(GoogleAuthenticationServer.Description, _clientId, _clientSecret);
            IAuthorizationState authorizationState = new AuthorizationState(_scopes);
            authorizationState.Callback = new Uri(_redirectUri);

            UriBuilder builder = new UriBuilder(provider.RequestUserAuthorization(authorizationState));
            NameValueCollection queryParameters = HttpUtility.ParseQueryString(builder.Query);

            queryParameters.Set("access_type", "offline");
            queryParameters.Set("approval_prompt", "force");
            queryParameters.Set("user_id", emailAddress);

            builder.Query = queryParameters.ToString();
            return builder.Uri.ToString();
        }

        public static GoogleAuthenticator GetAuthenticator(string authorizationCode)
        {
            var client = new NativeApplicationClient(GoogleAuthenticationServer.Description, _clientId, _clientSecret);
            IAuthorizationState state = new AuthorizationState() { Callback = new Uri(_redirectUri) };
            state = client.ProcessUserAuthorization(authorizationCode, state);

            var auth = new OAuth2Authenticator<NativeApplicationClient>(client, (c) => state);
            auth.LoadAccessToken();

            return new GoogleAuthenticator(auth);
        }

        public static GoogleAuthenticator RefreshAuthenticator(string refreshToken)
        {
            var state = new AuthorizationState(_scopes)
            {
                RefreshToken = refreshToken
            };

            var client = new NativeApplicationClient(GoogleAuthenticationServer.Description, _clientId, _clientSecret);
            bool result = client.RefreshToken(state);

            var auth = new OAuth2Authenticator<NativeApplicationClient>(client, (c) => state);
            auth.LoadAccessToken();

            return new GoogleAuthenticator(auth);
        }
    }
}