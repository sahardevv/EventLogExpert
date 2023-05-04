// // Copyright (c) Microsoft Corporation.
// // Licensed under the MIT License.

using EventLogExpert.Library.Models;
using EventLogExpert.Store.EventLog;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace EventLogExpert.Components;

public partial class EventTable
{
    private readonly Dictionary<string, int> _colWidths = new()
    {
        { "RecordId", 75 },
        { "TimeCreated", 165 },
        { "Id", 50 },
        { "MachineName", 100 },
        { "Level", 100 },
        { "ProviderName", 250 },
        { "Task", 150 }
    };

    private IJSObjectReference? _jsModule;
    private ElementReference _tableRef;

    [Inject] private IJSRuntime JSRuntime { get; set; } = null!;

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            _jsModule = await JSRuntime.InvokeAsync<IJSObjectReference>("import", "./Components/EventTable.razor.js");
            _jsModule?.InvokeVoidAsync("enableColumnResize", _tableRef).AsTask();
        }

        await base.OnAfterRenderAsync(firstRender);
    }

    private string GetInlineStyle(string colName) => $"width: {_colWidths[colName]}px";

    private void SelectEvent(DisplayEventModel @event) => Dispatcher.Dispatch(new EventLogAction.SelectEvent(@event));
}
