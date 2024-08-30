using System.Collections.Generic;
public interface ICarDataManager
{
    void SaveCarData(int selectedCarIndex, List<CarData> carDataList);
    void LoadCarData(List<CarData> carDataList, out int selectedCarIndex);
}