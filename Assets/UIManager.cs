using System;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI textTitleLevel;
    [SerializeField]
    private TextMeshProUGUI textCountDifference;
    [SerializeField]
    private TextMeshProUGUI textTime;

    [SerializeField]
    private GameObject game;
    [SerializeField]
    private GameObject endGameWindow; 
    [SerializeField]
    private GameObject loseGameWindow;
    
    private LevelMaster levelMaster;

    [Inject]
    private void Construct(LevelMaster levelMaster)
    {
        this.levelMaster = levelMaster;
    }
    
    private void OnEnable()
    {
        GlobalEventManager.UpdateBar.AddListener(UpdateBar);
        GlobalEventManager.EndGame.AddListener(() => endGameWindow.SetActive(true));
        GlobalEventManager.LoseGame.AddListener(() => loseGameWindow.SetActive(true));
    }

    public void UpdateBar(Level level)
    {
        SetTitle("Level" + level.number);
        UpdateCountDifference(level.countDifferences);
        UpdateTime(120);
    }
    
    private void SetTitle(string title)
    {
        textTitleLevel.text = title;
    }

    public void UpdateCountDifference(int count)
    {
        textCountDifference.text = count.ToString();
    }

    public void UpdateTime(float time)
    {
        textTime.text = TimeSpan.FromSeconds(time).ToString(@"mm\:ss");
    }

    public void ButtonStartGame()
    {
        GlobalEventManager.StartGame.Invoke();
        game.SetActive(true);
    }

    public void ButtonRestartGame()
    {
        loseGameWindow.SetActive(false);
        GlobalEventManager.RestartGame.Invoke();
    }
}
