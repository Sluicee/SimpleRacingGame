#if PLAYER_PREFS
using UnityEngine;

public class PlayerPrefsTrackDataManager : ITrackDataManager
{
    private const string SELECTED_TRACK_KEY = "SelectedTrackIndex";

    public void SaveTrackData(int selectedTrackIndex)
    {
        PlayerPrefs.SetInt(SELECTED_TRACK_KEY, selectedTrackIndex);
        PlayerPrefs.Save();
    }

    public void LoadTrackData(out int selectedTrackIndex)
    {
        selectedTrackIndex = PlayerPrefs.GetInt(SELECTED_TRACK_KEY, 0);
    }
}
#endif
