#if PLAYER_PREFS
using System.Collections.Generic;
using UnityEngine;
public class PlayerPrefsCarDataManager : ICarDataManager
{
    private const string UNLOCKED_CARS_KEY = "UnlockedCars";
    private const string SELECTED_CAR_KEY = "SelectedCarIndex";

    public void SaveCarData(int selectedCarIndex, List<CarData> carDataList)
    {
        for (int i = 0; i < carDataList.Count; i++)
        {
            PlayerPrefs.SetInt(UNLOCKED_CARS_KEY + i, carDataList[i].isUnlocked ? 1 : 0);
        }
        PlayerPrefs.SetInt(SELECTED_CAR_KEY, selectedCarIndex);
        PlayerPrefs.Save();
    }

    public void LoadCarData(List<CarData> carDataList, out int selectedCarIndex)
    {
        selectedCarIndex = PlayerPrefs.GetInt(SELECTED_CAR_KEY, 0);
        for (int i = 0; i < carDataList.Count; i++)
        {
            int isUnlockedValue = PlayerPrefs.GetInt(UNLOCKED_CARS_KEY + i, -1);
            if (isUnlockedValue != -1)
            {
                carDataList[i].isUnlocked = isUnlockedValue == 1;
            }
        }
    }
}
#endif

