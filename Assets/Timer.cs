using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class Timer : MonoBehaviour
{
    private UIManager uiManager;
    private IEnumerator coroutineTimer;

    [Inject]
    private void Construct(UIManager uiManager)
    {
        this.uiManager = uiManager;
    }
    
    public void Launch(float time)
    {
        coroutineTimer = TimerCoroutine(time, 1f);
        StartCoroutine(coroutineTimer);
    }
    
    private IEnumerator TimerCoroutine(float time, float period) {
        for(float remainingTime = time; remainingTime >= 0; remainingTime--) {
            uiManager.UpdateTime(remainingTime);
            yield return new WaitForSeconds(period);
        }
        GlobalEventManager.LoseGame.Invoke();
    }

    private void OnEnable()
    {
        GlobalEventManager.EndGame.AddListener(() => StopCoroutine(coroutineTimer));
    }
}
