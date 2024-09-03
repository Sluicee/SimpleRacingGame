
using System.Collections.Generic;

namespace YG
{
    [System.Serializable]
    public class SavesYG
    {
        // "Технические сохранения" для работы плагина (Не удалять)
        public int idSave;
        public bool isFirstSession = true;
        public string language = "ru";
        public bool promptDone;

        public int currency = 0;
        public List<int> unlockedCars = new List<int> { 0 };
        public int SelectedCarIndex = 0;
        public int SelectedTrackIndex = 0;
        public float recordTime = 0f;
        public List<string> trackNames = new List<string>();
        public List<float> trackRecords = new List<float>();
        public int racedTimes = 0;


        // Вы можете выполнить какие то действия при загрузке сохранений
        public SavesYG()
        {
            // Допустим, задать значения по умолчанию для отдельных элементов массива
        }
    }
}
