using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PokemonCard : MonoBehaviour
{
	public TextMeshProUGUI nameText;
	public TextMeshProUGUI exp;
	public RawImage image;

	public static event System.Action<string, string, RawImage> OnSendPokemonData;

	public void RaiseSendPokemonCardData()
	{
		Debug.Log("*** RaisePokemonData called");
		SendPokemonCardData(nameText.text, exp.text, image);
	}

	public void SendPokemonCardData(string nameArg, string expArg, RawImage imageArg)
	{
		OnSendPokemonData?.Invoke(nameArg, expArg, imageArg);
	}
}