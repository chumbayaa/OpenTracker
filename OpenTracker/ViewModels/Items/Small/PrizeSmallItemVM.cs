﻿using OpenTracker.Interfaces;
using OpenTracker.Models.UndoRedo;
using OpenTracker.Models.Sections;
using ReactiveUI;
using System.ComponentModel;
using System.Text;
using OpenTracker.Models.PrizePlacements;
using OpenTracker.Models.Items;
using System.Linq;
using System.Reactive;
using Avalonia.Input;
using OpenTracker.Utils;

namespace OpenTracker.ViewModels.Items.Small
{
    /// <summary>
    /// This class contains dungeon prize small items panel control ViewModel data.
    /// </summary>
    public class PrizeSmallItemVM : ViewModelBase, ISmallItemVMBase
    {
        private readonly IPrizeDictionary _prizes;
        private readonly IUndoRedoManager _undoRedoManager;
        private readonly IUndoableFactory _undoableFactory;

        private readonly IPrizeSection _section;

        public string ImageSource
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("avares://OpenTracker/Assets/Images/Prizes/");

                if (_section.PrizePlacement.Prize == null)
                {
                    sb.Append("unknown");
                }
                else
                {
                    sb.Append(
                        _prizes.FirstOrDefault(
                            x => x.Value == _section.PrizePlacement.Prize).Key.ToString()
                                .ToLowerInvariant());
                }

                sb.Append(_section.IsAvailable() ? "0" : "1");
                sb.Append(".png");

                return sb.ToString();
            }
        }
        
        public ReactiveCommand<PointerReleasedEventArgs, Unit> HandleClickCommand { get; }

        public delegate PrizeSmallItemVM Factory(IPrizeSection section);

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="prizes">
        /// The prizes dictionary.
        /// </param>
        /// <param name="undoRedoManager">
        /// The undo/redo manager.
        /// </param>
        /// <param name="undoableFactory">
        /// A factory for creating undoable actions.
        /// </param>
        /// <param name="section">
        /// The prize section to be represented.
        /// </param>
        public PrizeSmallItemVM(
            IPrizeDictionary prizes, IUndoRedoManager undoRedoManager,
            IUndoableFactory undoableFactory, IPrizeSection section)
        {
            _prizes = prizes;
            _undoRedoManager = undoRedoManager;
            _undoableFactory = undoableFactory;

            _section = section;
            
            HandleClickCommand = ReactiveCommand.Create<PointerReleasedEventArgs>(HandleClick);

            _section.PropertyChanged += OnSectionChanged;
            _section.PrizePlacement.PropertyChanged += OnPrizeChanged;
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
        private void OnPrizeChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(IPrizePlacement.Prize))
            {
                this.RaisePropertyChanged(nameof(ImageSource));
            }
        }

        /// <summary>
        /// Subscribes to the PropertyChanged event on the ISection interface.
        /// </summary>
        /// <param name="sender">
        /// The sending object of the event.
        /// </param>
        /// <param name="e">
        /// The arguments of the PropertyChanged event.
        /// </param>
        private void OnSectionChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(ISection.Available))
            {
                this.RaisePropertyChanged(nameof(ImageSource));
            }
        }

        /// <summary>
        /// Creates an undoable action to toggle the prize section and sends it to the undo/redo manager.
        /// </summary>
        private void TogglePrize()
        {
            _undoRedoManager.Execute(_undoableFactory.GetTogglePrize(_section, true));
        }

        /// <summary>
        /// Creates an undoable action to change the prize and sends it to the undo/redo manager.
        /// </summary>
        private void ChangePrize()
        {
            _undoRedoManager.Execute(_undoableFactory.GetChangePrize(_section.PrizePlacement));
        }

        /// <summary>
        /// Handles clicking the control.
        /// </summary>
        /// <param name="e">
        /// The pointer released event args.
        /// </param>
        private void HandleClick(PointerReleasedEventArgs e)
        {
            switch (e.InitialPressMouseButton)
            {
                case MouseButton.Left:
                {
                    TogglePrize();
                }
                    break;
                case MouseButton.Right:
                {
                    ChangePrize();
                }
                    break;
            }
        }
    }
}
