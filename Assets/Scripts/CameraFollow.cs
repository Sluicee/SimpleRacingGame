using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform target;       // Цель, за которой будет следовать камера (машина)
    [SerializeField] private Vector3 offset;         // Смещение камеры относительно цели
    [SerializeField] private float followSpeed = 10f; // Скорость следования камеры
    [SerializeField] private float lookSpeed = 5f;    // Скорость поворота камеры

    private bool isInCollisionMode = false; // Флаг для проверки режима столкновения

    private void FixedUpdate()
    {
        if (!isInCollisionMode)
        {
            // Рассчитываем желаемую позицию камеры с учетом смещения
            Vector3 desiredPosition = target.position + target.TransformVector(offset);

            // Плавное следование камеры за машиной
            transform.position = Vector3.Lerp(transform.position, desiredPosition, followSpeed * Time.deltaTime);
        }
    }

    private void LateUpdate()
    {
        if (isInCollisionMode)
        {
            // Поворот камеры, чтобы она всегда смотрела на цель
            Vector3 directionToTarget = target.position - transform.position;
            Quaternion desiredRotation = Quaternion.LookRotation(directionToTarget, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, desiredRotation, lookSpeed * Time.deltaTime);
        }
        else
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

    public void SetCollisionMode(bool isCollisionMode)
    {
        isInCollisionMode = isCollisionMode;
    }
}
