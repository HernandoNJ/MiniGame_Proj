using TMPro;
using UnityEngine;

public class PokemonCardMain : MonoBehaviour
{
	public MainCardInfo[] mainCardItems = new MainCardInfo[3];
	
	public static event System.Action<string> OnSetPokemonEvolutions;

	private void OnEnable() => PokemonCard.OnSendPokemonData += SetPokemonCardMainData;
	private void OnDisable() => PokemonCard.OnSendPokemonData -= SetPokemonCardMainData;

	public void SetPokemonCardMainData(string speciesUrlInfo)
	{
		OnSetPokemonEvolutions?.Invoke(speciesUrlInfo);
	}
}
