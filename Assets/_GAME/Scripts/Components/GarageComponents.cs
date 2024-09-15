using System.Collections.Generic;
using System.Linq;
using _GAME.Scripts.Configs;
using _Game.Scripts.Systems;
using _GAME.Scripts.Upgrades;
using _GAME.Scripts.WorldCanvas;
using UnityEngine;

namespace _GAME.Scripts.Components
{
    public class GarageComponents : MonoBehaviour
    {
        [SerializeField] private GameObject Root;
        [SerializeField] private SoundSystem _soundSystem;
        [SerializeField] private ScreenSpace _screenSpaceCanvas;
        [SerializeField] private CurrensySystem currensySystem;

        private List<IComponentInitializer> _components = new();
        private List<ISoundPlayer> _soundPlayers = new();
        private List<ILevelStartComponents> _levelStartComponents = new();

        [SerializeField] private List<UpgradableItem> _upgradables;
        [SerializeField] private List<CarConfig> configs;

        private void Awake()
        {
            //GetComponentInChildren<LevelManager>().InitStartLevel();

            // DOTween.Init(true, true, LogBehaviour.ErrorsOnly).SetCapacity(2000, 100);

            _soundPlayers = Root.GetComponentsInChildren<ISoundPlayer>(true).ToList();
            for (var i = 0; i < _soundPlayers.Count; i++) _soundPlayers[i].InitSound(_soundSystem);

            for (var i = 0; i < _upgradables.Count; i++) _upgradables[i].Init();
            for (var i = 0; i < configs.Count; i++) configs[i].Init();
            _screenSpaceCanvas.Init();

            var currComp = Root.GetComponentsInChildren<ICurrencyComponent>(true).ToList();
            var sComponents = Root.GetComponentsInChildren<IScreenSpaceClaimer>(true).ToList();
            _components = Root.GetComponentsInChildren<IComponentInitializer>(true).ToList();
            _levelStartComponents = GetComponentsInChildren<ILevelStartComponents>(true).ToList();
            currensySystem.Initialize();
            for (var i = 0; i < currComp.Count; i++) currComp[i].SetCurrencySystem(currensySystem);


            for (var i = 0; i < _levelStartComponents.Count; i++) _levelStartComponents[i].Initialize();
            for (var i = 0; i < _components.Count; i++) _components[i].Initialize();
            for (var i = 0; i < sComponents.Count; i++) sComponents[i].ClaimScreenSpaceCanvas(_screenSpaceCanvas);
        }
    }
}