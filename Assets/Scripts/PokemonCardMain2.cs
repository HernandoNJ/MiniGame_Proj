using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PokemonCardMain2 : MonoBehaviour
{
	public TextMeshProUGUI nameText;
	public TextMeshProUGUI exp;
	public RawImage image;

	private void OnEnable() => PokemonCard.OnSendPokemonData += SetPokemonCardMainData;
	private void OnDisable() => PokemonCard.OnSendPokemonData -= SetPokemonCardMainData;

	public void SetPokemonCardMainData(string nameInfo, string expInfo, RawImage imageInfo)
	{
		nameText.text = nameInfo;
		exp.text = expInfo;
		image.texture = imageInfo.texture;
	}
}
