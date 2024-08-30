#if PLAYER_PREFS
using UnityEngine;

public class PlayerPrefsCurrencyManager : ICurrencyManager
{
    public void SaveCurrency(int currency)
    {
        PlayerPrefs.SetInt("Currency", currency);
    }

    public int LoadCurrency()
    {
        return PlayerPrefs.GetInt("Currency", 0);
    }
}
#endif
