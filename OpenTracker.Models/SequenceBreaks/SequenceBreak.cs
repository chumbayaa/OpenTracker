﻿using OpenTracker.Models.SaveLoad;
using ReactiveUI;

namespace OpenTracker.Models.SequenceBreaks
{
    /// <summary>
    /// This class contains sequence breaks.
    /// </summary>
    public class SequenceBreak : ReactiveObject, ISequenceBreak
    {
        private readonly bool _starting;

        private bool _enabled = true;
        public bool Enabled
        {
            get => _enabled;
            set => this.RaiseAndSetIfChanged(ref _enabled, value);
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="starting">
        /// A boolean representing the starting value of this sequence break.
        /// </param>
        public SequenceBreak(bool starting = true)
        {
            _starting = starting;
        }

        /// <summary>
        /// Resets the sequence break to its starting value.
        /// </summary>
        public void Reset()
        {
            Enabled = _starting;
        }

        /// <summary>
        /// Returns a new sequence break save data instance for this sequence break.
        /// </summary>
        /// <returns>
        /// A new sequence break save data instance.
        /// </returns>
        public SequenceBreakSaveData Save()
        {
            return new SequenceBreakSaveData()
            {
                Enabled = Enabled
            };
        }

        /// <summary>
        /// Loads sequence break save data.
        /// </summary>
        public void Load(SequenceBreakSaveData saveData)
        {
            Enabled = saveData.Enabled;
        }
    }
}
