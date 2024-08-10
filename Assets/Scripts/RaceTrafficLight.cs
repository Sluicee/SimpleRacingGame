using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RaceTrafficLight : MonoBehaviour
{
    public GameObject[][] lights; // Массив для хранения ссылок на кружки светофора
    public float minWaitTime = 3f; // Минимальное время ожидания
    public float maxWaitTime = 6f; // Максимальное время ожидания
    public float percentForWhiteLight = 0.2f; // Процент времени, отведенный на белый свет
    private float waitTime; // Время задержки перед началом гонки
    public bool LightsOff = false;

    [SerializeField] private Color initialColor = new Color32(138, 16, 16, 255); // Цвет #8A1010
    [SerializeField] private Color activeColor = new Color32(212, 50, 50, 255); // Цвет #D43232
    [SerializeField] private Color startColor = new Color32(237, 237, 237, 255); // Цвет #EDEDED

    [SerializeField] private Button boostBtn;

    public CarController carController;

    private Coroutine lightSequence;
    private bool isPaused = false;

    void Start()
    {
        // Рандомное время ожидания
        waitTime = Random.Range(minWaitTime, maxWaitTime);

        // Инициализация массива светов
        InitializeLights();

        // Запуск корутины для управления светофором
        lightSequence = StartCoroutine(TrafficLightSequence());
    }

    public void Initialize(CarController _carController)
    {
        carController = _carController;
        carController.enabled = false;
    }

    void InitializeLights()
    {
        lights = new GameObject[4][];
        for (int i = 0; i < 4; i++)
        {
            lights[i] = new GameObject[3];
            for (int j = 0; j < 3; j++)
            {
                lights[i][j] = transform.Find($"Column{i + 1}/Light{j + 1}").gameObject;
                lights[i][j].GetComponent<Image>().color = initialColor;
            }
        }
    }

    IEnumerator TrafficLightSequence()
    {
        // Время для каждого столбца
        float timeForEachColumn = (waitTime * (1 - percentForWhiteLight)) / 4f;

        // Зажигаем красные огни
        for (int i = 0; i < 4; i++)
        {
            if (isPaused)
                yield break;

            for (int j = 0; j < 3; j++)
            {
                lights[i][j].GetComponent<Image>().color = activeColor;
            }
            yield return new WaitForSeconds(timeForEachColumn); // Интервал между столбцами
        }

        // Задержка перед началом белого света
        yield return new WaitForSeconds(waitTime * percentForWhiteLight);

        // Меняем цвет на #EDEDED на полсекунды и включаем машину
        for (int i = 0; i < 4; i++)
        {
            if (isPaused)
                yield break;

            for (int j = 0; j < 3; j++)
            {
                lights[i][j].GetComponent<Image>().color = startColor;
            }
        }

        carController.enabled = true;
        carController.StartLap();
        boostBtn.interactable = true;
        LightsOff = true;

        yield return new WaitForSeconds(0.5f);

        // Деактивация светофора
        gameObject.SetActive(false);
    }

    public void PauseTrafficLight()
    {
        isPaused = true;
        if (lightSequence != null)
        {
            StopCoroutine(lightSequence);
        }
    }

    public void ResumeTrafficLight()
    {
        isPaused = false;
        lightSequence = StartCoroutine(TrafficLightSequence());
    }

    public bool IsLightsOff()
    {
        return LightsOff;
    }
}
