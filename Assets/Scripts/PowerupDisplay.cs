using UnityEngine;
using System.Collections;

public class PowerupDisplay : MonoBehaviour {

    public static PowerupDisplay Instance;

    public Transform powerupExtraJumpPrefab;
    public Transform powerupGunPrefab;
    public Transform powerupTransparencyPrefab;

    private GUIStyle style;
    private Transform current;
    private int left;

    void Awake()
    {
        Instance = this;
        style = new GUIStyle();
        style.fontSize = 24;
        style.normal.textColor = Color.white;
    }

    void OnGUI()
    {
        if (0 < left) {
            GUI.Label(new Rect(0, 0, 100, 100), left.ToString());
        }
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
