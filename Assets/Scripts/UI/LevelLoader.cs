using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;

public class LevelLoader : MonoBehaviour
{
    [SerializeField]
    private Slider slider;
    private CanvasGroup canvasGroup;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    private void Start()
    {
        HideLevelLoader();
    }

    public void LoadLevel(AsyncOperationHandle<GameObject> handle)
    {
        ShowLevelLoader();
        StartCoroutine(Load(handle));
    }
    
    private IEnumerator Load(AsyncOperationHandle<GameObject> handlel)
    {
        while (handlel.PercentComplete < 1 && handlel.Status != AsyncOperationStatus.Succeeded)
        {
            slider.DOValue(handlel.PercentComplete / .8f, .3f).SetUpdate(true);
            Debug.Log(handlel.PercentComplete);
            yield return null;
        }
        HideLevelLoader();
    }
    
    public void ShowLevelLoader()
    {
        canvasGroup.DOFade(255f, 1f).SetUpdate(true);
        slider.gameObject.SetActive(true);
    }
    
    public void HideLevelLoader()
    {
        canvasGroup.DOFade(0f, 1f).SetUpdate(true);
        slider.gameObject.SetActive(false);
    }
}
