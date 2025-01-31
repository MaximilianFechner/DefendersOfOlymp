using UnityEngine;

public interface IDataPersistence
{
    void LoadGame(GameData data);

    void SaveData(ref GameData data);   
}
