using Microsoft.AspNetCore.Components;

public static class NavigationManagerExtensions
{
    public static string GetRoute(this NavigationManager manager) => manager.Uri.Substring(manager.BaseUri.Length);
}