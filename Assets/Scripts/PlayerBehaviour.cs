using UnityEngine;
using System.Collections;

public class PlayerBehaviour : MonoBehaviour
{
    public const int TEN_SECONDS = 10;

    public const float range = 5.0f;
    public const float hspeed = 10.0f;
    public const float vspeed = 3.0f; // difficulty
    public const float hlimit = 4.5f;
    public const float jumpacc = 10.0f;
    public const float extrajumpheight = 4.0f;
    public const float extrajumpspeed = 10.0f;
    public static Vector2 knockback = new Vector2(-20.0f, +1.0f);
    public const float gun_interval = 0.1f;
    public const float walk_interval = 0.4f;
    public const float gravity = -20.0f;
    public static Color normal_color = Color.white;
    public static Color transparent_color = new Color(0f, 1f, 0.5f, 0.5f);
    
    public Transform bulletPrefab;
    public AudioClip walksound;
    public AudioClip jumpsound;
    public AudioClip extrajumpsound;
    public AudioClip landsound;
    public AudioClip eatsound;
    public AudioClip hitsound;
    public AudioClip powerupsound;
    public AudioClip ticksound;
    public AudioClip firesound;

    private bool landed;
    private float walk_tick;
    private int powerup_timer;
    private float powerup_tick;
    private PowerupType powerup_active;
    private PowerupType powerup_owned = PowerupType.None;
    private float gun_tick;

    void Start()
    {
        landed = false;
        PowerupDisplay.Instance.SendMessage("UpdateTimer", powerup_timer);
        PowerupDisplay.Instance.SendMessage("UpdateType", powerup_owned);
        setTransparency(false);
    }

    private void setTransparency(bool transparent)
    {
        if (transparent) {
            renderer.material.color = transparent_color;
            rigidbody.isKinematic = true;
            collider.enabled = false;
        } else {
            renderer.material.color = normal_color;
            rigidbody.isKinematic = false;
            collider.enabled = true;
        }
    }

    void FixedUpdate()
    {
        if (powerup_active != PowerupType.ExtraJump) {
            rigidbody.AddForce(Vector3.up * gravity);
        }
    }

    void Update()
    {
        float t = Time.time;

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
        
        if (landed && vx != 0) {
            if (walk_tick+walk_interval < t) {
                walk_tick = t;
                if (walksound) {
                    audio.PlayOneShot(walksound);
                }
            }
        } else {
            walk_tick = 0;
        }

	if (powerup_owned != PowerupType.None &&
            Input.GetButtonDown("Fire1")) {
            powerup_timer = TEN_SECONDS;
            powerup_tick = t;
            powerup_active = powerup_owned;
            powerup_owned = PowerupType.None;
            PowerupDisplay.Instance.SendMessage("UpdateTimer", powerup_timer);
            switch (powerup_active) {
            case PowerupType.ExtraJump:
                if (extrajumpsound) {
                    audio.PlayOneShot(extrajumpsound);
                }
                break;
            case PowerupType.Gun:
                gun_tick = t;
                break;
            case PowerupType.Transparency:
                if (powerupsound) {
                    audio.PlayOneShot(powerupsound);
                }
                setTransparency(true);
                break;
            }
        }

        if (powerup_active == PowerupType.Gun) {
            if (gun_tick+gun_interval < t) {
                gun_tick = t;
                if (firesound) {
                    audio.PlayOneShot(firesound, 0.5f);
                }
                Vector3 pos = (transform.up * transform.localScale.y * -0.5f +
                               transform.forward * transform.localScale.z);
                Instantiate(bulletPrefab, transform.position + pos, transform.rotation);
            }
        }

        float x = transform.position.x;

        if (powerup_active == PowerupType.ExtraJump) {
            x = Mathf.Clamp(transform.position.x, -range, +range);
            if (transform.position.y < extrajumpheight) {
                transform.Translate(Vector3.up * extrajumpspeed * Time.deltaTime);
            }
        }

        if (0 < powerup_timer) {
            if (powerup_tick+1.0f < t) {
                audio.PlayOneShot(ticksound);
                powerup_tick = t;
                powerup_timer--;
                PowerupDisplay.Instance.SendMessage("UpdateTimer", powerup_timer);
            }
            if (powerup_timer == 0) {
                setTransparency(false);
                powerup_active = PowerupType.None;
                PowerupDisplay.Instance.SendMessage("UpdateType", PowerupType.None);
            }
        }

        transform.position = new Vector3(
            x,
            Mathf.Clamp(transform.position.y, -100f, hlimit),
            Mathf.Clamp(transform.position.z, -100f, +100f));

        SceneBuilder.Instance.SendMessage("SetPlayerPos", transform.position);
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
                rigidbody.AddForce(Vector3.up * knockback.y +
                                   Vector3.forward * knockback.x,
                                   ForceMode.Impulse);
            }
            
	} else if (col.gameObject.tag == "food") {
            if (powerup_active != PowerupType.Transparency) {
                if (eatsound) {
                    audio.PlayOneShot(eatsound);
                }
                FoodBehaviour food = col.gameObject.GetComponent<FoodBehaviour>();
                SceneBuilder.Instance.SendMessage("UpdateScore", food.food_type);
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
