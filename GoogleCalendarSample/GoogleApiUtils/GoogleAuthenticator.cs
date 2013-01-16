using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Google.Apis.Authentication;
using Google.Apis.Authentication.OAuth2;
using Google.Apis.Authentication.OAuth2.DotNetOpenAuth;

namespace GoogleApiUtils
{
    public class GoogleAuthenticator
    {
        private OAuth2Authenticator<NativeApplicationClient> _authenticator;

        public GoogleAuthenticator(OAuth2Authenticator<NativeApplicationClient> authenticator)
        {
            _authenticator = authenticator;
        }

        internal IAuthenticator Authenticator
        {
            get { return _authenticator; }
        }

        public bool IsValid
        {
            get
            {
                return _authenticator != null &&
                    DateTime.Compare(DateTime.Now.ToUniversalTime(), _authenticator.State.AccessTokenExpirationUtc.Value) < 0;
            }
        }

        public string RefreshToken
        {
            get { return _authenticator.State.RefreshToken; }
        }
    }
}
