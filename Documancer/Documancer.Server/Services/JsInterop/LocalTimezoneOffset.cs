using Documancer.Server.Services.JsInterop;
using Microsoft.JSInterop;

namespace Documancer.Server.Services.JsInterop
{
    public class LocalTimezoneOffset
    {
        private readonly IJSRuntime _jsRuntime;

        public LocalTimezoneOffset(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }

        public async ValueTask<int> HourOffset()
        {
            var jsmodule = await _jsRuntime.InvokeAsync<IJSObjectReference>("import", "/js/timezoneoffset.js").ConfigureAwait(false);
            return await jsmodule.InvokeAsync<int>(JSInteropConstants.GetTimeZoneOffset).ConfigureAwait(false);
        }
    }
}