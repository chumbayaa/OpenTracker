using RxVoid = ReactiveUI.Primitives.RxVoid;
using System.Reactive;
using ReactiveUI;

namespace OpenTracker.ViewModels.Capture;

public interface ICaptureManager
{
    ReactiveCommand<ICaptureWindowVM, RxVoid> OpenCaptureWindow { get; }
    void GenerateInitialData();
}