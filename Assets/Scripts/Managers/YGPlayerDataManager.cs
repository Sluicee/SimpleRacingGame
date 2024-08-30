#if YANDEX_SDK
using YG;
using UnityEngine;
using System.Collections.Generic;
public class YGPlayerDataManager : IPlayerData
{
    public void SaveRecord(float recordTime, string trackName)
    {
        int trackIndex = YandexGame.savesData.trackNames.IndexOf(trackName);
        string leaderboardName = trackName.Replace(" ", "");
        if (trackIndex >= 0){
            YandexGame.savesData.trackRecords[trackIndex] = recordTime;
            YandexGame.NewLBScoreTimeConvert(leaderboardName, recordTime);
        }
        else {
            YandexGame.savesData.trackNames.Add(trackName);
            YandexGame.savesData.trackRecords.Add(recordTime);
            YandexGame.NewLBScoreTimeConvert(leaderboardName, recordTime);
        }
        YandexGame.SaveProgress();
    }

    public float LoadRecord(string trackName)
    {
        int trackIndex = YandexGame.savesData.trackNames.IndexOf(trackName);

        if (trackIndex >= 0)
        {
            // Возвращаем рекордное время для конкретной трассы
            return YandexGame.savesData.trackRecords[trackIndex];
        }
        else
        {
            // Возвращаем бесконечность, если рекордов для этой трассы еще нет
            return Mathf.Infinity;
        }
    }
}
#endif
