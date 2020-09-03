﻿using OpenTracker.Models.AccessibilityLevels;
using OpenTracker.Models.Dungeons;
using OpenTracker.Models.KeyDoors;
using OpenTracker.Models.NodeConnections;
using OpenTracker.Models.RequirementNodes;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace OpenTracker.Models.DungeonNodes
{
    /// <summary>
    /// This is the class for dungeon requirement node.
    /// </summary>
    public class DungeonNode : IDungeonNode
    {
        private readonly IMutableDungeon _dungeonData;

        public int ExitsAccessible { get; set; }
        public int KeysProvided { get; }
        public List<INodeConnection> Connections { get; } =
            new List<INodeConnection>();

        public event PropertyChangedEventHandler PropertyChanged;

        private bool _alwaysAccessible;
        public bool AlwaysAccessible
        {
            get => _alwaysAccessible;
            set
            {
                if (_alwaysAccessible != value)
                {
                    _alwaysAccessible = value;
                    UpdateAccessibility();
                }
            }
        }

        private AccessibilityLevel _accessibility;
        public AccessibilityLevel Accessibility
        {
            get => _accessibility;
            private set
            {
                if (_accessibility != value)
                {
                    _accessibility = value;
                    OnPropertyChanged(nameof(Accessibility));
                }
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="id">
        /// The node identity.
        /// </param>
        /// <param name="dungeonData">
        /// The mutable dungeon data parent class.
        /// </param>
        /// <param name="freeKeysProvided">
        /// A 32-bit signed integer representing the number of free keys provided by accessing the
        /// node.
        /// </param>
        /// <param name="entryConnections">
        /// A list of connections to the node from outside the dungeon.
        /// </param>
        /// <param name="keyDoorConnections">
        /// A list of key doors to which this node is connected.
        /// </param>
        /// <param name="dungeonConnections">
        /// A list of non-key door connections to this node from within the dungeon.
        /// </param>
        public DungeonNode(IMutableDungeon dungeonData, int freeKeysProvided)
        {
            _dungeonData = dungeonData ?? throw new ArgumentNullException(nameof(dungeonData));
            KeysProvided = freeKeysProvided;

            _dungeonData.Nodes.NodeCreated += OnNodeCreated;
            _dungeonData.KeyDoorDictionary.DoorCreated += OnDoorCreated;
        }

        /// <summary>
        /// Raises the PropertyChanged event for the specified property.
        /// </summary>
        /// <param name="propertyName">
        /// The string of the property name of the changed property.
        /// </param>
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
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
        private void OnNodeCreated(object sender, KeyValuePair<DungeonNodeID, IDungeonNode> e)
        {
            if (e.Value == this)
            {
                _dungeonData.Nodes.NodeCreated -= OnNodeCreated;
                DungeonNodeFactory.PopulateNodeConnections(
                    e.Key, this, _dungeonData, Connections);
                UpdateAccessibility();

                foreach (var connection in Connections)
                {
                    connection.PropertyChanged += OnConnectionChanged;
                }
            }
        }

        /// <summary>
        /// Subscribes to the DoorCreated event on the KeyDoorDictionary class.
        /// </summary>
        /// <param name="sender">
        /// The sending object of the event.
        /// </param>
        /// <param name="e">
        /// The arguments of the DoorCreated event.
        /// </param>
        private void OnDoorCreated(object sender, KeyValuePair<KeyDoorID, IKeyDoor> e)
        {
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
        private void OnConnectionChanged(object sender, PropertyChangedEventArgs e)
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

            if (excludedNodes == null)
            {
                throw new ArgumentNullException(nameof(excludedNodes));
            }

            AccessibilityLevel finalAccessibility = AccessibilityLevel.None;

            if (excludedNodes.Count == 0)
            {
                foreach (var connection in Connections)
                {
                    finalAccessibility = AccessibilityLevelMethods.Max(
                        finalAccessibility, connection.Accessibility);

                    if (finalAccessibility == AccessibilityLevel.Normal)
                    {
                        break;
                    }
                }

                return finalAccessibility;
            }

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

        public void Reset()
        {
            AlwaysAccessible = false;
        }
    }
}
