using UnityEngine;
using System.Collections;

public class SyncShadow : MonoBehaviour 
{
    public new Transform light;
    public Transform target;

    void Awake()
    {
        transform.rotation = light.rotation;
    }
	
    void Update()
    {
	transform.position = target.position;
    }
}
