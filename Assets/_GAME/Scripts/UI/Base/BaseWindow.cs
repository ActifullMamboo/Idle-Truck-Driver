using System;
using System.Collections.Generic;
using System.Linq;
using _Game.Scripts.Systems;
using _Game.Scripts.Tools;
using _Game.Scripts.Ui.Base.Animations;
using _Game.Scripts.Ui.Buttons;
using UnityEngine;

namespace _Game.Scripts.Ui.Base
{
    public class BaseWindow : BaseUIView
    {
        private enum WindowState
        {
            Opened,
            Closed,
            PlayingAnim
        }

        public static event Action<int> SoundEvent = delegate { };
        
        public Action<BaseWindow> Opened;
        public Action<BaseWindow> Closed;
     
        [SerializeField] private bool _addToOpenedStack = true;
        [SerializeField] private bool _toOpenedOverStack;
        [SerializeField] private bool _ignoreAnimations;
        [SerializeField] private bool _ignoreInitClose;
        [SerializeField] private bool _renderUiOnly;
        
        private const string INNER_WINDOW = "Window";
        
        private WindowState _state;

        private CloseWindowButton _closeButton;
        private WindowBack _windowBack;
        protected List<BaseUIAnimation> _windowAnimations = new();
        
        protected RectTransform WindowRect;

        public bool Inited { get; private set; }
        public bool AddToOpenedStack => _addToOpenedStack;
        public bool ToOpenOverStack => _toOpenedOverStack;
        public bool IsOpened => _state == WindowState.Opened;
        public bool IsClosed => _state == WindowState.Closed;
        public bool RenderUiOnly => _renderUiOnly;
        public BaseButton CloseButton => _closeButton;

        public virtual void Init()
        {
            _state = WindowState.Closed;
            
            _closeButton = GetComponentInChildren<CloseWindowButton>(true);
            _windowBack = GetComponentInChildren<WindowBack>(true);
            
            if (!_ignoreAnimations)
            {
                _windowAnimations = GetComponentsInChildren<BaseUIAnimation>().ToList();
               
                foreach (var windowAnimation in _windowAnimations)
                {
                    windowAnimation.Init();
                }
            }
            
            foreach (Transform child in transform)
            {
                if (child.name != INNER_WINDOW) continue;
                WindowRect = child as RectTransform;
                break;
            }
            
            if (_closeButton != null) _closeButton.SetCallback(Close);
            if (_windowBack != null && _closeButton != null) _windowBack.Init(_closeButton);
            if (!_ignoreInitClose) this.Deactivate();

            Inited = true;
        }

        public virtual void Open(params object[] list)
        {
            SetState(WindowState.Opened);
            if (_windowAnimations.Count == 0)
            {
                this.Activate();
                OnOpened();
            }
            else
            {
                this.Activate();
                foreach (var windowAnim in _windowAnimations)
                {
                    windowAnim.PlayOpenAnimation(WindowRect, OnOpened);   
                }
            }
          
        }

    private void OnOpened()
        {
            Opened?.Invoke(this);
        }

        public virtual void Close()
        {
            PlaySound(GameSoundType.Window);
            if (_windowAnimations.Count == 0)
            {
                OnClosed();
            }
            else
            {
                foreach (var windowAnim in _windowAnimations)
                {
                    windowAnim.PlayCloseAnimation(WindowRect, OnClosed);   
                }
            }
        }

        private void OnClosed()
        {
            this.Deactivate();
            SetState(WindowState.Closed);
            Closed?.Invoke(this);
        }

        private void SetState(WindowState state)
        {
            _state = state;
        }

        public virtual void UpdateLocalization()
        {
        }
        
        public virtual void Tick(float deltaTime)
        {
        }

        public virtual void ResetGameLogic()
        {
        }

        protected void PlaySound(GameSoundType type)
        {
            SoundEvent?.Invoke((int)type);
        }

        public virtual BaseButton GetTutorialButton(int value = 0)
        {
            return null;
        }
    }
}