﻿using OpenTracker.Models.AutoTracking;
using OpenTracker.Models.SaveLoad;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace OpenTracker.Models.Items
{
    /// <summary>
    /// This is the class containing autotracking data for an item.
    /// </summary>
    public class AutoTrackedItem : IItem
    {
        private readonly IItem _item;

        private Func<int, int?> AutoTrackFunction { get; }

        public int Current
        {
            get => _item.Current;
            set => _item.Current = value;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="item">
        /// The base item to be wrapped in auto tracking.
        /// </param>
        /// <param name="autoTrackFunction">
        /// The action delegate to perform autotracking.
        /// </param>
        /// <param name="memoryAddresses">
        /// The list of memory addresses to which to subscribe to indicate a change in autotracking.
        /// </param>
        internal AutoTrackedItem(
            IItem item, Func<int, int?> autoTrackFunction,
            List<(MemorySegmentType, int)> memoryAddresses)
        {
            _item = item ?? throw new ArgumentNullException(nameof(item));
            AutoTrackFunction = autoTrackFunction ??
                throw new ArgumentNullException(nameof(autoTrackFunction));

            foreach ((MemorySegmentType, int) address in memoryAddresses)
            {
                SubscribeToMemoryAddress(address.Item1, address.Item2);
            }

            _item.PropertyChanged += OnPropertyChanged;
        }

        /// <summary>
        /// Raises the PropertyChanged event for the specified property.
        /// </summary>
        /// <param name="propertyName">
        /// The string of the property name of the changed property.
        /// </param>
        private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            PropertyChanged?.Invoke(this, e);
        }

        /// <summary>
        /// Subscribes to the PropertyChanged event on the MemoryAddress class.
        /// </summary>
        /// <param name="sender">
        /// The sending object of the event.
        /// </param>
        /// <param name="e">
        /// The arguments of the PropertyChanged event.
        /// </param>
        private void OnMemoryChanged(object sender, PropertyChangedEventArgs e)
        {
            AutoTrack();
        }

        /// <summary>
        /// Autotrack the item.
        /// </summary>
        private void AutoTrack()
        {
            int? result = AutoTrackFunction(Current);

            if (result.HasValue)
            {
                Current = result.Value;
            }
        }

        /// <summary>
        /// Creates subscription to the PropertyChanged event on the MemoryAddress class.
        /// </summary>
        /// <param name="segment">
        /// The memory segment to which to subscribe.
        /// </param>
        /// <param name="index">
        /// The index within the memory address list to which to subscribe.
        /// </param>
        private void SubscribeToMemoryAddress(MemorySegmentType segment, int index)
        {
            List<MemoryAddress> memory = segment switch
            {
                MemorySegmentType.Room => AutoTracker.Instance.RoomMemory,
                MemorySegmentType.OverworldEvent => AutoTracker.Instance.OverworldEventMemory,
                MemorySegmentType.Item => AutoTracker.Instance.ItemMemory,
                MemorySegmentType.NPCItem => AutoTracker.Instance.NPCItemMemory,
                _ => throw new ArgumentOutOfRangeException(nameof(segment))
            };

            if (index >= memory.Count)
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }

            memory[index].PropertyChanged += OnMemoryChanged;
        }

        /// <summary>
        /// Returns whether an item can be added.
        /// </summary>
        /// <returns>
        /// A boolean representing whether an item can be added.
        /// </returns>
        public bool CanAdd()
        {
            return _item.CanAdd();
        }

        /// <summary>
        /// Returns whether an item can be removed.
        /// </summary>
        /// <returns>
        /// A boolean representing whether an item can be removed.
        /// </returns>
        public bool CanRemove()
        {
            return _item.CanRemove();
        }

        /// <summary>
        /// Resets the item to its starting values.
        /// </summary>
        public void Reset()
        {
            _item.Reset();
        }

        /// <summary>
        /// Returns a new item save data instance for this item.
        /// </summary>
        /// <returns>
        /// A new item save data instance.
        /// </returns>
        public ItemSaveData Save()
        {
            return new ItemSaveData()
            {
                Current = Current
            };
        }

        /// <summary>
        /// Loads item save data.
        /// </summary>
        public void Load(ItemSaveData saveData)
        {
            if (saveData == null)
            {
                throw new ArgumentNullException(nameof(saveData));
            }

            Current = saveData.Current;
        }
    }
}
