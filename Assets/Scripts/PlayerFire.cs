using UnityEngine;
using System.Collections;

public class PlayerFire : MonoBehaviour
{
    public Transform bulletPrefab;
    public AudioClip firesound;

    void Update()
    {
	if (Input.GetButtonDown("Fire1")) {
	    Vector3 pos = transform.position + transform.forward;
	    Instantiate(bulletPrefab, pos, transform.rotation);
            if (firesound) {
                audio.PlayOneShot(firesound);
            }
	}
    }
}
