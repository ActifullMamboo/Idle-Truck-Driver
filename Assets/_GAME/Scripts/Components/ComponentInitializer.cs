using System.Collections.Generic;
using System.Linq;
using _GAME.Scripts.AI;
using _GAME.Scripts.AI.Base;
using _GAME.Scripts.Base;
using _GAME.Scripts.Other;
using _GAME.Scripts.Pools;
using _Game.Scripts.Systems;
using _GAME.Scripts.UI.WorldSpace;
using _GAME.Scripts.Upgrades;
using _GAME.Scripts.WorldCanvas;
using DG.Tweening;
using UnityEngine;

namespace _GAME.Scripts.Components
{
    public class ComponentInitializer : MonoBehaviour
    {
        [SerializeField] private GameObject Root;
        [SerializeField] private SoundSystem _soundSystem;
        [SerializeField] private ScreenSpace _screenSpaceCanvas;
        [SerializeField] private PoolHandler poolHandler;
        [SerializeField] private CurrensySystem _currensySystem;
        [SerializeField] private PointsSystem pointsSystem;
        [SerializeField] private CharactersControllSystem charactersControllSystem;
       
        private List<IComponentInitializer> _components = new();
        private List<ISoundPlayer> _soundPlayers = new();
        private List<ILevelStartComponents> _levelStartComponents = new();

        [SerializeField] private List<UpgradableItem> _upgradables;

        private void Awake()
        {
            GetComponentInChildren<LevelManager>().InitStartLevel();
            
           // DOTween.Init(true, true, LogBehaviour.ErrorsOnly).SetCapacity(2000, 100);
            
            _soundPlayers = Root.GetComponentsInChildren<ISoundPlayer>(true).ToList();
            for (var i = 0; i < _soundPlayers.Count; i++) _soundPlayers[i].InitSound(_soundSystem);

            for (var i = 0; i < _upgradables.Count; i++) _upgradables[i].Init();
             _screenSpaceCanvas.Init();
             _currensySystem.Initialize();
             poolHandler.Initialize();
             var pools = Root.GetComponentsInChildren<IPoolClaimer>(true);
             for (int i = 0; i < pools.Length; i++)
             {
                 pools[i].GetPool(poolHandler);
             }
            _components = Root.GetComponentsInChildren<IComponentInitializer>(true).ToList();
            _levelStartComponents = GetComponentsInChildren<ILevelStartComponents>().ToList();
            pointsSystem.Initialize(Root);
            //charactersControllSystem.InitSceneObjects(pointsSystem);

            for (var i = 0; i < _levelStartComponents.Count; i++) _levelStartComponents[i].Initialize();
            for (var i = 0; i < _components.Count; i++) _components[i].Initialize();
            var screenSpaces = Root.GetComponentsInChildren<IScreenSpaceClaimer>(true);
            for (int i = 0; i < screenSpaces.Length; i++)
            {
                screenSpaces[i].ClaimScreenSpaceCanvas(_screenSpaceCanvas);
            }
            
            
            var curensyC = Root.GetComponentsInChildren<ICurrencyComponent>(true);

            for (int i = 0; i < curensyC.Length; i++)
            {
                curensyC[i].SetCurrencySystem(_currensySystem);
            }


        }

    }

    public interface IComponentInitializer
    {
        public void Initialize();
    }

    public interface ILevelStartComponents
    {
        public void Initialize();
    }
}