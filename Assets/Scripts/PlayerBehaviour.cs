using UnityEngine;
using System.Collections;

public class PlayerBehaviour : MonoBehaviour
{
    public const int TEN_SECONDS = 10;

    public const float hspeed = 10.0f;
    public const float vspeed = 2.0f;
    public const float jumpacc = 10.0f;
    public const float extrajumpduration = 0.3f;
    public static Vector2 setback = new Vector2(-10.0f, +1.0f);
    public const float gun_interval = 0.1f;
    public const float gravity = -20.0f;
    public static Color normal_color = Color.white;
    public static Color transparent_color = new Color(0f, 1f, 0.5f, 0.2f);
    
    public Transform bulletPrefab;
    public AudioClip jumpsound;
    public AudioClip landsound;
    public AudioClip eatsound;
    public AudioClip hitsound;
    public AudioClip powerupsound;
    public AudioClip ticksound;
    public AudioClip firesound;

    private bool landed;
    private int powerup_timer;
    private float powerup_tick;
    private PowerupType powerup_active;
    private PowerupType powerup_owned = PowerupType.Transparency;
    private float gun_tick;
    private float jump_end;

    void Start()
    {
        landed = false;
        PowerupDisplay.Instance.SendMessage("UpdateTimer", powerup_timer);
        PowerupDisplay.Instance.SendMessage("UpdateType", powerup_owned);
        renderer.material.color = normal_color;
    }

    void Update()
    {
        float vx = Input.GetAxis("Horizontal") * hspeed * Time.deltaTime;
        //float vz = Input.GetAxis("Vertical") * r;
        float vz = vspeed * Time.deltaTime;
	transform.Translate(Vector3.right * vx + 
                            Vector3.forward * vz);

        if (landed && 
            powerup_active != PowerupType.ExtraJump &&
            Input.GetButtonDown("Jump")) {
            if (jumpsound) {
                audio.PlayOneShot(jumpsound);
            }
            rigidbody.AddForce(Vector3.up * jumpacc, ForceMode.Impulse);
        }

	if (powerup_owned != PowerupType.None &&
            Input.GetButtonDown("Fire1")) {
            if (powerupsound) {
                audio.PlayOneShot(powerupsound);
            }
            powerup_timer = TEN_SECONDS;
            powerup_tick = Time.time;
            powerup_active = powerup_owned;
            powerup_owned = PowerupType.None;
            PowerupDisplay.Instance.SendMessage("UpdateTimer", powerup_timer);
            switch (powerup_active) {
            case PowerupType.ExtraJump:
                rigidbody.AddForce(Vector3.up * jumpacc, ForceMode.Impulse);
                jump_end = Time.time + extrajumpduration;
                break;
            case PowerupType.Gun:
                gun_tick = Time.time;
                break;
            case PowerupType.Transparency:
                renderer.material.color = transparent_color;
                break;
            }
        }

        if (powerup_active == PowerupType.Gun) {
            float t = Time.time;
            if (gun_tick+gun_interval < t) {
                gun_tick = t;
                if (firesound) {
                    audio.PlayOneShot(firesound);
                }
                Vector3 pos = (transform.up * transform.localScale.y * -0.5f +
                               transform.forward * transform.localScale.z);
                Instantiate(bulletPrefab, transform.position + pos, transform.rotation);
            }
        }

        if (powerup_active == PowerupType.ExtraJump) {
            float t = Time.time;
            if (0 < jump_end && jump_end < t) {
                rigidbody.AddForce(Vector3.up * (-jumpacc), ForceMode.Impulse);
                jump_end = -1;
            }
        } else {
            rigidbody.AddForce(Vector3.up * gravity);
        }

        if (0 < powerup_timer) {
            float t = Time.time;
            if (powerup_tick+1.0f < t) {
                audio.PlayOneShot(ticksound);
                powerup_tick = t;
                powerup_timer--;
                PowerupDisplay.Instance.SendMessage("UpdateTimer", powerup_timer);
            }
            if (powerup_timer == 0) {
                switch (powerup_active) {
                case PowerupType.Transparency:
                    renderer.material.color = normal_color;
                    break;
                }
                powerup_active = PowerupType.None;
                PowerupDisplay.Instance.SendMessage("UpdateType", PowerupType.None);
            }
        }

        if (transform.position.y < -1f) {
            GameManager.Instance.SendMessage("GameOver");
        }
    }

    void OnCollisionEnter(Collision col)
    {
	if (!landed && col.gameObject.tag == "floor") {
            if (landsound) {
                audio.PlayOneShot(landsound);
            }
	    landed = true;

        } else if (col.gameObject.tag == "block" ||
                   col.gameObject.tag == "enemy") {
            if (powerup_active != PowerupType.Transparency) {
                if (hitsound) {
                    audio.PlayOneShot(hitsound);
                }
                rigidbody.AddForce(Vector3.up * setback.y +
                                   Vector3.forward * setback.x,
                                   ForceMode.Impulse);
            }
            
	} else if (col.gameObject.tag == "food") {
            if (powerup_active != PowerupType.Transparency) {
                if (eatsound) {
                    audio.PlayOneShot(eatsound);
                }
                FoodBehaviour food = col.gameObject.GetComponent<FoodBehaviour>();
                GameManager.Instance.SendMessage("UpdateScore", food.food_type);
                Destroy(col.gameObject);
            }

        } else if (col.gameObject.tag == "powerup") {
            if (powerup_active == PowerupType.None) {
                if (eatsound) {
                    audio.PlayOneShot(eatsound);
                }
                PowerupBehaviour powerup = col.gameObject.GetComponent<PowerupBehaviour>();
                powerup_owned = powerup.powerup_type;
                PowerupDisplay.Instance.SendMessage("UpdateType", powerup_owned);
                Destroy(col.gameObject);
            }
        }
    }

    void OnCollisionExit(Collision col)
    {
	if (landed && col.gameObject.tag == "floor") {
            landed = false;
        }
    }

}
