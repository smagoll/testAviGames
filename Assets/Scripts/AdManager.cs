using AppodealStack.Monetization.Api;
using AppodealStack.Monetization.Common;
using UnityEngine;

public class AdManager : MonoBehaviour
{
    private void Start()
    {
        int adTypes = AppodealAdType.Interstitial;
        string appKey = "8766856430919e5fb9fd240f7ea291a82e2628afa8bff59c";
        Appodeal.Initialize(appKey, adTypes);
        
        #if UNITY_EDITOR
        AppodealCallbacks.Sdk.OnInitialized += OnInitializationFinished;
        #endif
    }

    private void OnInitializationFinished(object sender, SdkInitializedEventArgs e)
    {
        Debug.Log("sdk initialize");
    }
}
