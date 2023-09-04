using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PokemonCard : MonoBehaviour
{
	public TextMeshProUGUI nameText;
	public RawImage image;

	private void Start()
	{
		PokemonInfo.OnSetPokemonCardData += GetPokemonData;
		Debug.Log("start called in pokemon card");
	}

	private void GetPokemonData(string nameArg, RawImage imageArg)
	{
		nameText.text = nameArg; image.texture = imageArg.texture;
		Debug.Log("get pokemon data called");
	}
}
