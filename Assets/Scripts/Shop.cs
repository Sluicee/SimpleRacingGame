using UnityEngine;

public class Shop : MonoBehaviour
{
    // Метод для добавления валюты
    public void AddCurrency(int amount)
    {
        CurrencyManager.Instance.AddCurrency(amount);
    }

    public void CurrencyForAds(int amount)
    {
        CurrencyManager.Instance.CurrencyForAds(amount);
    }
}
