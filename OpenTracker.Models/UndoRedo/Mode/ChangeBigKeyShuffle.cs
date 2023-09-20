﻿using OpenTracker.Models.Modes;
using OpenTracker.Utils.Autofac;

namespace OpenTracker.Models.UndoRedo.Mode;

/// <summary>
/// This class contains the <see cref="IUndoable"/> action to change the <see cref="IMode.BigKeyShuffle"/> property.
/// </summary>
[DependencyInjection]
public sealed class ChangeBigKeyShuffle : IChangeBigKeyShuffle
{
    private readonly IMode _mode;

    private readonly bool _newValue;

    private bool _previousValue;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="mode">
    ///     The <see cref="IMode"/> data.
    /// </param>
    /// <param name="newValue">
    ///     A <see cref="bool"/> representing the new <see cref="IMode.BigKeyShuffle"/> value.
    /// </param>
    public ChangeBigKeyShuffle(IMode mode, bool newValue)
    {
        _mode = mode;
        _newValue = newValue;
    }

    public bool CanExecute()
    {
        return true;
    }

    public void ExecuteDo()
    {
        _previousValue = _mode.BigKeyShuffle;
        _mode.BigKeyShuffle = _newValue;
    }

    public void ExecuteUndo()
    {
        _mode.BigKeyShuffle = _previousValue;
    }
}