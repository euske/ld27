using UnityEngine;
using System.Collections;

public class TitleScript : MonoBehaviour 
{
    void Update () {
        if (Input.GetButtonDown("Horizontal") ||
            Input.GetButtonDown("Vertical") ||
            Input.GetButtonDown("Jump") ||
            Input.GetButtonDown("Fire1")) {
            Application.LoadLevel("scene1");
        }
    }
}
