using UnityEngine;
using Zenject;

public class LevelMasterInstaller : MonoInstaller
{
    [SerializeField]
    private LevelMaster levelMaster;

    public override void InstallBindings()
    {
        Container.Bind<LevelMaster>().FromInstance(levelMaster).AsSingle().NonLazy();
    }
}
