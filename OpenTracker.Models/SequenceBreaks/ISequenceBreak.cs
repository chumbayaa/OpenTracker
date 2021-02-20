﻿using OpenTracker.Models.SaveLoad;
using System.ComponentModel;

namespace OpenTracker.Models.SequenceBreaks
{
    /// <summary>
    /// This is the interface for sequence breaks.
    /// </summary>
    public interface ISequenceBreak : INotifyPropertyChanged
    {
        bool Enabled { get; set; }

        delegate ISequenceBreak Factory(bool starting = true);

        void Load(SequenceBreakSaveData saveData);
        void Reset();
        SequenceBreakSaveData Save();
    }
}