using System;
using AppodealStack.Monetization.Api;
using AppodealStack.Monetization.Common;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Purchasing;
using UnityEngine.Purchasing.Extension;
using UnityEngine.ResourceManagement.AsyncOperations;
using Zenject;

public class LevelMaster : MonoBehaviour
{
    private UIManager uiManager;
    [SerializeField]
    private Transform levelTransform;
    [SerializeField]
    private AssetReferenceGameObject levelReference;
    
    private Timer timer;
    private Level currentLevel;
    
    public Level CurrentLevel => currentLevel;

    public int RemainingDifferences
    {
        get
        {
            var value = currentLevel.CountDifferences - DataManager.instance.gameData.findedDifference.Count;
            if (value == 0) GlobalEventManager.EndGame.Invoke();
            return value;
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
        GlobalEventManager.DifferenceFound.AddListener(() => uiManager.UpdateCountDifference(RemainingDifferences));
        GlobalEventManager.StartGame.AddListener(CreateLevel);
        GlobalEventManager.RestartGame.AddListener(RestartGame);
        
        AppodealCallbacks.Interstitial.OnClosed += OnInterstitialClosed;
    }

    private void OnDisable()
    {
        GlobalEventManager.DifferenceFound.RemoveListener(() => uiManager.UpdateCountDifference(RemainingDifferences));
        GlobalEventManager.StartGame.RemoveListener(CreateLevel);
        GlobalEventManager.RestartGame.RemoveListener(RestartGame);
        
        AppodealCallbacks.Interstitial.OnClosed -= OnInterstitialClosed;
    }

    private void SetLevel(Level level)
    {
        GlobalEventManager.UpdateBar.Invoke(level);
        timer.Launch(DataManager.instance.gameData.remaining_time);
    }

    private void CreateLevel()
    { 
        var handle = levelReference.InstantiateAsync(levelTransform);
        uiManager.LaunchLevelLoader(handle);
        handle.Completed += OnLevelInstantiated;
    }

    private void OnLevelInstantiated(AsyncOperationHandle<GameObject> handle) // Уровень загружен
    {
        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            if(currentLevel != null) levelReference.ReleaseInstance(currentLevel.gameObject);
            currentLevel = handle.Result.GetComponent<Level>();
            SetLevel(currentLevel);
            var a = levelReference.InstantiateAsync(levelTransform);
            AudioManager.instance.PlayMusic(AudioManager.instance.backgroundLevel);
        }
    }

    private void RestartGame()
    {
        if(Appodeal.IsLoaded(AppodealAdType.Interstitial)) { // Показ рекламы
            Appodeal.Show(AppodealShowStyle.Interstitial);
            AudioManager.instance.SwitchMusic(false);
        }
    }
    
    private void OnInterstitialClosed(object sender, EventArgs e) => CreateLevel(); // Реклама закрыта

    public void PurchaseComplete(Product product) // Совершена покупка
    {
        float additionalTime = (float)product.definition.payout.quantity;
        DataManager.instance.gameData.remaining_time += additionalTime;
        DataManager.instance.Save();
        
        timer.Launch(DataManager.instance.gameData.remaining_time);
    }
    
    public void PurchaseFailed(Product product, PurchaseFailureDescription purchaseFailureDescription) // Покупка провалилась
    {
        timer.Launch(DataManager.instance.gameData.remaining_time);
    }
}
