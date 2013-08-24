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
    public AudioClip powerupsound;
    public AudioClip ticksound;

    private bool landed;
    private int health;
    private int powerup;
    private int powerup_type;
    private float t0;

    void Start()
    {
        t0 = Time.time;
        landed = false;
        health = maxhealth;
        GameManager.Instance.SendMessage("SetHealth", health);
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
        if (0 < powerup) {
            float t = Time.time;
            if (t0+1.0f < t) {
                t0 = t;
                powerup--;
                audio.PlayOneShot(ticksound);
            }
        }
        float gravity = (0 < powerup && powerup_type == 0)? 0.0f : 10.0f;
        rigidbody.AddForce(Vector3.down * gravity);
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
            GameManager.Instance.SendMessage("SetHealth", health);
            
        } else if (col.gameObject.tag == "powerup_extra_jump") {
            if (powerupsound) {
                audio.PlayOneShot(powerupsound);
            }
            powerup = 10;
            powerup_type = 0;
            Destroy(col.gameObject);
            
        } else if (col.gameObject.tag == "powerup_gun") {
            if (powerupsound) {
                audio.PlayOneShot(powerupsound);
            }
            Destroy(col.gameObject);
            
        } else if (col.gameObject.tag == "powerup_transparency") {
            if (powerupsound) {
                audio.PlayOneShot(powerupsound);
            }
            Destroy(col.gameObject);
            
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
