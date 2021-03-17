﻿using System.Collections.Generic;
using OpenTracker.Models.DungeonItems;
using OpenTracker.Models.DungeonNodes;
using OpenTracker.Models.KeyDoors;
using OpenTracker.Models.Locations;

namespace OpenTracker.Models.Dungeons
{
    /// <summary>
    /// This interface contains the mutable dungeon data.
    /// </summary>
    public interface IMutableDungeon
    {
        LocationID ID { get; }
        IDungeonNodeDictionary Nodes { get; }
        IDungeonItemDictionary DungeonItems { get; }
        IKeyDoorDictionary KeyDoors { get; }

        delegate IMutableDungeon Factory(IDungeon dungeon);

        void ApplyState(IDungeonState state);
        List<KeyDoorID> GetAccessibleKeyDoors(bool sequenceBreak = false);
        int GetAvailableSmallKeys(bool sequenceBreak = false);
        IDungeonResult GetDungeonResult(IDungeonState state);
        bool ValidateKeyLayout(IDungeonState state);
        void Reset();
    }
}