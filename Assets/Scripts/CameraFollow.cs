using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;       // Цель, за которой будет следовать камера (машина)
    public Vector3 offset;         // Смещение камеры относительно цели
    public float followSpeed = 10f; // Скорость следования камеры
    public float lookSpeed = 5f;    // Скорость поворота камеры

    void FixedUpdate()
    {
        // Рассчитываем желаемую позицию камеры с учетом смещения
        Vector3 desiredPosition = target.position + target.TransformVector(offset);

        // Плавное следование камеры за машиной
        transform.position = Vector3.Lerp(transform.position, desiredPosition, followSpeed * Time.deltaTime);
    }

    void LateUpdate()
    {
        // Рассчитываем направление, в котором должна смотреть камера
        Vector3 targetForward = target.forward;
        Vector3 directionToTarget = target.position - transform.position;

        // Поворот камеры так, чтобы она смотрела в направлении движения машины
        Quaternion desiredRotation = Quaternion.LookRotation(targetForward, Vector3.up);

        // Плавное поворачивание камеры
        transform.rotation = Quaternion.Slerp(transform.rotation, desiredRotation, lookSpeed * Time.deltaTime);
    }
}
