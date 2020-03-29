﻿using OpenTracker.Interfaces;
using OpenTracker.Models;
using OpenTracker.Models.Enums;

namespace OpenTracker.Actions
{
    public class ChangeDungeonItemShuffle : IUndoable
    {
        private readonly Mode _mode;
        private readonly DungeonItemShuffle _dungeonItemShuffle;
        private DungeonItemShuffle _previousDungeonItemShuffle;

        public ChangeDungeonItemShuffle(Mode mode, DungeonItemShuffle dungeonItemShuffle)
        {
            _mode = mode;
            _dungeonItemShuffle = dungeonItemShuffle;
        }

        public void Execute()
        {
            _previousDungeonItemShuffle = _mode.DungeonItemShuffle.Value;
            _mode.DungeonItemShuffle = _dungeonItemShuffle;
        }

        public void Undo()
        {
            _mode.DungeonItemShuffle = _previousDungeonItemShuffle;
        }
    }
}
