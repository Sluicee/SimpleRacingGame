public class PlayerDataFactory
{
    public static IPlayerData CreatePlayerDataManager()
    {
#if YANDEX_SDK
            return new YGPlayerDataManager();
#elif PLAYER_PREFS
            return new PlayerPrefsPlayerDataManager();
#else
        return null; // или бросьте исключение, если ни один SDK не задан
#endif
    }
}
