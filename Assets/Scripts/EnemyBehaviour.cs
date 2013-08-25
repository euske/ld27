using UnityEngine;
using System.Collections;

public class EnemyBehaviour : MonoBehaviour {

    public const float range = 5.0f;
    public const float speed = 5.0f;

    public Transform shape;

    private float targetx;
    private float shrinkrate;

    void Start () {
        shrinkrate = Random.Range(0f, 20f);
        updateTarget();
    }
	
    void Update () {
        float r = speed * Time.deltaTime;
        float dx = targetx - transform.position.x;
        float vx = (dx < 0) ? -1 : +1;
        transform.Translate(new Vector3(vx*r, 0, 0));
        if (Mathf.Abs(dx) < 1.0f) {
            updateTarget();
        }

        if (shape) {
            float scaley = 1.0f + 0.5f * Mathf.Sin(Time.time * shrinkrate);
            shape.localScale = new Vector3(1.0f, scaley, 1.0f);
            shape.localPosition = new Vector3(0.0f, scaley, 0.0f);
        }
    }

    private void updateTarget()
    {
        targetx = Random.Range(-range, +range);
    }
}
