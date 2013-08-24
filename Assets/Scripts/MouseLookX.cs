using UnityEngine;
using System.Collections;

public class MouseLookX : MonoBehaviour {

    public float sensitivity = 10f;

    void Update ()
    {
	transform.Rotate(0, Input.GetAxis("Mouse X") * sensitivity, 0);
    }
}
