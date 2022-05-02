﻿using MicaWPF.Controls;

namespace MicaWPF.Services;
internal class DynamicThemeService
{
    private readonly OsVersion _currentOsVersion;
    private readonly Window _window;
    private CancellationTokenSource _waitForDynamicThemeCancellationToken = new();
    private bool _isWaitingForThemeChange = false;
    private bool _isThemeAware = false;

    public DynamicThemeService(Window window)
    {
        _window = window;
        _currentOsVersion = OsHelper.GlobalOsVersion;
    }

    public void SetThemeAware(bool isThemeAware, BackdropType micaType = BackdropType.Mica, bool useSystemAccent = true)
    {
        if (!_isThemeAware && isThemeAware)
        {
            _isThemeAware = true;
            if (_currentOsVersion is not OsVersion.WindowsOld && isThemeAware)
            {
                SystemEvents.UserPreferenceChanged += (s, e) =>
                {
                    switch (e.Category)
                    {
                        case UserPreferenceCategory.General:
                            Application.Current.Dispatcher.Invoke(() => MicaHelper.EnableMica(_window, WindowsTheme.Auto, micaType, -1, useSystemAccent));
                            SetThemeAware(isThemeAware, micaType, useSystemAccent);
                            break;
                    }
                };
            }
        }
        else if (_isThemeAware && !isThemeAware)
        {
            _isThemeAware = false;
            var handler = (UserPreferenceChangedEventHandler)Delegate.CreateDelegate(typeof(UserPreferenceChangedEventHandler), this, "UserPreferenceChangedHandler");
            SystemEvents.UserPreferenceChanged -= handler;
        }
    }

    public void AwaitManualThemeChange(bool awaitChange, BackdropType micaType = BackdropType.Mica, bool useSystemAccent = true)
    {
        if (!_isWaitingForThemeChange && awaitChange)
        {
            _waitForDynamicThemeCancellationToken = new();
            if (_window is MicaWindow micaWindow)
            {
                var oldTheme = micaWindow.Theme;
                _ = Task.Run(async () =>
                {
                    while (oldTheme == micaWindow.Theme && !_waitForDynamicThemeCancellationToken.IsCancellationRequested)
                    {
                        await Task.Delay(500);
                    }

                    if (!_waitForDynamicThemeCancellationToken.IsCancellationRequested)
                    {
                        Application.Current.Dispatcher.Invoke(() => MicaHelper.EnableMica(_window, micaWindow.Theme, micaType, -1));
                        AwaitManualThemeChange(awaitChange, micaType, useSystemAccent);
                    }
                    else
                    {
                        _isWaitingForThemeChange = false;
                    }
                });
            }
        }
        else if (_isWaitingForThemeChange && !awaitChange)
        {
            _waitForDynamicThemeCancellationToken.Cancel();
        }
    }
}
