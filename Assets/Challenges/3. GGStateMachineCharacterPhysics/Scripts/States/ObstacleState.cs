using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using GGPlugins.GGStateMachine.Scripts.Abstract;
using UnityEngine;

public class ObstacleState : GGStateBase
{
    private readonly Transform _characterTransform;

    public ObstacleState(Transform characterTransform)
    {
        _characterTransform = characterTransform;
    }

    // Start is called before the first frame update
    public override void Setup()
    {

    }

    public override async UniTask Entry(CancellationToken cancellationToken)
    {
        Debug.Log("ExampleState: Entry");
        await UniTask.Delay(TimeSpan.FromSeconds(2f), cancellationToken: cancellationToken);
    }

    public override async UniTask Exit(CancellationToken cancellationToken)
    {
        Debug.Log("ExampleState: Exit");
    }

    public override void CleanUp()
    {
    }
}
