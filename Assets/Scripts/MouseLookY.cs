using UnityEngine;
using System.Collections;

public class MouseLookY : MonoBehaviour {

    public float sensitivity = 5f;
    public float minimum = -60f;
    public float maximum = 60f;

    private float roty = 0F;

    void Update ()
    {
	roty += Input.GetAxis("Mouse Y") * sensitivity;
	roty = Mathf.Clamp(roty, minimum, maximum);
	transform.localEulerAngles = new Vector3(-roty, transform.localEulerAngles.y, 0);
    }
}
