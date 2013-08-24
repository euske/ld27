using UnityEngine;
using System.Collections;

public class PlayerMove : MonoBehaviour
{
    public float speed = 10.0f;
    public float jumpacc = 10.0f;
    public int maxhealth = 5;

    public AudioClip jumpsound;
    public AudioClip landsound;
    public AudioClip eatsound;
    public AudioClip hitsound;

    private bool landed;
    private int health;

    void Start()
    {
        landed = false;
        health = maxhealth;
    }

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

	} else if (col.gameObject.tag == "food") {
            if (eatsound) {
                audio.PlayOneShot(eatsound);
            }
            GameManager.Instance.SendMessage("ScoreIt");
            Destroy(col.gameObject);

        } else if (col.gameObject.tag == "block") {
            if (hitsound) {
                audio.PlayOneShot(hitsound);
            }
            health--;
            
        }
    }

    void OnCollisionExit(Collision col)
    {
	if (landed && col.gameObject.tag == "floor") {
            landed = false;
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
    }

}
