using UnityEngine;
using YG;

public class Shop : MonoBehaviour
{
    [SerializeField] private AudioSource UIAudio;
    [SerializeField] private AudioClip buySoundEffect;

    // Метод для добавления валюты
    public void AddCurrency(int amount)
    {
        CurrencyManager.Instance.AddCurrency(amount);
    }

    public void CurrencyForAds(int amount)
    {
        CurrencyManager.Instance.CurrencyForAds(amount);
    }
#if YANDEX_SDK
    // Подписываемся на ивенты успешной/неуспешной покупки
    private void OnEnable()
    {
        YandexGame.PurchaseSuccessEvent += SuccessPurchased;
        YandexGame.PurchaseFailedEvent += FailedPurchased; // Необязательно
    }

    private void OnDisable()
    {
        YandexGame.PurchaseSuccessEvent -= SuccessPurchased;
        YandexGame.PurchaseFailedEvent -= FailedPurchased; // Необязательно
    }

    // Покупка успешно совершена, выдаём товар
    void SuccessPurchased(string id)
    {
        // код для обработки покупки.
        if (id == "10000")
            CurrencyManager.Instance.AddCurrency(10000);
        else if (id == "25000")
            CurrencyManager.Instance.AddCurrency(25000);
        else if (id == "50000")
            CurrencyManager.Instance.AddCurrency(50000);
        else if (id == "100000")
            CurrencyManager.Instance.AddCurrency(100000);
        else if (id == "250000")
            CurrencyManager.Instance.AddCurrency(250000);
        UIAudio.PlayOneShot(buySoundEffect);
        MetricaSender.TriggerSend("SuccessPurchased");
    }

    // Покупка не была произведена
    void FailedPurchased(string id)
    {
        // Например, можно открыть уведомление о неуспешности покупки.
    }
#endif
}
