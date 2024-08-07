using System.Security.Cryptography.X509Certificates;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Transform spawnPoint; // Точка спавна машины
    public CameraFollow cameraFollow;
    public RaceTrafficLight trafficLight;
    public EventTrigger accBtn;
    public Button boostBtn;

    [Header("Car Controller Init")]
    [SerializeField] private TextMeshProUGUI speedText;     // Ссылка на TextMeshPro текст для отображения скорости
    [SerializeField] private TextMeshProUGUI lapTimeText;   // Ссылка на TextMeshPro текст для отображения времени круга
    [SerializeField] private RectTransform speedIndicator;  // Ссылка на RectTransform для отображения текущей скорости
    [SerializeField] private RaceWinLose raceWinLose; // Ссылка на RaceWinLose

    [Header("Path Follower init")]
    [SerializeField] private WaypointGenerator waypointGenerator;

    private CarController carController;
    private PathFollower pathFollower;
    private ControlLoss controlLoss;

    private void Start()
    {
        string selectedCarName = GameData.SelectedCarName;
        GameObject carPrefab = Resources.Load<GameObject>($"Cars/{selectedCarName}");
        if (carPrefab != null)
        {
            GameObject carInstance = Instantiate(carPrefab, spawnPoint.position, spawnPoint.rotation);
            carController = carInstance.GetComponent<CarController>();
            pathFollower = carInstance.GetComponent<PathFollower>();
            controlLoss = carInstance.GetComponent<ControlLoss>();
            cameraFollow.target = carInstance.transform;
            if (carController != null)
            {
                carController.Initialize(speedText, lapTimeText, speedIndicator, raceWinLose); // Передаем ссылки на необходимые объекты
                pathFollower.Initialize(waypointGenerator);
                controlLoss.Initialize(raceWinLose, cameraFollow);
                trafficLight.carController = carController;

                EventTrigger.Entry pointerDownEntry = new EventTrigger.Entry
                {
                    eventID = EventTriggerType.PointerDown
                };
                pointerDownEntry.callback.AddListener((eventData) => { accBtnDown((PointerEventData)eventData, carController); });

                // Добавляем событие в EventTrigger
                accBtn.triggers.Add(pointerDownEntry);

                // Создаем новое событие для PointerUp
                EventTrigger.Entry pointerUpEntry = new EventTrigger.Entry
                {
                    eventID = EventTriggerType.PointerUp
                };
                pointerUpEntry.callback.AddListener((eventData) => { accBtnUp((PointerEventData)eventData, carController); });

                // Добавляем событие в EventTrigger
                accBtn.triggers.Add(pointerUpEntry);

                boostBtn.onClick.AddListener(Boost);
            }
        }
        else
        {
            Debug.LogError($"Car prefab with name {selectedCarName} not found in Resources/Cars/");
        }
    }

    private void accBtnDown(PointerEventData eventData, CarController carController)
    {
        carController.Run();
    }

    private void accBtnUp(PointerEventData eventData, CarController carController)
    {
        carController.Stop();
    }

    private void Boost()
    {
        carController.StartBoost();
    }

    public void Leave()
    {
        SceneManager.LoadScene("Menu");
    }
}
