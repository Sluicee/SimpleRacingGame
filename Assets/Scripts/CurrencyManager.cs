using UnityEngine;

public class CurrencyManager : MonoBehaviour
{
    private static CurrencyManager _instance;
    public static CurrencyManager Instance => _instance;

    private int currency;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
            DontDestroyOnLoad(this.gameObject);
        }

        LoadCurrency();
    }

    public int GetCurrency()
    {
        return currency;
    }

    public void AddCurrency(int amount)
    {
        currency += amount;
        SaveCurrency();
    }

    public void SpendCurrency(int amount)
    {
        currency -= amount;
        SaveCurrency();
    }

    private void SaveCurrency()
    {
        PlayerPrefs.SetInt("Currency", currency);
    }

    private void LoadCurrency()
    {
        currency = PlayerPrefs.GetInt("Currency", 0);
    }
}
