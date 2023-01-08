using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DefaultNamespace
{
    [CreateAssetMenu]
    public class RiseUpAppear : AppearStrategy
    {
        [SerializeField] private float scaleTime;
        [SerializeField] private float randomAmount;
        [SerializeField] private float hideDistance;
        [SerializeField] private LeanTweenType easing;
        
        public override void Initialize(List<GameObject> platforms)
        {
            foreach (var gameObject in platforms)
            {
                gameObject.SetActive(false);
                var pos = gameObject.transform.position;
                pos.y -= hideDistance;
                gameObject.transform.position = pos;
            }
        }

        public override IEnumerator Apply(List<GameObject> platforms)
        {
            foreach (var gameObject in platforms)
            {
                gameObject.SetActive(true);
                var pos = gameObject.transform.position;
                pos.y += hideDistance;
                gameObject.transform.LeanMoveY(pos.y, scaleTime + Random.Range(-randomAmount, randomAmount)).setEase(easing);
            }

            yield break;
        }
    }
}