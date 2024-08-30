#if YANDEX_SDK
using YG;

public class YGTrackDataManager : ITrackDataManager
{
    public void SaveTrackData(int selectedTrackIndex)
    {
        YandexGame.savesData.SelectedTrackIndex = selectedTrackIndex;
        YandexGame.SaveProgress();
    }

    public void LoadTrackData(out int selectedTrackIndex)
    {
        selectedTrackIndex = YandexGame.savesData.SelectedTrackIndex;
    }
}
#endif