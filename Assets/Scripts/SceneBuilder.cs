using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum PlayerStatus
{
    None = 0,
    Died,
    Goaled,
}

public class SceneBuilder : MonoBehaviour
{
    public static SceneBuilder Instance;

    public const float range = 5.0f;
    public const float floorspeed = 30.0f;
    public const float floorlimit = 50.0f;
    //public const float goalpos = -30.0f; // testing
    public const float goalpos = +45.0f;
    public const float objinterval = 1.0f;
    public const float floortexratio = floorspeed/floorlimit*0.5f;
    public const float bottomy = -20.0f;

    public Transform[] blockPrefabs;
    public Transform[] foodPrefabs;
    public Transform[] powerupPrefabs;
    public Material floorMaterial;
    public AudioClip destroysound;
    public AudioClip pancakesound;
    private GUIStyle style_big;
    private GUIStyle style_normal;
    private PlayerStatus status = PlayerStatus.None;

    public int[][] distribution = {
        // -1: no object
        // 0,1,2,3,4: obstacle: large, lava, small, enemy, upper
        // 10, 11: food: normal, golden 
        // 20, 21, 22: powerup: extra jump, gun, transparency

        // mode 0: normal
        new int[] { -1, -1, -1, -1, -1, -1, -1,
                    0, 1, 2, 2, 3, 4,
                    10, 10,
                    20, 21, 22, },

        // mode 1: upper block
        new int[] { -1, -1, -1,
                    0, 2, 4, 4, 4, 
                    10, 
                    20, 21, },

        // mode 2: food!
        new int[] { -1, -1, -1, 
                    0, 2, 
                    10, 10, 10, 10, 10, 11, 
                    20, 21, },

        // mode 3: food and upper block
        new int[] { -1, -1, -1, 
                    2, 3, 4, 4,
                    10, 10, 10, 10, 11,
                    20, },

        // mode 4: too much enemies
        new int[] { -1, -1, 
                    0, 1, 2, 3, 3, 3, 4, 4,
                    10, 
                    20, 21, },

    };
    
    private int score;
    private int mode;
    private float mode_end;
    private float t0;
    private List<Transform> objects;
    private bool visible;
    
    void Awake()
    {
        Instance = this;
        style_big = new GUIStyle();
        style_big.fontSize = 360;
        style_big.alignment = TextAnchor.MiddleCenter;
        style_big.normal.textColor = Color.white;
        style_normal = new GUIStyle();
        style_normal.fontSize = 48;
        style_normal.alignment = TextAnchor.MiddleCenter;
        style_normal.normal.textColor = Color.white;
    }

    void Start () 
    {
        t0 = Time.time;
        score = 0;
        objects = new List<Transform>();
        changeMode();
    }

    void SetPlayerPos(Vector3 pos)
    {
        if (status == PlayerStatus.None) {
            if (goalpos < pos.z) {
                status = PlayerStatus.Goaled;
            } else if (pos.y < -1f) {
                status = PlayerStatus.Died;
                audio.PlayOneShot(pancakesound);
            }
        } 

        if (pos.y < bottomy) {
            switch (status) {
            case PlayerStatus.Goaled:
                Application.LoadLevel("scene2");
                break;
            case PlayerStatus.Died:
                Application.LoadLevel("scene0");
                break;
            }
        }
    }

    void UpdateScore(FoodType food)
    {
        score += (food == FoodType.Golden)? 3 : 1;
    }

    void SomethingDestroyed()
    {
        audio.PlayOneShot(destroysound);
    }

    void OnGUI()
    {
        switch (status) {
        case PlayerStatus.Died:
            GUI.Label(new Rect(32, 0, 200, 100), "You Failed!", style_normal);
            break;
        default:
            GUI.Label(new Rect(32, 0, 200, 100), ("Score: "+score), style_normal);
            break;
        }

        if (visible) {
            Rect r = new Rect(Screen.width/2-100, Screen.height/2-100, 200, 200);
            switch (status) {
            case PlayerStatus.Goaled:
                GUI.Label(r, ":)", style_big);
                break;
            case PlayerStatus.Died:
                GUI.Label(r, ":(", style_big);
                break;
            }
        }
    }

    private void changeMode()
    {
        mode = Random.Range(0, distribution.Length);
        mode_end = Time.time + Random.Range(1f, 3f);
    }

    private Transform getPrefab(int objtype)
    {
        if (objtype < 0) {
            return null;
        } else if (objtype < 10) {
            return blockPrefabs[objtype];
        } else if (objtype < 20) {
            return foodPrefabs[objtype-10];
        } else if (objtype < 30) {
            return powerupPrefabs[objtype-20];
        }
        return null;
    }

    void Update () {
        float t = Time.time;
        if (mode_end < t) {
            changeMode();
        }

        visible = (((t/0.2f) % 2) < 1);
        
        if (t0 + objinterval/floorspeed <= t) {
            t0 = t;
            int[] dist1 = distribution[mode];
            int objtype = dist1[Random.Range(0, dist1.Length)];
            Transform prefab = getPrefab(objtype);
            if (prefab != null) {
                Vector3 pos = new Vector3(Random.Range(-range, +range), 0, +floorlimit*2);
                Transform obj = Instantiate(prefab, pos, transform.rotation) as Transform;
                obj.parent = transform;
                objects.Add(obj);
            }
        }

        Vector3 v = new Vector3(0, 0, Time.deltaTime*(-floorspeed));
        List<Transform> destroyed = new List<Transform>();
        foreach (Transform obj in objects) {
            obj.Translate(v);
            if (obj.position.z < -floorlimit) {
                destroyed.Add(obj);
            }
        }
        foreach (Transform obj in destroyed) {
            objects.Remove(obj);
            Destroy(obj.gameObject);
        }

        if (floorMaterial) {
            floorMaterial.mainTextureOffset = new Vector2(0, t*floortexratio);
        }

    }
}
