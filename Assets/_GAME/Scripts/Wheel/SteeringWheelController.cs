using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;

public class SteeringWheelController : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    // Steering wheel properties
    public float maxSteeringAngle = 180f;   // Maximum steering angle in degrees
    public float rotationSpeed = 200f;      // Speed of rotation
    public float defaultRotationSpeed = 100f; // Speed of rotation when releasing input
    public float defaultRotationThreshold = 1f; // Threshold for considering the wheel at default rotation

    private float currentSteeringAngle = 0f; // Current steering angle
    private bool isPointerDown = false; // Flag to track pointer down state
    public Action OnPress;
    public Action<SimpleInput.AxisInput, RectTransform> OnMove;

    public Action OnRelease;
    // Reference to the UI Image representing the steering wheel
    public RectTransform steeringWheelImage;

    // Update is called once per frame
    void Update()
    {
        if (!isPointerDown && Mathf.Abs(currentSteeringAngle) <= defaultRotationThreshold)
        {
            currentSteeringAngle = 0f;
        }

        // Apply rotation to the steering wheel image
        steeringWheelImage.localRotation = Quaternion.Euler(0f, 0f, -currentSteeringAngle);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isPointerDown = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isPointerDown = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        float steeringInput = eventData.delta.x / Screen.width;
        currentSteeringAngle = steeringInput * maxSteeringAngle;
    }

    public void Init()
    {
        throw new System.NotImplementedException();
    }
}