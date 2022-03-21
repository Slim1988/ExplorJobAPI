using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace ExplorJobAPI.Infrastructure
{
    public class IpSafeListMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<IpSafeListMiddleware> _logger;
        private readonly string _ipSafeList;

        public IpSafeListMiddleware(
            RequestDelegate next, 
            ILogger<IpSafeListMiddleware> logger, 
            string ipSafeList
        ) {
            _ipSafeList = ipSafeList;
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(
            HttpContext context
        ) {
            if (context.Request.Method != "OPTIONS") {
                var remoteIp = context.Connection.RemoteIpAddress;
                _logger.LogDebug("Request from Remote IP address: {RemoteIp}", remoteIp);

                string[] ip = _ipSafeList.Split(';');

                var bytes = remoteIp.GetAddressBytes();
                var badIp = true;

                foreach (var address in ip) {
                    var testIp = IPAddress.Parse(address);
                    
                    if(testIp.GetAddressBytes().SequenceEqual(bytes)) {
                        badIp = false;
                        break;
                    }
                }

                if(badIp) {
                    _logger.LogInformation(
                        "Forbidden Request from Remote IP address: {RemoteIp}",
                        remoteIp
                    );
                    context.Response.StatusCode = 401;
                    return;
                }
            }

            await _next.Invoke(context);
        }
    }
}
