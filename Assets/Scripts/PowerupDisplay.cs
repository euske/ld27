using UnityEngine;
using System.Collections;

public class PowerupDisplay : MonoBehaviour {

    public static PowerupDisplay Instance;

    public Transform powerupExtraJumpPrefab;
    public Transform powerupGunPrefab;
    public Transform powerupTransparencyPrefab;

    private GUIStyle style_big;
    private GUIStyle style_normal;
    private Transform current;
    private int left;
    private bool visible;

    void Awake()
    {
        Instance = this;
        style_big = new GUIStyle();
        style_big.fontSize = 720;
        style_big.alignment = TextAnchor.MiddleCenter;
        style_big.normal.textColor = Color.white;
        style_normal = new GUIStyle();
        style_normal.fontSize = 72;
        style_normal.alignment = TextAnchor.MiddleCenter;
        style_normal.normal.textColor = Color.white;
    }

    void OnGUI()
    {
        if (visible) {
            if (0 < left) {
                Rect r = new Rect(Screen.width/2-100, Screen.height/2-100, 200, 200);
                GUI.Label(r, left.ToString(), style_big);
            } else if (current != null) {
                Rect r = new Rect(Screen.width/2+100, 0, 200, 72);
                GUI.Label(r, "Take this! --->", style_normal);
            }
        }
    }

    void Update()
    {
        visible = (((Time.time/0.2f) % 2) < 1);
    }

    void UpdateTimer(int v)
    {
        left = v;
    }

    void UpdateType(PowerupType powerup_type)
    {
        if (current) {
            Destroy(current.gameObject);
        }

        Transform prefab = null;
        switch (powerup_type) {
        case PowerupType.ExtraJump:
            prefab = powerupExtraJumpPrefab;
            break;
        case PowerupType.Gun:
            prefab = powerupGunPrefab;
            break;
        case PowerupType.Transparency:
            prefab = powerupTransparencyPrefab;
            break;
        }

        if (prefab != null) {
            current = (Instantiate(prefab, transform.position, transform.rotation) as Transform);
        }
    }
}
