using UnityEngine;

[CreateAssetMenu(fileName ="New Item", menuName ="Puzzle/New Item")]
public class Item : ScriptableObject {

    public new string name;
    public Sprite icon;

    public GameObject prefab;

    public AudioClip pickupSound;
}
