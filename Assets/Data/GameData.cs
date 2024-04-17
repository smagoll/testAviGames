using System.Collections.Generic;

[System.Serializable]
public class GameData
{
    public int current_level;
    public float remaining_time;
    public List<string> findedDifference = new();
    
    public GameData(int level, float time)
    {
        current_level = level;
        remaining_time = time;
    }

}