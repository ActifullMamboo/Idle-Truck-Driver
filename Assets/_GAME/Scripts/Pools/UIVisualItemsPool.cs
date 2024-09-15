using System.Collections.Generic;
using System.Linq;
using _GAME.Scripts.Base;
using _Game.Scripts.Tools;
using _GAME.Scripts.UI.MoveTargets;
using _Game.Scripts.View;
using UnityEngine;

namespace _GAME.Scripts.Pools
{
    public class UIVisualItemsPool : Pool
    {
        
        protected override void Init(int itemsCount)
        {
            base.Init(itemsCount);
        }
    
        private void SpawnSpecial(UITargetType itemType, int count)
        {
            var newItems = new List<BaseView>();
            for (var i = 0; i < count; i++)
            {
                var item = Instantiate(baseView, transform);
                item.Init();
                item.Deactivate();
                listOfItems.Add(item);
                newItems.Add(item);
            }
                
            var items = newItems.ConvertAll(itm => (UIVisualItem)itm);
    
            for (int i = 0; i < items.Count; i++)
            {
                items[i].SetupItemView(itemType);
            }
        }
    
        public UIVisualItem GetItem(UITargetType type)
        {
            var items = listOfItems.ConvertAll(itm => (UIVisualItem)itm);
    
            var item = items.FirstOrDefault(x => !x.isActiveAndEnabled);
            if (item!=null)
            {
                item.SetupItemView(type);
    
            }
    
            if (item==null)
            {
                SpawnSpecial(type, 500);
                Debug.Log("SPAWNED MORE 500 "+ type  );
                items = listOfItems.ConvertAll(itm => (UIVisualItem)itm);
                item = items.Find(x => x.ItemConfig.TargetType == type);
    
            }
    
            listOfItems.Remove(item);
                
            item.Activate();
                
            return item;
        }
    }
}
