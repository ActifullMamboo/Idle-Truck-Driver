using UnityEngine;

public class TrailerController : MonoBehaviour
{
    public Transform truck;
    public float followDistance = 10f;
    public float rotationSpeed = 5f;
    public float moveSpeed = 5f;

    private Vector3 targetPosition;

    void Start()
    {
        targetPosition = truck.position - truck.forward * followDistance;
    }

    void FixedUpdate()
    {
        // Рассчитываем позицию, куда должен двигаться прицеп
        targetPosition = truck.position - truck.forward * followDistance;

        // Перемещаем прицеп к целевой позиции
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * moveSpeed);

        // Вычисляем поворот прицепа в зависимости от поворота грузовика
        Quaternion targetRotation = Quaternion.LookRotation(truck.position - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
    }
}