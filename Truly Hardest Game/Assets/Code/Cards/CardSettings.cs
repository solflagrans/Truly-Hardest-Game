using UnityEngine;

[CreateAssetMenu(fileName = "Card Settings", menuName = "Card Settings")]
public class CardSettings : ScriptableObject
{

    [SerializeField] public sbyte _rating;
    [SerializeField] public Sprite _image;

}
