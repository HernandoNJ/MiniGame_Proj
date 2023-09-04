using TMPro;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "PokemonData", menuName = "ScriptableObjects/PokemonData")]
public class PokemonData : ScriptableObject
{
    public Texture2D imageTexture;
    public string nameText;
    public string exp;

}
