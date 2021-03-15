﻿using ReactiveUI;

namespace OpenTracker.Models.AutoTracking
{
    /// <summary>
    /// This interface contains SNES memory address data.
    /// </summary>
    public interface IMemoryAddress : IReactiveObject
    {
        byte Value { get; set; }

        delegate IMemoryAddress Factory();

        void Reset();
    }
}