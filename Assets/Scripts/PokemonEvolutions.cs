using UnityEngine;

public class PokemonEvolutions : MonoBehaviour
{
	[SerializeField] private PokemonCard[] pokemonCardItems;

	public PokemonCard[] GetPokemonCardItems () => pokemonCardItems;
}
