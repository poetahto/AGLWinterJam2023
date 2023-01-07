using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DefaultNamespace
{
    public abstract class AppearStrategy : ScriptableObject
    {
        public abstract void Initialize(List<GameObject> platforms);
        public abstract IEnumerator Apply(List<GameObject> platforms);
    }
}