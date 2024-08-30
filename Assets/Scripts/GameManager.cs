using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
#if YANDEX_SDK
using YG;
#endif

public class GameManager : MonoBehaviour
{
    public Transform spawnPoint; // Точка спавна машины
    public CameraFollow cameraFollow; // Камера для слежения за машиной
    public RaceTrafficLight trafficLight; // Система стартовых огней
    public EventTrigger accBtn; // Кнопка акселератора
    public Button boostBtn; // Кнопка буста

    [Header("Car Controller Init")]
    [SerializeField] private TextMeshProUGUI speedText;     // Отображение скорости
    [SerializeField] private TextMeshProUGUI lapTimeText;   // Отображение времени круга
    [SerializeField] private RectTransform speedIndicator;  // Индикатор скорости
    [SerializeField] private RaceWinLose raceWinLose; // Система управления победой и поражением
    [SerializeField] private LapTrigger lapTrigger; // Триггер для засчёта кругов
    [SerializeField] private GameObject boostEffect; // Триггер для засчёта кругов

    [Header("Path Follower init")]
    [SerializeField] private WaypointGenerator waypointGenerator; // Генератор путевых точек

    private CarController carController; // Контроллер машины
    private PathFollower pathFollower; // Следование по пути
    private ControlLoss controlLoss; // Управление потерей контроля

    private void Start()
    {
        // Загружаем имя выбранной машины из статического класса GameData
        string selectedCarName = GameData.SelectedCarName;

        // Загружаем префаб машины из папки Resources/Cars
        GameObject carPrefab = Resources.Load<GameObject>($"Cars/{selectedCarName}");

        Time.timeScale = 1;

        if (carPrefab != null)
        {
            // Создаем экземпляр машины на точке спавна
            GameObject carInstance = Instantiate(carPrefab, spawnPoint.position, spawnPoint.rotation);

            // Получаем компоненты машины
            carController = carInstance.GetComponent<CarController>();
            pathFollower = carInstance.GetComponent<PathFollower>();
            controlLoss = carInstance.GetComponent<ControlLoss>();

            // Настраиваем камеру, чтобы она следила за машиной
            cameraFollow.target = carInstance.transform;

            if (carController != null)
            {
                // Инициализация контроллера машины с необходимыми ссылками на UI-элементы
                carController.Initialize(speedText, lapTimeText, speedIndicator, raceWinLose, cameraFollow, boostEffect);

                // Инициализация системы следования по пути
                pathFollower.Initialize(waypointGenerator);

                // Инициализация системы потери контроля
                controlLoss.Initialize(raceWinLose, cameraFollow);

                // Настраиваем объекты, связанные с машиной
                trafficLight.carController = carController;
                lapTrigger.carController = carController;
                raceWinLose.carController = carController;

                // Настройка событий для кнопки акселератора
                AddEventTrigger(accBtn, EventTriggerType.PointerDown, (eventData) => carController.Run());
                AddEventTrigger(accBtn, EventTriggerType.PointerUp, (eventData) => carController.Stop());

                // Настройка события для кнопки буста
                boostBtn.onClick.AddListener(carController.StartBoost);
            }
        }
        else
        {
            Debug.LogError($"Car prefab with name {selectedCarName} not found in Resources/Cars/");
        }
    }

    // Метод для добавления событий в EventTrigger
    private void AddEventTrigger(EventTrigger trigger, EventTriggerType eventType, UnityEngine.Events.UnityAction<BaseEventData> action)
    {
        EventTrigger.Entry entry = new EventTrigger.Entry { eventID = eventType };
        entry.callback.AddListener(action);
        trigger.triggers.Add(entry);
    }

    // Метод для выхода в главное меню
    public void Leave()
    {
#if YANDEX_SDK
        YandexGame.GameplayStop();
#endif
        SceneManager.LoadScene("Menu");
    }
}
