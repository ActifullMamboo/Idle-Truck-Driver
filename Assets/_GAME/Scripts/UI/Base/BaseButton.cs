using System;
using _Game.Scripts.Tools;
using _Game.Scripts.Ui.Buttons;
using _Game.Scripts.Systems;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace _Game.Scripts.Ui.Base
{
	[ExecuteInEditMode]
	public class BaseButton : BaseUIView,
		IPointerEnterHandler,
		IPointerExitHandler,
		IPointerDownHandler,
		IPointerUpHandler
	{
		public delegate void ButtonEvent(BaseButton button);
		
		public static event Action<int> ClickSoundEvent = delegate { };
		public static ButtonEvent AnyButtonClickedEvent = delegate{ };

		[SerializeField] protected Button _button;
		[SerializeField] protected int _clickSound = -1;
		
		[Header("ViewSettings")]
		[SerializeField] protected Image _image;
		[SerializeField] protected Image _back;
		[SerializeField] protected Image _front1;

		[Header("ColorSettings")]
		[SerializeField] private Color _interactableColorBack;
		[SerializeField] private Color _interactableColorFront1;
		[SerializeField] private Color _unInteractableColorBack;
		[SerializeField] private Color _unInteractableColorFront1;

		[SerializeField] protected TextMeshProUGUI[] _texts;
		[SerializeField] private bool _ignoreInteractableBlind;
		
		private static BaseButton _allowedTutorialButton;
		
		private bool Interactable => _button.interactable;
		private static bool IsBlocked = false;
		
		private static BaseButton _currentOverlapped;
		private BlindEffect _blindEffect;
		
		public Action Callback;
		public Action OnExit { get; set; }
		
		public Action OnDown { get; set; }
		public Action OnUp { get; set; }
		public Color InteractableColorBack => _interactableColorBack;
		public Color UnInteractableColorBack => _unInteractableColorBack;

		public static bool IsOver => _currentOverlapped != null;
		public static int LastInteractionFrame { get; private set; }

#if UNITY_EDITOR
		protected void Awake()
		{
			if (Application.isPlaying) return;

			if (_image == null) _image = GetComponent<Image>();
			if (_button == null) _button = GetComponentInChildren<Button>();
			_texts = GetComponentsInChildren<TextMeshProUGUI>(true);
			_blindEffect ??= GetComponentInChildren<BlindEffect>(true);
		}
#endif

		protected virtual void OnEnable()
		{
			if (_button == null) _button = GetComponent<Button>();
			if (_button == null) return;
			_button.onClick.RemoveAllListeners();
			_button.onClick.AddListener(OnClick);
		}
		
		public void SetText(string text)
		{
			if (_texts.Length < 1) return;
			_texts[0].text = text;
		}
		
		public void SetText(int element, string text)
		{
			if (_texts.Length < element) return;
			_texts[element].text = text;
		}

		public void SetTextColor(int element, Color color)
		{
			if (_texts.Length < element) return;
			_texts[element].color = color;
		}
		
		public BaseButton SetSprite(Sprite sprite)
		{
			_image.sprite = sprite;
			return this;
		}

		public void SetColors(bool interactable)
		{
			if (interactable)
			{
				RestoreColors();
			}
			else
			{
				SetUnInteractableColors();
			}
		}

		public void SetColors(Color image = default, Color back = default, Color front1 = default)
		{
			if (image != default) _image.color = image;
			if (back != default) _back.color = back;
			if (front1 != default) _front1.color = front1;
		}
		
		public void SetColorFront(Color front)
		{
			if (front != default) _front1.color = front;
		}

		public void SetColorBack(Color back)
		{
			if (back != default) _back.color = back;
		}

		public void RestoreColors()
		{
			if (_front1 != default) _front1.color = _interactableColorFront1;
			if (_back != default) _back.color = _interactableColorBack;
		}
		
		public void SetUnInteractableColors()
		{
			if (_front1 != default) _front1.color = _unInteractableColorFront1;
			if (_back != default) _back.color = _unInteractableColorBack;
		}
		
		protected void ShowImage()
		{
			if (_image != null) _image.Activate();
		}

		protected void HideImage()
		{
			if (_image != null) _image.Deactivate();
		}

		public BaseButton SetCallback(Action callback)
		{
			Callback = callback;
			return this;
		}

		public void SetInteractable(bool interactable)
		{
			_button.interactable = interactable;
			if (!_ignoreInteractableBlind)
			{
				_blindEffect ??= GetComponentInChildren<BlindEffect>(true);
				if(_blindEffect) _blindEffect.Init(interactable);
			}
			if (interactable)
			{
				if (_back != null) _back.color = _interactableColorBack;
				if (_front1 != null) _front1.color = _interactableColorFront1;
			}
			else
			{
				if (_back != null) _back.color = _unInteractableColorBack;
				if (_front1 != null) _front1.color = _unInteractableColorFront1;
			}
		}

		public void SetBlind(bool value)
		{
			_blindEffect ??= GetComponentInChildren<BlindEffect>(true);
			if(_blindEffect) _blindEffect.Init(value);
		}

		public void SetInteractableColor()
		{
			_back.color = _unInteractableColorBack;
			if (_front1 != null) _front1.color = _unInteractableColorFront1;
		}

		private void RegisterLastInteraction()
		{
			LastInteractionFrame = Time.frameCount;
		}

		public virtual void OnPointerEnter(PointerEventData eventData)
		{
			_currentOverlapped = this;
			RegisterLastInteraction();
		}

		public virtual void OnPointerExit(PointerEventData eventData)
		{
			OnOverlapEnded();
			OnExit?.Invoke();
		}
		
		public virtual void OnPointerDown(PointerEventData eventData)
		{
			RegisterLastInteraction();
			OnDown?.Invoke();
		}

		public virtual void OnPointerUp(PointerEventData eventData)
		{
			RegisterLastInteraction();
			OnUp?.Invoke();
		}

		private void OnDestroy()
		{
			OnOverlapEnded();
		}

		protected virtual void OnDisable()
		{
			OnOverlapEnded();
		}

		private void OnOverlapEnded()
		{
			RegisterLastInteraction();
			if (_currentOverlapped != this) return;
			_currentOverlapped = null;
		}

		protected virtual void OnClick()
		{
			if (!Interactable || IsBlocked) return;

			if (_allowedTutorialButton != null)
			{
				if (_allowedTutorialButton != this) return;
				_allowedTutorialButton = null;
			}

			Callback?.Invoke();
			AnyButtonClickedEvent?.Invoke(this);

			if (_clickSound == -1) _clickSound = 2;
			PlaySound((GameSoundType)_clickSound);
		}

		public void SimulateClick()
		{
			OnClick();
		}

		public void PlaySound(GameSoundType type)
		{
			ClickSoundEvent?.Invoke((int)type);
		}

		public static void SetAllowedButton(BaseButton button)
		{
			_allowedTutorialButton = button;
		}

		public static void BlockAll()
        {
			IsBlocked = true;
		}
		public static void UnlockAll()
		{
			IsBlocked = false;
		}
	}
} 