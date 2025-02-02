﻿using System.Runtime.CompilerServices;

namespace MicaWPF.Events;

/// <summary>
/// A weak event that is less prone to memory leaks.
/// </summary>
/// <typeparam name="T">The type to pass to the <see cref="ISubscription"/>.</typeparam>
public interface IWeakEvent<T>
{
    ConfiguredTaskAwaitable PublishAsync(T data, bool awaitAllCalls = false);
    void Publish(T data);
    ISubscription Subscribe(Action<T> action);
}
