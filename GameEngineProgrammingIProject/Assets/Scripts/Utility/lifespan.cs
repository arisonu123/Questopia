using UnityEngine;
using System.Collections;

[DisallowMultipleComponent]
public class lifespan : MonoBehaviour {
    [SerializeField]
    private float duration = Mathf.Infinity;

    private void Awake()
    {
        Destroy(gameObject, duration);
        
    }


 
}
