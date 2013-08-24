using UnityEngine;
using System.Collections;

public enum PowerupType
{
    None = 0,
    ExtraJump,
    Gun,
    Transparency,
}

public class PowerupBehaviour : MonoBehaviour 
{
    public PowerupType powerup_type;
}
