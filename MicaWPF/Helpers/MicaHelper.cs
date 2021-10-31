﻿using System;
using System.Management;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Shell;

namespace MicaWPF.Helpers;

public static class MicaHelper
{
    [DllImport("dwmapi.dll")]
    private static extern int DwmSetWindowAttribute(IntPtr hwnd, DwmWindowAttribute dwAttribute, ref int pvAttribute, int cbAttribute);

    [Flags]
    private enum DwmWindowAttribute : uint
    {
        DWMWA_USE_IMMERSIVE_DARK_MODE = 20,
        DWMWA_MICA_EFFECT = 1029
    }

    private static ManagementObject GetMngObj(string className)
    {
        ManagementClass wmi = new(className);

        foreach (ManagementBaseObject o in wmi.GetInstances())
        {
            ManagementObject mo = (ManagementObject)o;
            if (mo != null)
            {
                return mo;
            }
        }

        return null;
    }

    public static Version GetOsVer()
    {
        ManagementObject mo = GetMngObj("Win32_OperatingSystem");

        return mo == null ? new Version(0, 0, 0, 0) : Version.Parse(mo["Version"].ToString());
    }

    public static void EnableMica(this Window window, WindowsTheme theme, bool isThemeAware)
    {
        IntPtr windowHandle = new WindowInteropHelper(window).Handle;
        WindowsTheme darkThemeEnabled = ThemeHelper.GetWindowsTheme();

        int trueValue = 0x01;
        int falseValue = 0x00;

        if (theme is WindowsTheme.Auto)
        {
            _ = darkThemeEnabled == WindowsTheme.Dark
                ? DwmSetWindowAttribute(windowHandle, DwmWindowAttribute.DWMWA_USE_IMMERSIVE_DARK_MODE, ref trueValue, Marshal.SizeOf(typeof(int)))
                : DwmSetWindowAttribute(windowHandle, DwmWindowAttribute.DWMWA_USE_IMMERSIVE_DARK_MODE, ref falseValue, Marshal.SizeOf(typeof(int)));
        }
        else
        {
            _ = theme is WindowsTheme.Light
                ? DwmSetWindowAttribute(windowHandle, DwmWindowAttribute.DWMWA_USE_IMMERSIVE_DARK_MODE, ref falseValue, Marshal.SizeOf(typeof(int)))
                : DwmSetWindowAttribute(windowHandle, DwmWindowAttribute.DWMWA_USE_IMMERSIVE_DARK_MODE, ref trueValue, Marshal.SizeOf(typeof(int)));
        }
        if (GetOsVer() >= new Version(10, 0, 222000, 0) && Environment.OSVersion.Platform == PlatformID.Win32NT)
        {
            _ = DwmSetWindowAttribute(windowHandle, DwmWindowAttribute.DWMWA_MICA_EFFECT, ref trueValue, Marshal.SizeOf(typeof(int)));
        }
        else
        {
            WindowChrome.SetWindowChrome(
                window,
                new WindowChrome()
                {
                    CaptionHeight = 20,
                    ResizeBorderThickness = new Thickness(8),
                    CornerRadius = new CornerRadius(0),
                    GlassFrameThickness = new Thickness(0, 32, 0, 0),
                    UseAeroCaptionButtons = true
                }
                );
        }

        if (isThemeAware is true) 
        {
            _ = Task.Run(() =>
            {
                var watcher = ThemeHelper.WatchThemeChange();
                watcher.EventArrived += (sender, args) =>
                {
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        EnableMica(window, theme, isThemeAware);
                    });
                };
                watcher.Start();
            });
        }
    }
}
