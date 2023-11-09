using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PokemonCard : MonoBehaviour
{
	public TextMeshProUGUI nameText;
	public TextMeshProUGUI exp;
	public RawImage image;
	public string speciesUrl;

	public static event System.Action<string> OnSendPokemonData;

	public void RaiseSendPokemonCardData() => SendPokemonCardData(speciesUrl);

	public void SendPokemonCardData(string speciesUrlArg) => OnSendPokemonData?.Invoke(speciesUrlArg);
}