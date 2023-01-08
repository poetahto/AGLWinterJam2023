using poetools.New_folder;
using UnityEngine;

public class VoidFallReset : MonoBehaviour
{
    [SerializeField] private float resetDistance = -100f;
    
    private InputController _inputController;

    private void Awake()
    {
        _inputController = FindObjectOfType<InputController>();
    }

    private void Update()
    {
        var target = _inputController.Target.transform;
        
        if (target.position.y < resetDistance)
            target.position = transform.position;
    }
}
