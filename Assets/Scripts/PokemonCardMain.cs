using UnityEngine.UI;

public class PokemonCardMain : PokemonCard
{
	private void OnEnable() => OnSendPokemonData += SetPokemonCardMainData;
	private void OnDisable() => OnSendPokemonData -= SetPokemonCardMainData;

	public void SetPokemonCardMainData(string nameInfo, string expInfo, RawImage imageInfo)
	{
		nameText.text = nameInfo;
		exp.text = expInfo;
		image.texture = imageInfo.texture;
	}
}
