using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ScaleAppear : AppearStrategy
{
    [SerializeField] private float scaleTime;
    [SerializeField] private float randomAmount;
    [SerializeField] private LeanTweenType easing;
        
    public override void Initialize(List<GameObject> platforms)
    {
        foreach (var gameObject in platforms)
        {
            gameObject.SetActive(false);
            gameObject.transform.localScale = Vector3.zero;
        }
    }

    public override IEnumerator Apply(List<GameObject> platforms)
    {
        foreach (var gameObject in platforms)
        {
            gameObject.SetActive(true);
            gameObject.transform.LeanScale(Vector3.one, scaleTime + Random.Range(-randomAmount, randomAmount)).setEase(easing);
        }

        yield break;
    }
}