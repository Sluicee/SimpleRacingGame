#if PLAYER_PREFS
using UnityEngine;
public class PlayerPrefsPlayerDataManager : IPlayerData
{
    private const string RaceCounterKey = "RaceCounter";

    public void SaveRecord(float recordTime,string trackName)
    {
        PlayerPrefs.SetFloat(trackName + "_RecordTime", newRecord);
        PlayerPrefs.Save();
    }

    public float LoadRecord(string trackName)
    {
        return PlayerPrefs.GetFloat(trackName + "_RecordTime", Mathf.Infinity);
    }

    public void RaceFinished()
    {
        // Проверяем, существует ли значение с ключом "RaceCounter"
        if (PlayerPrefs.HasKey(RaceCounterKey))
        {
            // Если существует, увеличиваем значение на 1
            int currentCount = PlayerPrefs.GetInt(RaceCounterKey);
            currentCount++;
            PlayerPrefs.SetInt(RaceCounterKey, currentCount);
        }
        else
        {
            // Если не существует, создаем его и устанавливаем значение 1
            PlayerPrefs.SetInt(RaceCounterKey, 1);
        }

        // Сохраняем изменения в PlayerPrefs
        PlayerPrefs.Save();
    }
}
#endif
