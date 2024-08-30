#if PLAYER_PREFS
using UnityEngine;
public class PlayerPrefsPlayerDataManager : IPlayerData
{
    public void SaveRecord(float recordTime,string trackName)
    {
        PlayerPrefs.SetFloat(trackName + "_RecordTime", newRecord);
        PlayerPrefs.Save();
    }

    public float LoadRecord(string trackName)
    {
        return PlayerPrefs.GetFloat(trackName + "_RecordTime", Mathf.Infinity);
    }
}
#endif
