﻿using Microsoft.JSInterop;

namespace Documancer.Server.Services.JsInterop
{
    public class OpenSeadragon
    {
        private readonly IJSRuntime _jsRuntime;

        public OpenSeadragon(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }

        public async Task<ValueTask> Open(string url)
        {
            var target = "openseadragon";
            var jsmodule = await _jsRuntime.InvokeAsync<IJSObjectReference>("import", "/js/openseadragon.js").ConfigureAwait(false);
            return jsmodule.InvokeVoidAsync(JSInteropConstants.ShowOpenSeadragon, target, url);
        }
    }
}