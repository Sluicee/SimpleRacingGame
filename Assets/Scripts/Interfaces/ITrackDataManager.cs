public interface ITrackDataManager
{
    void SaveTrackData(int selectedTrackIndex);
    void LoadTrackData(out int selectedTrackIndex);
}