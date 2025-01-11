using System;
using Microsoft.JSInterop;
using System.Threading.Tasks;

public class ToastService
{
    private readonly IJSRuntime JS;

    public ToastService(IJSRuntime js)
    {
        JS = js;
    }

    public async Task ShowSuccess(string message)
    {
        await ShowToast("successToast", message);
    }

    public async Task ShowError(string message)
    {
        await ShowToast("errorToast", message);
    }

    public async Task ShowInfo(string message)
    {
        await ShowToast("infoToast", message);
    }

    public async Task ShowWarning(string message)
    {
        await ShowToast("warningToast", message);
    }

    private async Task ShowToast(string toastType, string message)
    {
        await JS.InvokeVoidAsync("showToast", toastType, message);
    }
}