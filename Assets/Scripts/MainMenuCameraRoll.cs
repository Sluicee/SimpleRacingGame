using UnityEngine;

public class MainMenuCameraRoll : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 10f; // Скорость вращения

    void Update()
    {
        // Вращение камеры по оси X
        transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
    }
}
