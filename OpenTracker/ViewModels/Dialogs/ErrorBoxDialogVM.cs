﻿using System.Reactive;
using OpenTracker.Autofac;
using OpenTracker.Utils.Dialog;
using ReactiveUI;

namespace OpenTracker.ViewModels.Dialogs;

/// <summary>
/// This is the ViewModel for the error box dialog window.
/// </summary>
[DependencyInjection]
public sealed class ErrorBoxDialogVM : DialogViewModelBase
{
    public string Title { get; }
    public string Text { get; }
    
    public Interaction<Unit, Unit> RequestCloseInteraction { get; } = new(RxApp.MainThreadScheduler);

    public ReactiveCommand<Unit, Unit> OkCommand { get; }

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="title">
    /// A string representing the window title.
    /// </param>
    /// <param name="text">
    /// A string representing the dialog text.
    /// </param>
    public ErrorBoxDialogVM(string title, string text)
    {
        OkCommand = ReactiveCommand.Create(Ok);

        Title = title;
        Text = text;
    }

    /// <summary>
    /// Selects Ok to the dialog.
    /// </summary>
    private void Ok()
    {
        Close();
    }
}