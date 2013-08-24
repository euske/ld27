using UnityEngine;
using System.Collections;

public class MouseLookX : MonoBehaviour {

    public float sensitivity = 5f;
    public float minimum = -60f;
    public float maximum = 60f;

    private float rotx = 0F;

    void Update ()
    {
	rotx += Input.GetAxis("Mouse X") * sensitivity;
	rotx = Mathf.Clamp(rotx, minimum, maximum);
	transform.localEulerAngles = new Vector3(0, rotx, 0);
    }
}
