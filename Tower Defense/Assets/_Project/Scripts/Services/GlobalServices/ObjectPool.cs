using System;
using System.Collections.Generic;
using UnityEngine;
using _Project.Scripts.GameEntities;

namespace _Project.Scripts.Services
{
    public sealed class ObjectPool<T> where T : MonoBehaviour, IPoolable<T>
    {
        private const int DEFAULT_CAPACITY = 8;

        private readonly Stack<T> _pool;
        private readonly Func<T> _create;

        private readonly int _capacity;

        public ObjectPool(Func<T> create, int capacity = DEFAULT_CAPACITY)
        {
            _create = create;
            _capacity = capacity;
            _pool = new Stack<T>(_capacity);
        }

        public T Get(Vector2 position)
        {
            if (_pool.Count == 0)
                Fill();

            var obj = _pool.Pop();
            obj.transform.position = position;
            obj.gameObject.SetActive(true);
            obj.OnSpawned();
            return obj;
        }

        public void Return(T obj)
        {
            obj.gameObject.SetActive(false);
            _pool.Push(obj);
        }

        private void Fill()
        {
            for (int i = 0; i < _capacity; i++)
            {
                var obj = _create();
                obj.SetReturnDelegate(Return);
                obj.gameObject.SetActive(false);
                _pool.Push(obj);
            }
        }
    }
}
