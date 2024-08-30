using UnityEngine;
#if YANDEX_SDK
using YG;
#endif

public class CurrencyManager : MonoBehaviour
{
    private static CurrencyManager _instance;
    public static CurrencyManager Instance => _instance;

    private int currency;

    private ICurrencyManager currencyManager;

    private void Awake()
    {
        currencyManager = CurrencyManagerFactory.CreateCurrencyManager();
#if YANDEX_SDK
        // Проверяем запустился ли плагин
        if (YandexGame.SDKEnabled == true)
        {
            // Если запустился, то выполняем Ваш метод для загрузки
            GetLoad();

            // Если плагин еще не прогрузился, то метод не выполнится в методе Start,
            // но он запустится при вызове события GetDataEvent, после прогрузки плагина
        }
#endif
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
            DontDestroyOnLoad(this.gameObject);
        }

        GetLoad();
    }

    public int GetCurrency()
    {
        return currency;
    }

    public void AddCurrency(int amount = 5000)
    {
        currency += amount;
        currencyManager.SaveCurrency(currency);
    }

    public void SpendCurrency(int amount)
    {
        currency -= amount;
        currencyManager.SaveCurrency(currency);
    }

    public void GetLoad()
    {
        currency = currencyManager.LoadCurrency();
    }

    public void CurrencyForAds(int amount)
    {
#if YANDEX_SDK
        YandexGame.RewVideoShow(0);
#elif PLAYER_PREFS
        currency += amount;
        currencyManager.SaveCurrency(currency);
#endif
    }

#if YANDEX_SDK
    // Подписанный метод получения награды
    void Rewarded(int id)
    {
		AddCurrency();
    }
    // Подписываемся на событие открытия рекламы в OnEnable
    private void OnEnable()
    {
        YandexGame.RewardVideoEvent += Rewarded;
        YandexGame.GetDataEvent += GetLoad;
    }

    // Отписываемся от события открытия рекламы в OnDisable
    private void OnDisable()
    {
        YandexGame.RewardVideoEvent -= Rewarded;
        YandexGame.GetDataEvent -= GetLoad;
    }
#endif
}
