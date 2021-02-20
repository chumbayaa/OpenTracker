﻿using OpenTracker.Models.Locations;
using OpenTracker.Models.Markings;
using OpenTracker.Models.Sections;
using OpenTracker.Models.Settings;
using OpenTracker.Utils;
using ReactiveUI;
using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;

namespace OpenTracker.ViewModels.Maps.MapLocations
{
    /// <summary>
    /// This is the ViewModel class for map location tooltips.
    /// </summary>
    public class MapLocationToolTipVM : ViewModelBase
    {
        private readonly ILayoutSettings _layoutSettings;
        private readonly ILocation _location;

        public double Scale =>
            _layoutSettings.UIScale;
        public string Name =>
            _location.Name;
        public ObservableCollection<MapLocationToolTipMarkingVM> Markings { get; } =
            new ObservableCollection<MapLocationToolTipMarkingVM>();

        public MapLocationToolTipVM(ILocation location)
            : this(AppSettings.Instance.Layout, location)
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="location">
        /// The map location.
        /// </param>
        private MapLocationToolTipVM(
            ILayoutSettings layoutSettings, ILocation location)
        {
            _layoutSettings = layoutSettings;
            _location = location;

            _location.Notes.CollectionChanged += OnNotesChanged;
            _layoutSettings.PropertyChanged += OnLayoutChanged;

            RefreshMarkings();
        }

        /// <summary>
        /// Subscribes to the PropertyChanged event on the LayoutSettings class.
        /// </summary>
        /// <param name="sender">
        /// The sending object of the event.
        /// </param>
        /// <param name="e">
        /// The arguments of the PropertyChanged event.
        /// </param>
        private void OnLayoutChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(LayoutSettings.UIScale))
            {
                this.RaisePropertyChanged(nameof(Scale));
            }
        }

        /// <summary>
        /// Subscribes to the CollectionChanged event on the ObservableCollection of notes.
        /// </summary>
        /// <param name="sender">
        /// The sending object of the event.
        /// </param>
        /// <param name="e">
        /// The arguments of the PropertyChanged event.
        /// </param>
        private void OnNotesChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (object item in e.NewItems)
                {
                    Markings.Add(new MapLocationToolTipMarkingVM((IMarking)item));
                }
            }

            if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                foreach (object item in e.OldItems)
                {
                    IMarking marking = (IMarking)item;

                    foreach (var markingVM in Markings)
                    {
                        if (markingVM.Marking == marking)
                        {
                            Markings.Remove(markingVM);
                            break;
                        }
                    }
                }
            }

            if (e.Action == NotifyCollectionChangedAction.Reset)
            {
                RefreshMarkings();
            }
        }

        /// <summary>
        /// Refreshes the observable collection of marking ViewModel instances to match the
        /// location.
        /// </summary>
        private void RefreshMarkings()
        {
            Markings.Clear();

            if (_location.Sections[0] is IMarkableSection markableSection)
            {
                Markings.Add(new MapLocationToolTipMarkingVM(markableSection.Marking));
            }

            foreach (var note in _location.Notes)
            {
                Markings.Add(new MapLocationToolTipMarkingVM(note));
            }
        }
    }
}
