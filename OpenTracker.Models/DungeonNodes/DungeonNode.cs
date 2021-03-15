﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using OpenTracker.Models.AccessibilityLevels;
using OpenTracker.Models.Dungeons;
using OpenTracker.Models.NodeConnections;
using OpenTracker.Models.RequirementNodes;
using ReactiveUI;

namespace OpenTracker.Models.DungeonNodes
{
    /// <summary>
    /// This class contains the dungeon requirement node data.
    /// </summary>
    public class DungeonNode : ReactiveObject, IDungeonNode
    {
        private readonly IDungeonNodeFactory _factory;
        private readonly IMutableDungeon _dungeonData;
#if DEBUG        
        private readonly DungeonNodeID _id;
#endif

        public int ExitsAccessible { get; set; }
        public int DungeonExitsAccessible { get; set; }
        public int InsanityExitsAccessible { get; set; }

        public List<INodeConnection> Connections { get; } = new List<INodeConnection>();

        public event EventHandler? ChangePropagated;

        private bool _alwaysAccessible;
        public bool AlwaysAccessible
        {
            get => _alwaysAccessible;
            set => this.RaiseAndSetIfChanged(ref _alwaysAccessible, value);
        }

        private AccessibilityLevel _accessibility;
        public AccessibilityLevel Accessibility
        {
            get => _accessibility;
            private set
            {
                if (_accessibility == value)
                {
                    return;
                }
                
                this.RaiseAndSetIfChanged(ref _accessibility, value);
                ChangePropagated?.Invoke(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="factory">
        /// The dungeon node factory for creating node connections.
        /// </param>
        /// <param name="id">
        /// The node identity.
        /// </param>
        /// <param name="dungeonData">
        /// The mutable dungeon data parent class.
        /// </param>
        public DungeonNode(IDungeonNodeFactory factory, IMutableDungeon dungeonData, DungeonNodeID id)
        {
            _factory = factory;
            _dungeonData = dungeonData;
#if DEBUG
            _id = id;
#endif

            PropertyChanged += OnPropertyChanged;
            dungeonData.Nodes.ItemCreated += OnNodeCreated;
        }

        /// <summary>
        /// Subscribes to the PropertyChanged event on this object.
        /// </summary>
        /// <param name="sender">
        /// The sending object of the event.
        /// </param>
        /// <param name="e">
        /// The arguments of the PropertyChanged event.
        /// </param>
        private void OnPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(AlwaysAccessible):
                    UpdateAccessibility();
                    break;
            }
        }

        /// <summary>
        /// Subscribes to the NodeCreated event on the RequirementNodeDictionary class.
        /// </summary>
        /// <param name="sender">
        /// The sending object of the event.
        /// </param>
        /// <param name="e">
        /// The arguments of the NodeCreated event.
        /// </param>
        private void OnNodeCreated(object? sender, KeyValuePair<DungeonNodeID, IDungeonNode> e)
        {
            if (e.Value != this)
            {
                return;
            }
            
            var nodes = sender as IDungeonNodeDictionary ??
                throw new ArgumentNullException(nameof(sender));

            nodes.ItemCreated -= OnNodeCreated;

            _factory.PopulateNodeConnections(_dungeonData, e.Key, this, Connections);

            UpdateAccessibility();

            foreach (var connection in Connections)
            {
                connection.PropertyChanged += OnConnectionChanged;
            }
        }

        /// <summary>
        /// Subscribes to the PropertyChanged event on the RequirementNode, DungeonNode,
        /// IKeyDoor, and IRequirement classes/interfaces.
        /// </summary>
        /// <param name="sender">
        /// The sending object of the event.
        /// </param>
        /// <param name="e">
        /// The arguments of the PropertyChanged event.
        /// </param>
        private void OnConnectionChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(INodeConnection.Accessibility))
            {
                UpdateAccessibility();
            }
        }

        /// <summary>
        /// Updates the accessibility of the node.
        /// </summary>
        private void UpdateAccessibility()
        {
            Accessibility = GetNodeAccessibility(new List<IRequirementNode>());
        }

        /// <summary>
        /// Returns the node accessibility.
        /// </summary>
        /// <param name="excludedNodes">
        /// The list of node IDs from which to not check accessibility.
        /// </param>
        /// <returns>
        /// The accessibility of the node.
        /// </returns>
        public AccessibilityLevel GetNodeAccessibility(List<IRequirementNode> excludedNodes)
        {
            if (AlwaysAccessible)
            {
                return AccessibilityLevel.Normal;
            }

            var finalAccessibility = AccessibilityLevel.None;

            foreach (var connection in Connections)
            {
                finalAccessibility = AccessibilityLevelMethods.Max(
                    finalAccessibility, connection.GetConnectionAccessibility(excludedNodes));

                if (finalAccessibility == AccessibilityLevel.Normal)
                {
                    break;
                }
            }

            return finalAccessibility;
        }

        /// <summary>
        /// Resets the dungeon node for testing purposes.
        /// </summary>
        public void Reset()
        {
            AlwaysAccessible = false;
        }
    }
}
