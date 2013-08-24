using UnityEngine;
using System.Collections;

public class PlayerMove : MonoBehaviour
{
    public float speed = 10.0f;
    public float jumpacc = 10.0f;

    public AudioClip jumpsound;
    public AudioClip landsound;

    private bool landed = false;

    void Update()
    {
	float r = speed * Time.deltaTime;
        float vx = Input.GetAxis("Horizontal") * r;
        float vz = Input.GetAxis("Vertical") * r;
        Move(vx, vz);
        if (landed && Input.GetButtonDown("Jump")) {
            Jump();
        }
    }

    void OnCollisionEnter(Collision col)
    {
	if (!landed && col.gameObject.tag == "floor") {
            if (landsound) {
                audio.PlayOneShot(landsound);
            }
	    landed = true;
	}
    }

    private void Move(float vx, float vz)
    {
	Vector3 v = (Vector3.right * vx + Vector3.forward * vz);
	transform.Translate(v);
    }

    private void Jump()
    {
        rigidbody.AddForce(Vector3.up * jumpacc, ForceMode.Impulse);
        if (jumpsound) {
            audio.PlayOneShot(jumpsound);
        }
        landed = false;
    }

}
