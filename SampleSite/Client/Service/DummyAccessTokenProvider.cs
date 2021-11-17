using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;

namespace SampleSite.Client.Service
{
    public class DummyAccessTokenProvider : IAccessTokenProvider
    {
        public ValueTask<AccessTokenResult> RequestAccessToken()
        {
            return new ValueTask<AccessTokenResult>(new AccessTokenResult(
                AccessTokenResultStatus.Success,
                new AccessToken
                {
                    Expires = DateTimeOffset.UtcNow.AddHours(1),
                    GrantedScopes = Array.Empty<string>(),
                    Value = "THIS_IS_DUMMY_TOKEN"
                },
                redirectUrl: ""));
        }

        public ValueTask<AccessTokenResult> RequestAccessToken(AccessTokenRequestOptions options) => throw new NotImplementedException();
    }
}
