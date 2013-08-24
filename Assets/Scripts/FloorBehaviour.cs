using UnityEngine;
using System.Collections;

public class FloorBehaviour : MonoBehaviour {

    public float speed = 1.0f;
    public float rotspeed = 10.0f;
    public float scalespeed = 0.001f;

    private Vector3 direction;
    private Vector3 rotation;
    private Vector3 origscale;
    private float scaling;
    private float scale;

    void Start () {
        direction = Vector3.zero;
        rotation = Vector3.zero;
        origscale = transform.localScale;
        scale = 1.0f;
        scaling = 0;
    }
	
    void Update () {
        if (direction != Vector3.zero) {
            float r = speed * Time.deltaTime;
            transform.Translate(direction * r);
        }
        if (rotation != Vector3.zero) {
            float r = rotspeed * Time.deltaTime;
            transform.Rotate(rotation * r);
        }
        if (scaling != 0) {
            float r = scalespeed * Time.deltaTime;
            scale *= Mathf.Exp(scaling*r);
            scale = Mathf.Clamp(scale, 0.5f, 2.0f);
            transform.localScale = origscale * scale;
        }
    }

    void ChangeBehaviour()
    {
        int action = Random.Range(0, 6);

        switch (action) {
        case 0:
            direction = randvec();
            break;
        case 1:
            direction = Vector3.zero;
            break;
        case 2:
            rotation = randvec();
            break;
        case 3:
            rotation = Vector3.zero;
            break;
        case 4:
            scaling = Random.Range(-1.0f, +1.0f);
            break;
        case 5:
            scaling = 0;
            break;
        }
    }

    private Vector3 randvec()
    {
        return new Vector3(Random.Range(-1.0f, +1.0f), 
                           Random.Range(-1.0f, +1.0f),
                           Random.Range(-1.0f, +1.0f));
    }
}
