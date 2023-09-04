using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PokemonInfo : MonoBehaviour
{
	public TextMeshProUGUI nameText;
	public TextMeshProUGUI exp;
	public RawImage image;

	public static event System.Action<string, RawImage> OnSetPokemonCardData;

	public void SetPokemonCardData()
	{
		OnSetPokemonCardData?.Invoke(nameText.text, image);
		Debug.Log("set pokemon data called");
	}
}
