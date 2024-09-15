using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace SimpleInputNamespace
{
	public class SteeringWheel : MonoBehaviour, ISimpleInputDraggable
	{
		private SimpleInput.AxisInput axis = new SimpleInput.AxisInput( "Horizontal" );
		public Action OnPress;
		public Action OnRelease;
		public Action<SimpleInput.AxisInput, RectTransform> OnMove;

		private Graphic wheel;

		private RectTransform wheelTR;
		private Vector2 centerPoint;

		public float maximumSteeringAngle = 200f;
		public float wheelReleasedSpeed = 350f;
		public float valueMultiplier = 1f;

		private float wheelAngle = 0f;
		private float wheelPrevAngle = 0f;

		private bool wheelBeingHeld = false;

		private float m_value;
		public float Value { get { return m_value; } }

		public float Angle { get { return wheelAngle; } }

		public void Init()
		{
			wheel = GetComponent<Graphic>();
			wheelTR = wheel.rectTransform;

			SimpleInputDragListener eventReceiver = gameObject.AddComponent<SimpleInputDragListener>();
			eventReceiver.Listener = this;
			axis.StartTracking();
			SimpleInput.OnUpdate += OnUpdate;
		}


		private void OnDisable()
		{
			wheelBeingHeld = false;
			wheelAngle = wheelPrevAngle = m_value = 0f;
			wheelTR.localEulerAngles = Vector3.zero;

			axis.StopTracking();
			SimpleInput.OnUpdate -= OnUpdate;

			OnPress = null;
			OnRelease = null;
			OnMove = null;
		}

		private void OnUpdate()
		{
			// If the wheel is released, reset the rotation
			// to initial (zero) rotation by wheelReleasedSpeed degrees per second
			if( !wheelBeingHeld && wheelAngle != 0f )
			{
				float deltaAngle = wheelReleasedSpeed * Time.deltaTime;
				if( Mathf.Abs( deltaAngle ) > Mathf.Abs( wheelAngle ) )
					wheelAngle = 0f;
				else if( wheelAngle > 0f )
					wheelAngle -= deltaAngle;
				else
					wheelAngle += deltaAngle;
			}

			// Rotate the wheel image
			wheelTR.localEulerAngles = new Vector3( 0f, 0f, -wheelAngle );
			m_value = wheelAngle * valueMultiplier / maximumSteeringAngle;
			axis.value = m_value;
			OnMove?.Invoke(axis,wheelTR);

		}

		public void OnPointerDown( PointerEventData eventData )
		{
			// Executed when mouse/finger starts touching the steering wheel
			wheelBeingHeld = true;
			centerPoint = RectTransformUtility.WorldToScreenPoint( eventData.pressEventCamera, wheelTR.position );
			wheelPrevAngle = Vector2.Angle( Vector2.up, eventData.position - centerPoint );
			OnPress?.Invoke();
		}

		public void OnDrag( PointerEventData eventData )
		{
			Vector2 pointerPos = eventData.position;

			float wheelNewAngle = Vector2.Angle( Vector2.up, pointerPos - centerPoint );

			// Do nothing if the pointer is too close to the center of the wheel
			if( ( pointerPos - centerPoint ).sqrMagnitude >= 400f )
			{
				if( pointerPos.x > centerPoint.x )
					wheelAngle += wheelNewAngle - wheelPrevAngle;
				else
					wheelAngle -= wheelNewAngle - wheelPrevAngle;
			}

			// Make sure wheel angle never exceeds maximumSteeringAngle
			wheelAngle = Mathf.Clamp( wheelAngle, -maximumSteeringAngle, maximumSteeringAngle );
			wheelPrevAngle = wheelNewAngle;

		}

		public void OnPointerUp( PointerEventData eventData )
		{
			// Executed when mouse/finger stops touching the steering wheel
			// Performs one last OnDrag calculation, just in case
			OnDrag( eventData );
			OnRelease?.Invoke();

			wheelBeingHeld = false;
		}
	}
}