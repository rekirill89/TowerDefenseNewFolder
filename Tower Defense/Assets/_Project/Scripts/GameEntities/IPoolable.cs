using System;
using UnityEngine;

namespace _Project.Scripts
{
    public interface IPoolable<T> where T : MonoBehaviour
    {
        public void SetReturnDelegate(Action<T> returnDelegate);

        public void OnSpawned();
    }
}
