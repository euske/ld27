using UnityEngine;
using System.Collections;

public class BlockBehavior : MonoBehaviour 
{
    private float rotz;

    void Start()
    {
        rotz = Random.Range(0f, 90f);
    }

    void Update()
    {
        Vector3 rot = new Vector3(0f, 0f, rotz * Time.deltaTime);
        transform.Rotate(rot);
    }

}
