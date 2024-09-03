public interface IPlayerData
{
    void SaveRecord(float recordTime, string trackName);
    float LoadRecord(string trackName);

    void RaceFinished();

}
