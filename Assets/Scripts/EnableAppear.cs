using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class EnableAppear : AppearStrategy
{
    [SerializeField] private float appearDelay;

    public override IEnumerator Apply(List<GameObject> platforms)
    {
        foreach (var gameObject in platforms)
            gameObject.SetActive(true);

        yield break;
    }

    public override void Initialize(List<GameObject> platforms)
    {
        foreach (var gameObject in platforms)
        {
            gameObject.SetActive(false);
        }
    }
}