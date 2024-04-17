using UnityEngine;

public class EffectManager : MonoBehaviour
{
    public static EffectManager instance;

    [SerializeField]
    private LevelMaster levelMaster;
    [SerializeField]
    private Transform effectTransfrom;
    [SerializeField]
    private GameObject findEffect;

    private void Awake()
    {
        if (instance == null) instance = this;
    }

    public void CreateFindEffect(Transform transform)
    {
        var effectTrueLocation = Instantiate(findEffect, levelMaster.CurrentLevel.TrueLocation);
        var effectFakeLocation = Instantiate(findEffect, levelMaster.CurrentLevel.FakeLocation);
        effectTrueLocation.transform.localPosition = transform.localPosition;
        effectFakeLocation.transform.localPosition = transform.localPosition;
    }
}
