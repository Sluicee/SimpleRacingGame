#if YANDEX_SDK
using System.Collections.Generic;
using YG;

public class YGCarDataManager : ICarDataManager
{
    public void SaveCarData(int selectedCarIndex, List<CarData> carDataList)
    {
        YandexGame.savesData.unlockedCars = new List<int>();
        for (int i = 0; i < carDataList.Count; i++)
        {
            if (carDataList[i].isUnlocked)
            {
                YandexGame.savesData.unlockedCars.Add(i);
            }
        }
        YandexGame.savesData.SelectedCarIndex = selectedCarIndex;
        YandexGame.SaveProgress();
    }

    public void LoadCarData(List<CarData> carDataList, out int selectedCarIndex)
    {
        selectedCarIndex = YandexGame.savesData.SelectedCarIndex;
        foreach (var index in YandexGame.savesData.unlockedCars)
        {
            if (index >= 0 && index < carDataList.Count)
            {
                carDataList[index].isUnlocked = true;
            }
        }
    }
}

#endif