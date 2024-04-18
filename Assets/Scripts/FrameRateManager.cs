using System.Collections;
using System.Threading;
using UnityEngine;
public class FrameRateManager : MonoBehaviour
{
    [Header("Frame Settings")]
    int MaxRate = 9999;
    public float TargetFrameRate = 60.0f;
    float currentFrameTime;
    void Start()
    {
        StartCoroutine(StartFrameUnlock());
    }

    private IEnumerator StartFrameUnlock()
    {
        yield return new WaitForSeconds(1f);
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 61;
    }
    
    private IEnumerator WaitForNextFrame()
    {
        while (true)
        {
            yield return new WaitForEndOfFrame();
            currentFrameTime += 1.0f / TargetFrameRate;
            var t = Time.realtimeSinceStartup;
            var sleepTime = currentFrameTime - t - 0.01f;
            if (sleepTime > 0)
                Thread.Sleep((int)(sleepTime * 1000));
            while (t < currentFrameTime)
                t = Time.realtimeSinceStartup;
        }
    }
}
