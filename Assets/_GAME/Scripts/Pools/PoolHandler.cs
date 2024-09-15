using System.Linq;
using UnityEngine;

namespace _GAME.Scripts.Pools
{
    public class PoolHandler : MonoBehaviour
    {
        [SerializeField] private Pool[] pools;

        public void Initialize()
        {
            for (int i = 0; i < pools.Length; i++)
            {
                pools[i].Initialize();
            }
        }

        public Pool GetPool<T>()
        {
            var pool = pools.FirstOrDefault(w => w.GetType() == typeof(T));
            return pool == null ? null : pool;
        }
    }
}
