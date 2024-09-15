using UnityEngine;

namespace _GAME.Scripts.Car
{
    public class WheelRotation : MonoBehaviour
    {

        public void SetRotation(RectTransform rect)
        {
            transform.localEulerAngles = new Vector3(0, -rect.eulerAngles.z, 0f);
        }
    }
}
