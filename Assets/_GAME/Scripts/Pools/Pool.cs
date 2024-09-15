using System.Collections.Generic;
using System.Linq;
using _GAME.Scripts.Base;
using _Game.Scripts.Tools;
using _Game.Scripts.View;
using UnityEngine;

namespace _GAME.Scripts.Pools
{
    public class Pool : MonoBehaviour
    {
        [SerializeField] protected int itemCount = 50;

        [SerializeField] protected BaseView baseView;

        public List<BaseView> listOfItems;

        public virtual void Initialize()
        {
            Init(itemCount);
        }

        protected virtual void Init(int itemsCount)
        {
            for (var i = 0; i < itemsCount; i++)
            {
                var item = Instantiate(baseView, transform);
                item.Init();
                item.Deactivate();
                listOfItems.Add(item);
            }
        }

        public virtual BaseView GetItem()
        {
            if (listOfItems.Count == 0)
            {
                Init(100);
            }

            var item = listOfItems.First();
            listOfItems.RemoveAt(0);
            item.Activate();
            return item;
        }

        public virtual void DeSpawn(BaseView item)
        {
            item.Reset();
            listOfItems.Add(item);
            item.Deactivate();
        }
    }
    
    public interface IPoolClaimer
    {
        public void GetPool(PoolHandler pool);
    }
}