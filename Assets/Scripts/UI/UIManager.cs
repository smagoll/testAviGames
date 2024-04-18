using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;
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
    [SerializeField]
    private LevelLoader levelLoader;
    
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

    private void OnDisable()
    {
        GlobalEventManager.UpdateBar.RemoveListener(UpdateBar);
        GlobalEventManager.EndGame.RemoveListener(() => endGameWindow.SetActive(true));
        GlobalEventManager.LoseGame.RemoveListener(() => loseGameWindow.SetActive(true));
    }

    public void UpdateBar(Level level)
    {
        SetTitle("Level " + DataManager.instance.gameData.current_level);
        UpdateCountDifference(levelMaster.RemainingDifferences);
    }
    
    private void SetTitle(string title) => textTitleLevel.text = title;

    public void UpdateCountDifference(int count)
    {
        textCountDifference.text = $"Осталось: {count}";
        
        DOTween.Sequence()
            .Append(textCountDifference.transform.DOScale(1.2f, .1f))
            .Append(textCountDifference.transform.DOScale(1f, .1f).SetEase(Ease.OutBounce));
    }

    public void UpdateTime(float time) => textTime.text = TimeSpan.FromSeconds(time).ToString(@"mm\:ss");

    public void LaunchLevelLoader(AsyncOperationHandle<GameObject> handle)
    {
        levelLoader.gameObject.SetActive(true);
        levelLoader.LoadLevel(handle);
    }

    #region Buttons

    private void SoundButtonClick() => AudioManager.instance.PlaySFX(AudioManager.instance.buttonClick);
    
    public void ButtonStartGame()
    {
        GlobalEventManager.StartGame.Invoke();
        game.SetActive(true);
        SoundButtonClick();
    }

    public void ButtonRestartGame()
    {
        loseGameWindow.SetActive(false);
        GlobalEventManager.RestartGame.Invoke();
        SoundButtonClick();
    }
    
    public void ButtonNextGame()
    {
        endGameWindow.SetActive(false);
        GlobalEventManager.NextLevel.Invoke();
        GlobalEventManager.RestartGame.Invoke();
        SoundButtonClick();
    }

    #endregion
}
