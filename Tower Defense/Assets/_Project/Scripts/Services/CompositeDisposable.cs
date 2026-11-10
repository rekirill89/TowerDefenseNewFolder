using System;
using System.Collections.Generic;

namespace _Project.Scripts.Services
{
    public class CompositeDisposable : IDisposable
    {
        private readonly List<IDisposable> _disposables = new List<IDisposable>();
        
        public void Add(IDisposable disposable)
        {
            if (disposable != null && !_disposables.Contains(disposable))
            {
                _disposables.Add(disposable);
            }
        }
        
        public void Add(IEnumerable<IDisposable> disposables)
        {
            if (disposables != null)
            {
                _disposables.AddRange(disposables);
            }
        }
        
        public void Dispose()
        {
            foreach (var disposable in _disposables)
            {
                disposable.Dispose();
            }
            _disposables.Clear();
        }
    }
}