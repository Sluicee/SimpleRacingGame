public class CurrencyManagerFactory
{
    public static ICurrencyManager CreateCurrencyManager()
    {
#if YANDEX_SDK
            return new YGCurrencyManager();
#elif PLAYER_PREFS
            return new PlayerPrefsCurrencyManager();
#else
        return null; // или бросьте исключение, если ни один SDK не задан
#endif
    }
}
