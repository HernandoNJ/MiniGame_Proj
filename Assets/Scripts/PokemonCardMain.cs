public class PokemonCardMain : PokemonCard
{
	private void OnEnable() => OnSendPokemonData += SetPokemonCardMainData;
	private void OnDisable() => OnSendPokemonData -= SetPokemonCardMainData;

	public void SetPokemonCardMainData(PokemonCard pokemonCardArg)
	{
		nameText.text = pokemonCardArg.nameText.text;
		exp.text = pokemonCardArg.exp.text;
		image.texture = pokemonCardArg.image.texture;
	}
}
