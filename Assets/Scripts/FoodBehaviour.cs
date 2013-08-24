using UnityEngine;
using System.Collections;

public enum FoodType
{
    None = 0,
    Normal,
    Golden,
}

public class FoodBehaviour : MonoBehaviour 
{
    public FoodType food_type;
}
