public class CarDataFactory
{
    public static ICarDataManager CreateCarDataManager()
    {
#if YANDEX_SDK
            return new YGCarDataManager();
#elif PLAYER_PREFS
            return new PlayerPrefsCarDataManager();
#else
        return null; // или бросьте исключение, если ни один SDK не задан
#endif
    }
}
