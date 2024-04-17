using UnityEngine;

public class Level : MonoBehaviour
{
    public int number;
    public int countDifferences;

    [SerializeField]
    private GameObject trueLocation;
    [SerializeField]
    private GameObject fakeLocation;

    public Transform TrueLocation => trueLocation.transform;
    public Transform FakeLocation => fakeLocation.transform;
}
