using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

public class LevelMaster : MonoBehaviour
{
    private UIManager uiManager;
    [SerializeField]
    private Transform levelTransform;
    [SerializeField]
    private Level prefabLevel;

    [SerializeField]
    private int remainingDifferences;

    private Timer timer;
    
    private Level currentLevel;
    
    public Level CurrentLevel => currentLevel;
    
    public int RemainingDifferences
    {
        get => remainingDifferences;
        set
        {
            remainingDifferences = value;
            if(value == 0) GlobalEventManager.EndGame.Invoke();
            uiManager.UpdateCountDifference(value);
        }
    }

    [Inject]
    private void Construct(UIManager uiManager)
    {
        this.uiManager = uiManager;
    }
    
    private void Awake()
    {
        timer = GetComponent<Timer>();
    }

    private void OnEnable()
    {
        GlobalEventManager.DifferenceFound.AddListener(() => RemainingDifferences--);
        GlobalEventManager.StartGame.AddListener(CreateLevel);
        GlobalEventManager.RestartGame.AddListener(RestartGame);
    }

    private void SetLevel()
    {
        GlobalEventManager.UpdateBar.Invoke(currentLevel);
        uiManager.UpdateBar(currentLevel);
        remainingDifferences = prefabLevel.countDifferences;
        timer.Launch(10);
    }

    private void CreateLevel()
    {
        currentLevel = Instantiate(prefabLevel, levelTransform);
        SetLevel();
    }

    private void RestartGame()
    {
        Destroy(currentLevel.gameObject);
        CreateLevel();
    }
}
