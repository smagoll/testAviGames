using UnityEngine;

public class Level : MonoBehaviour
{
    private int countDifferences;
    public int CountDifferences => countDifferences;

    private void Awake()
    {
        countDifferences = FakeLocation.GetComponentsInChildren<Difference>().Length;
    }

    [SerializeField]
    private GameObject trueLocation;
    [SerializeField]
    private GameObject fakeLocation;

    public Transform TrueLocation => trueLocation.transform;
    public Transform FakeLocation => fakeLocation.transform;
}
