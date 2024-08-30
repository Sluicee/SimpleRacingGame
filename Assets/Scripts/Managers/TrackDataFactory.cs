public class TrackDataFactory
{
    public static ITrackDataManager CreateTrackDataManager()
    {
#if YANDEX_SDK
            return new YGTrackDataManager();
#elif PLAYER_PREFS
            return new PlayerPrefsTrackDataManager();
#else
        return null; // или бросьте исключение, если ни один SDK не задан
#endif
    }
}
