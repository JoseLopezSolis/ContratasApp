using System;
using Microsoft.Maui;

namespace ContratasApp.Utils;

public static class ServiceProvider
{
    public static object? GetService(Type serviceType)
        => Current != null
            ? Current.GetService(serviceType)
            : default;
    private static IServiceProvider? Current => IPlatformApplication.Current?.Services;
}