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

    private GameObject level;
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

    private void SetLevel()
    {
        GlobalEventManager.UpdateBar.Invoke(currentLevel);
        timer.Launch(DataManager.instance.gameData.remaining_time);
    }

    private void CreateLevel() => levelReference.InstantiateAsync(levelTransform).Completed += OnLevelInstantiated;

    private void OnLevelInstantiated(AsyncOperationHandle<GameObject> handle)
    {
        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            if(currentLevel != null) levelReference.ReleaseInstance(currentLevel.gameObject);
            currentLevel = handle.Result.GetComponent<Level>();
            SetLevel();
        };
    }

    private void RestartGame()
    {
        if(Appodeal.IsLoaded(AppodealAdType.Interstitial)) {
            Appodeal.Show(AppodealShowStyle.Interstitial);
        }
    }
    
    private void OnInterstitialClosed(object sender, EventArgs e) => CreateLevel();

    public void PurchaseComplete(Product product)
    {
        float additionalTime = (float)product.definition.payout.quantity;
        DataManager.instance.gameData.remaining_time += additionalTime;
        DataManager.instance.Save();
        
        timer.Launch(DataManager.instance.gameData.remaining_time);
    }
    
    public void PurchaseFailed(Product product, PurchaseFailureDescription purchaseFailureDescription)
    {
        timer.Launch(DataManager.instance.gameData.remaining_time);
    }
}
