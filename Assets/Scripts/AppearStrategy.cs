using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AppearStrategy : ScriptableObject
{
    public abstract void Initialize(List<GameObject> platforms);
    public abstract IEnumerator Apply(List<GameObject> platforms);
}