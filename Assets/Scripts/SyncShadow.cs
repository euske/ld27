using UnityEngine;
using System.Collections;

public class SyncShadow : MonoBehaviour {

    public Transform target;

    void Awake()
    {
        GameObject light = GameObject.FindWithTag("light");
        if (light) {
            transform.rotation = light.GetComponent<Transform>().rotation;
        }
    }
	
    void Update()
    {
	transform.position = target.position;
    }
}
