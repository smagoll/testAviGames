using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;
using UnityEngine.UI;

public class ClickButtonDefault : MonoBehaviour
{
    public UnityEvent endClick = new();
    
    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(Pressed);
    }

    private void Pressed()
    {
        DOTween.Sequence()
            .Append(transform.DOScale(.8f, .3f))
            .Append(transform.DOScale(1f, .1f))
            .AppendCallback(() => endClick.Invoke())
            .SetEase(Ease.OutBack)
            .SetUpdate(true);
    }
}