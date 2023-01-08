using System.Collections.Generic;
using UnityEngine;

public class PlatformGroup : MonoBehaviour
{
    [RuntimeInitializeOnLoadMethod]
    private static void Init()
    {
        LeanTween.reset();
    }
        
    [SerializeField] private AppearStrategy strategy;

    private List<GameObject> _children = new List<GameObject>();
    private bool _isVisible;

    private void Awake()
    {
        for (int i = 0; i < transform.childCount; i++)
            _children.Add(transform.GetChild(i).gameObject);
            
        strategy.Initialize(_children);
    }

    public void Show()
    {
        if (_isVisible)
            return;
            
        StartCoroutine(strategy.Apply(_children));
        _isVisible = true;
    }
}