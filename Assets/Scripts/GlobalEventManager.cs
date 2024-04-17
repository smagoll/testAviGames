using UnityEngine.Events;

public static class GlobalEventManager
{
    public static UnityEvent StartGame = new();
    public static UnityEvent LoseGame = new();
    public static UnityEvent EndGame = new();
    
    public static UnityEvent RestartGame = new();
    public static UnityEvent NextLevel = new();

    public static UnityEvent DifferenceFound = new();
    public static UnityEvent<Level> UpdateBar = new();
}
