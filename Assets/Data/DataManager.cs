using System.IO;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    [SerializeField]
    private float timeLevel;
    public static DataManager instance;

    public GameData gameData;

    private string filePath;

    private void OnEnable()
    {
        GlobalEventManager.NextLevel.AddListener(NextLevel);
        GlobalEventManager.RestartGame.AddListener(RestartLevel);
    }

    private void Awake()
    {
        if (instance != null && instance != this)
            Destroy(this);
        else
            instance = this;
        
        filePath = Application.persistentDataPath + "GameData.json";
        Load();
    }

    private void CreateNew()
    {
        gameData = new(1, timeLevel);
        Save();
    }

    public void Save()
    {
        string gameDataJson = JsonUtility.ToJson(gameData);
        File.WriteAllText(filePath, gameDataJson);
    }

    public void Load()
    {
        if (File.Exists(filePath))
        {
            string gameDataJson = File.ReadAllText(filePath);
            gameData = JsonUtility.FromJson<GameData>(gameDataJson);
        }
        else
        {
            CreateNew();
        }
    }

    private void RestartLevel()
    {
        gameData.remaining_time = timeLevel;
        gameData.findedDifference.Clear();
        Save();
    }
    
    private void NextLevel()
    {
        gameData.current_level++;
        RestartLevel();
    }

    public void UpdateRemainingTime(float time)
    {
        gameData.remaining_time = time;
        Save();
    }

    public void AddFindedDifference(string title)
    {
        gameData.findedDifference.Add(title);
    }
}