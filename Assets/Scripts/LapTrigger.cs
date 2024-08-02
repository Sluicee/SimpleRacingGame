using UnityEngine;

public class LapTrigger : MonoBehaviour
{
    [SerializeField] private CarController carController; // Ссылка на CarController

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Убедитесь, что тег соответствует вашему игроку
        {
            if (!carController.LapStarted) // Если круг ещё не начат
            {
                carController.StartLap();
            }
            else if (carController.LapStarted && !carController.LapEnded) // Если круг начат, но ещё не окончен
            {
                carController.EndLap();
            }
        }
    }
}
