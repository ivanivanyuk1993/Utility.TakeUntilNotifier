using System.Reactive;
using System.Reactive.Subjects;
using RunIfFirstCallProviderNS;

namespace TakeUntilNotifierNS
{
    public class TakeUntilNotifier :
        IDisposable,
        IObservable<Unit>
    {
        private readonly Subject<Unit> _takeUntilSubject = new();
        private readonly RunIfFirstCallProvider _disposeIfFirstCallProvider;

        public TakeUntilNotifier()
        {
            _disposeIfFirstCallProvider = new RunIfFirstCallProvider(action: DisposeWhichShouldRunOnce);
        }

        public void Dispose()
        {
            _disposeIfFirstCallProvider.RunIfFirstCall();
        }

        public void Notify()
        {
            _takeUntilSubject.OnNext(Unit.Default);
        }

        public IDisposable Subscribe(IObserver<Unit> observer)
        {
            return _takeUntilSubject.Subscribe(observer);
        }

        private void DisposeWhichShouldRunOnce()
        {
            Notify();
            _takeUntilSubject.OnCompleted();
            _takeUntilSubject.Dispose();
        }
    }
}
