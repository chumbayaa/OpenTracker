﻿using System.ComponentModel;
using OpenTracker.Models.AutoTracking.Values;
using OpenTracker.Models.BossPlacements;
using OpenTracker.Models.Dungeons.AccessibilityProvider;
using OpenTracker.Models.PrizePlacements;
using OpenTracker.Models.Requirements;
using OpenTracker.Models.SaveLoad;
using OpenTracker.Models.UndoRedo;
using OpenTracker.Models.UndoRedo.Sections;

namespace OpenTracker.Models.Sections
{
    /// <summary>
    /// This class contains boss/prize section data (end of each dungeon).
    /// </summary>
    public class PrizeSection : BossSection, IPrizeSection
    {
        private readonly ISaveLoadManager _saveLoadManager;

        private readonly ITogglePrizeSection.Factory _togglePrizeSectionFactory;
        
        private readonly bool _alwaysClearable;
        private readonly IAutoTrackValue? _autoTrackValue;

        public IPrizePlacement PrizePlacement { get; }

        public delegate PrizeSection Factory(
            string name, IBossPlacement bossPlacement, IPrizePlacement prizePlacement,
            IBossAccessibilityProvider accessibilityProvider, IAutoTrackValue? autoTrackValue, IRequirement requirement,
            bool alwaysClearable = false);

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="saveLoadManager">
        /// The save/load manager.
        /// </param>
        /// <param name="collectSectionFactory">
        /// An Autofac factory for creating collect section undoable actions.
        /// </param>
        /// <param name="uncollectSectionFactory">
        /// An Autofac factory for creating uncollect section undoable actions.
        /// </param>
        /// <param name="togglePrizeSectionFactory">
        /// An Autofac factory for creating undoable actions to toggle the prize.
        /// </param>
        /// <param name="name">
        /// A string representing the name of the section.
        /// </param>
        /// <param name="bossPlacement">
        /// The boss placement of this section.
        /// </param>
        /// <param name="prizePlacement">
        /// The prize placement of this section.
        /// </param>
        /// <param name="accessibilityProvider">
        /// The dungeon accessibility provider.
        /// </param>
        /// <param name="autoTrackValue">
        /// The section auto-track value.
        /// </param>
        /// <param name="requirement">
        /// The requirement for this section to be visible.
        /// </param>
        /// <param name="alwaysClearable">
        /// A boolean representing whether the section is always clearable (used for GT final).
        /// </param>
        public PrizeSection(
            ISaveLoadManager saveLoadManager, ICollectSection.Factory collectSectionFactory,
            IUncollectSection.Factory uncollectSectionFactory, ITogglePrizeSection.Factory togglePrizeSectionFactory,
            string name, IBossPlacement bossPlacement, IPrizePlacement prizePlacement,
            IBossAccessibilityProvider accessibilityProvider, IAutoTrackValue? autoTrackValue, IRequirement requirement,
            bool alwaysClearable = false)
            : base(collectSectionFactory, uncollectSectionFactory, name, bossPlacement, requirement,
                accessibilityProvider)
        {
            _saveLoadManager = saveLoadManager;
            
            _alwaysClearable = alwaysClearable;
            _autoTrackValue = autoTrackValue;
            _togglePrizeSectionFactory = togglePrizeSectionFactory;
            
            PrizePlacement = prizePlacement;

            PropertyChanged += OnPropertyChanged;
            PrizePlacement.PropertyChanging += OnPrizeChanging;
            PrizePlacement.PropertyChanged += OnPrizeChanged;

            if (_autoTrackValue is null)
            {
                return;
            }
            
            _autoTrackValue.PropertyChanged += OnAutoTrackValueChanged;
        }

        /// <summary>
        /// Subscribes to the PropertyChanged event on itself.
        /// </summary>
        /// <param name="sender">
        /// The sending object of the event.
        /// </param>
        /// <param name="e">
        /// The arguments of the PropertyChanged event.
        /// </param>
        private void OnPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName != nameof(Available) || PrizePlacement.Prize is null)
            {
                return;
            }
            
            if (IsAvailable())
            {
                PrizePlacement.Prize.Remove();
                return;
            }

            PrizePlacement.Prize.Add();
        }

        /// <summary>
        /// Subscribes to the PropertyChanging event on IPrizePlacement interface.
        /// </summary>
        /// <param name="sender">
        /// The sending object of the event.
        /// </param>
        /// <param name="e">
        /// The arguments of the PropertyChanging event.
        /// </param>
        private void OnPrizeChanging(object? sender, PropertyChangingEventArgs e)
        {
            if (e.PropertyName != nameof(IPrizePlacement.Prize))
            {
                return;
            }
            
            if (!IsAvailable())
            {
                PrizePlacement.Prize?.Remove();
            }
        }

        /// <summary>
        /// Subscribes to the PropertyChanged event on the IPrizePlacement interface.
        /// </summary>
        /// <param name="sender">
        /// The sending object of the event.
        /// </param>
        /// <param name="e">
        /// The arguments of the PropertyChanged event.
        /// </param>
        private void OnPrizeChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName != nameof(IPrizePlacement.Prize))
            {
                return;
            }
            
            if (!IsAvailable())
            {
                PrizePlacement.Prize?.Add();
            }
        }

        /// <summary>
        /// Subscribes to the PropertyChanged event on the IAutoTrackValue interface.
        /// </summary>
        /// <param name="sender">
        /// The sending object of the event.
        /// </param>
        /// <param name="e">
        /// The arguments of the PropertyChanged event.
        /// </param>
        private void OnAutoTrackValueChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName != nameof(IAutoTrackValue.CurrentValue))
            {
                return;
            }
            
            AutoTrackUpdate();
        }

        /// <summary>
        /// Updates the section value from the auto-tracked value.
        /// </summary>
        private void AutoTrackUpdate()
        {
            if (!_autoTrackValue!.CurrentValue.HasValue)
            {
                return;
            }

            if (Available == 1 - _autoTrackValue.CurrentValue.Value)
            {
                return;
            }
            
            Available = 1 - _autoTrackValue.CurrentValue.Value;
            _saveLoadManager.Unsaved = true;
        }

        /// <summary>
        /// Returns whether the section can be cleared or collected.
        /// </summary>
        /// <returns>
        /// A boolean representing whether the section can be cleared or collected.
        /// </returns>
        public override bool CanBeCleared(bool force)
        {
            if (IsAvailable() && _alwaysClearable)
            {
                return true;
            }

            return base.CanBeCleared(force);
        }

        /// <summary>
        /// Returns a new undoable action to toggle the prize.
        /// </summary>
        /// <param name="force">
        /// A boolean representing whether to ignore the logic.
        /// </param>
        /// <returns>
        /// An undoable action to toggle the prize.
        /// </returns>
        public IUndoable CreateTogglePrizeSectionAction(bool force)
        {
            return _togglePrizeSectionFactory(this, force);
        }
    }
}
