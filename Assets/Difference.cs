using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Difference : MonoBehaviour, IPointerClickHandler
{
    private Image image;
    private GameObject findEffect;
    private bool isFind;

    private void Awake()
    {
        if (DataManager.instance.gameData.findedDifference.Contains(name))
        {
            gameObject.SetActive(false);
            isFind = true;
        }
        
        image = GetComponent<Image>();
    }

    private void Hide()
    {
        isFind = true;
        SpawnParticle();
        DataManager.instance.AddFindedDifference(name);
        GlobalEventManager.DifferenceFound.Invoke();
        
        DOTween.Sequence()
            .Append(transform.DOScale(transform.localScale * 1.2f, .3f))
            .AppendCallback(() =>
            {
                transform.DOScale(transform.localScale, .5f);
                image.DOFade(0f, .5f);
            })
            .AppendInterval(.5f)
            .AppendCallback(() =>
            {
                gameObject.SetActive(false);
            });
    }

    private void SpawnParticle() => EffectManager.instance.CreateFindEffect(transform);

    public void OnPointerClick(PointerEventData eventData)
    {
        if (isFind) return;
        Hide();
    }
}
