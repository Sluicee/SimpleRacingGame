#if YANDEX_SDK
using YG;
public class YGCurrencyManager : ICurrencyManager
{
    public void SaveCurrency(int currency)
    {
        YandexGame.savesData.currency = currency;
        YandexGame.SaveProgress();
    }

    public int LoadCurrency()
    {
        return YandexGame.savesData.currency;
    }
}
#endif
