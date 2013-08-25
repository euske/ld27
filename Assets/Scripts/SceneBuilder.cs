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
    public const int tiling = 100;
    public const float timeout = 2f;
    public const float goalpos = +50.0f;
    public const float objinterval = 1.5f;
    public const float floortexratio = floorspeed/floorlimit*tiling*0.5f;
    public const float mode_min = 0.5f;
    public const float mode_max = 4.0f;

    public Transform[] blockPrefabs;
    public Transform[] foodPrefabs;
    public Transform[] powerupPrefabs;
    public GameObject floorObject;
    public Material floorMaterial;
    public AudioClip destroysound;
    public AudioClip pancakesound;
    private GUIStyle2 style_big;
    private GUIStyle2 style_normal;
    private PlayerStatus status = PlayerStatus.None;

    public int[][] distribution = {
        // -1: no object
        // 0,1,2,3,4: obstacle: large, lava, small, enemy, upper
        // 10, 11: food: normal, golden 
        // 20, 21, 22: powerup: extra jump, gun, transparency

        // mode 0: normal
        new int[] { -1, -1, -1, -1, -1, -1, -1, -1, -1,
                    0, 1, 1, 1, 2, 2, 3, 4, 
                    10, 10, 10, 10,
                    20, 21, 22, },

        // mode 1: upper block
        new int[] { -1, -1, -1, -1, -1, 
                    0, 2, 2, 4, 4, 4, 
                    10, 10, 
                    20, 22, },

        // mode 2: food!
        new int[] { -1, -1, -1, 
                    0, 2, 
                    10, 10, 10, 10, 10, 11, 
                    20, 21, 22, },

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

        // mode 5: no food
        new int[] { -1, -1, -1, -1, -1, -1,
                    0, 2, 3, 4, 
                    22, },

    };
    
    private int score;
    private int mode;
    private float mode_end;
    private float game_end;
    private float t0;
    private List<Transform> objects;
    private bool visible;

    void Awake()
    {
        Instance = this;
        style_big = new GUIStyle2(360, Color.white);
        style_normal = new GUIStyle2(49, Color.white);
        if (floorObject && floorMaterial) {
            Renderer renderer = floorObject.GetComponent<Renderer>();
            renderer.material.CopyPropertiesFromMaterial(floorMaterial);
        }
    }

    void Start () 
    {
        t0 = Time.time;
        score = 0;
        objects = new List<Transform>();
        changeMode();
        GameManager.Instance.score = 0;
    }

    void SetPlayerPos(Vector3 pos)
    {
        if (status == PlayerStatus.None) {
            if (goalpos < pos.z) {
                status = PlayerStatus.Goaled;
                game_end = Time.time + timeout;
            } else if (pos.y < -1f) {
                status = PlayerStatus.Died;
                game_end = Time.time + timeout;
                audio.PlayOneShot(pancakesound);
            }
        } 
    }

    void UpdateScore(FoodType food)
    {
        score += (food == FoodType.Golden)? 3 : 1;
        GameManager.Instance.score = score;
    }

    void SomethingDestroyed()
    {
        audio.PlayOneShot(destroysound);
    }

    void OnGUI()
    {
        switch (status) {
        case PlayerStatus.Died:
            style_normal.Render(new Rect(32, 0, 200, 100), "You Failed!");
            break;
        default:
            style_normal.Render(new Rect(32, 0, 200, 100), ("Score: "+score));
            break;
        }

        if (visible) {
            Rect r = new Rect(Screen.width/2-100, Screen.height/2-100, 200, 200);
            switch (status) {
            case PlayerStatus.Goaled:
                style_big.Render(r, ":)");
                break;
            case PlayerStatus.Died:
                style_big.Render(r, ":(");
                break;
            }
        }
    }

    private void changeMode()
    {
        mode = Random.Range(0, distribution.Length);
        mode_end = Time.time + Random.Range(mode_min, mode_max);
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
        if (status != PlayerStatus.None && game_end < t) {
            switch(status) {
            case PlayerStatus.Died:
                Application.LoadLevel("scene0");
                break;
            case PlayerStatus.Goaled:
                Application.LoadLevel("scene2");
                break;
            }
        }

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

        if (floorObject) {
            Renderer renderer = floorObject.GetComponent<Renderer>();
            renderer.material.CopyPropertiesFromMaterial(floorMaterial);
            renderer.material.mainTextureOffset = new Vector2(0, t*floortexratio);
        }

    }
}
