
using TradeX.Application.Abstractions.Authentication;

namespace TradeX.Api.DelegatingHandlers
{
    //public class AuthenticationRetry : DelegatingHandler
    //{
    //    private readonly IJwtTokenProvider _jwtTokenProvider;

    //    public AuthenticationRetry(IJwtTokenProvider jwtTokenProvider)
    //    {
    //        _jwtTokenProvider = jwtTokenProvider;
    //    }

    //    protected async override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    //    {
    //        var result = await base.SendAsync(request, cancellationToken);

    //        if (result.StatusCode == System.Net.HttpStatusCode.Unauthorized)
    //        {
    //            var expiredAccessToken = request.Headers.Authorization!.ToString().Split(' ')[1];
                
    //            //var jwt = _jwtTokenProvider.RefreshAccessToken()
    //        }
    //    }
    //}
}
