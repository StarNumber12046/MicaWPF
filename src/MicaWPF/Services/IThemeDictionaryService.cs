﻿using System.ComponentModel;

namespace MicaWPF.Services;

///<summary>
///Service that manages the theme dictionnaries from MicaWPF.
///</summary>
public interface IThemeDictionaryService
{
    /// <summary>
    /// The source of the current theme.
    /// </summary>
    Uri? ThemeSource { get; set; }

    /// <summary>
    /// Event that is raised when the `ThemeSource` property is changed.
    /// </summary>
    event PropertyChangedEventHandler? PropertyChanged;

    /// <summary>
    /// Sets the source of the current theme.
    /// </summary>
    /// <param name="source">The source of the theme to set.</param>
    void SetThemeSource(Uri source);

    /// <summary>
    /// Force the current theme source to reload.
    /// </summary>
    void RefreshThemeSource();
}