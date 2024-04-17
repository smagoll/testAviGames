using UnityEngine.Events;

public static class GlobalEventManager
{
    public static readonly UnityEvent StartGame = new();
    public static readonly UnityEvent LoseGame = new();
    public static readonly UnityEvent EndGame = new();
    
    public static readonly UnityEvent RestartGame = new();
    public static readonly UnityEvent NextLevel = new();

    public static readonly UnityEvent DifferenceFound = new();
    public static readonly UnityEvent<Level> UpdateBar = new();
}
