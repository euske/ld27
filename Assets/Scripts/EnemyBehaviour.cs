using UnityEngine;
using System.Collections;

public class EnemyBehaviour : MonoBehaviour {

    public float speed = 5.0f;

    private Transform target;

    void Start () {
        GameObject player = GameObject.FindWithTag("player");
        if (player) {
            target = player.GetComponent<Transform>();
        }
    }
	
    void Update () {
        if (target) {
            float r = speed * Time.deltaTime;
            float dx = target.position.x - transform.position.x;
            float vx = (dx < 0) ? -1 : +1;
            transform.Translate(new Vector3(vx*r, 0, 0));
        }
    }
}
