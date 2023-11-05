using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PokemonCard : MonoBehaviour
{
	public TextMeshProUGUI nameText;
	public TextMeshProUGUI exp;
	public RawImage image;

	public static event System.Action<PokemonCard> OnSendPokemonData;

	public void SetPokemonCardData(PokemonCard pokemonCardArg)
	{
		pokemonCardArg = this;
		OnSendPokemonData?.Invoke(pokemonCardArg);
	}
}