#if YANDEX_SDK
using YG;
using UnityEngine;
using System.Collections.Generic;
public class YGPlayerDataManager : IPlayerData
{
    string MAIN_LEADERBOARD_NAME = "LEADERBOARD";
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

    public void RaceFinished(int award)
    {
        YandexGame.savesData.racedTimes += 1;
        YandexGame.SaveProgress();
        YandexGame.NewLeaderboardScores(MAIN_LEADERBOARD_NAME, award);
        MetricaSender.TriggerSend("Finishes: " + YandexGame.savesData.racedTimes);
    }
}
#endif
