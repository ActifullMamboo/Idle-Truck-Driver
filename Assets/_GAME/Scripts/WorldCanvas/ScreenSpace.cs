using System.Collections.Generic;
using System.Linq;
using _Game.Scripts.Ui.Base;
using _GAME.Scripts.UI.MoveTargets;
using UnityEngine;

namespace _GAME.Scripts.WorldCanvas
{
    public class ScreenSpace : BaseUIView
    {
        private List<BaseWindow> _windows = new();
        private List<UIMoveTarget> _moveTargets = new List<UIMoveTarget>();

        public void Init()
        {
            _windows = GetComponentsInChildren<BaseWindow>(true).ToList();
            _moveTargets = GetComponentsInChildren<UIMoveTarget>(true).ToList();

            for (var i = 0; i < _windows.Count; i++) _windows[i].Init();

        }

        public BaseWindow GetWindow<T>()
        {
            var window = _windows.Find(w => w.GetType() == typeof(T));
            return window;
        }
        public List<BaseWindow> GetWindows<T>()
        {
            var baseWindows = _windows.FindAll(w => w.GetType() == typeof(T));
            return baseWindows;
        }

        public Transform GetMoveParentForParticles(UITargetType type)
        {
            var target = _moveTargets.Find(x => x.targetType == type);
            return target.transform;
        }
    }

    public interface IScreenSpaceClaimer
    {
        public void ClaimScreenSpaceCanvas(ScreenSpace screenSpace);
    }
}