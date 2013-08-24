using UnityEngine;
using System.Collections;

public class PlayerBehaviour : MonoBehaviour
{
    public const int TEN_SECONDS = 10;

    public const float speed = 10.0f;
    public const float jumpacc = 6.0f;
    public const float extrajumpacc = 5.0f;
    public const int maxhealth = 5;
    public const float setback = 5.0f;
    public const float gun_interval = 0.1f;
    public const float gravity = -20.0f;
    
    public Transform bulletPrefab;
    public AudioClip jumpsound;
    public AudioClip landsound;
    public AudioClip eatsound;
    public AudioClip hitsound;
    public AudioClip powerupsound;
    public AudioClip ticksound;
    public AudioClip firesound;

    private bool landed;
    private int health;
    private int powerup_timer;
    private float powerup_tick;
    private PowerupType powerup_active;
    private PowerupType powerup_owned = PowerupType.ExtraJump;
    private float gun_tick;

    void Start()
    {
        landed = false;
        health = maxhealth;
        GameManager.Instance.SendMessage("SetHealth", health);
        PowerupDisplay.Instance.SendMessage("UpdateTimer", powerup_timer);
        PowerupDisplay.Instance.SendMessage("UpdateType", powerup_owned);
    }

    void Update()
    {
	float r = speed * Time.deltaTime;
        float vx = Input.GetAxis("Horizontal") * r;
        float vz = Input.GetAxis("Vertical") * r;
	transform.Translate(Vector3.right * vx + 
                            Vector3.forward * vz);

        if (landed && 
            powerup_active != PowerupType.ExtraJump &&
            Input.GetButtonDown("Jump")) {
            if (jumpsound) {
                audio.PlayOneShot(jumpsound);
            }
            rigidbody.AddForce(transform.up * jumpacc, ForceMode.Impulse);
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
                rigidbody.AddForce(transform.up * extrajumpacc, ForceMode.Impulse);
                break;
            case PowerupType.Gun:
                gun_tick = Time.time;
                break;
            case PowerupType.Transparency:
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
                Vector3 pos = transform.position + transform.forward * transform.localScale.z;
                Instantiate(bulletPrefab, pos, transform.rotation);
            }
        }

        if (powerup_active != PowerupType.ExtraJump) {
            rigidbody.AddForce(transform.up * gravity);
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

        } else if (col.gameObject.tag == "block") {
            if (powerup_active != PowerupType.Transparency) {
                if (hitsound) {
                    audio.PlayOneShot(hitsound);
                }
                health--;
                GameManager.Instance.SendMessage("SetHealth", health);
                rigidbody.AddForce(transform.up * setback +
                                   transform.forward * -setback,
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
