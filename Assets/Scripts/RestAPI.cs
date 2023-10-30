using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;
using TMPro;
using Cysharp.Threading.Tasks;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Pokemon.API
{
	public class RestAPI : MonoBehaviour
	{
		public string initialPokemonResultsJson;
		public string pokeUrl = "https://pokeapi.co/api/v2/pokemon/300";
		public string pokemonResultsUrl = "https://pokeapi.co/api/v2/pokemon?offset=300&limit=200";
		public string pokemonFromApiJson;
		public string rawImageUrl;
		public int pokeMaxCount;
		public int[] itemsCountArray;

		public GameObject pokeInfoPrefab;
		public GameObject newPokemon;
		public ParentHandler[] parentHandlers = new ParentHandler[3];
		public Texture2D newPokeTexture;

		public EvolutionChainRoot evolutionChainRoot;
		public List<Pokemon> pokemons;
		public PokemonResults pokemonResults;
		public PokemonSpecies pokemonSpecies;

		private void Start()
		{
			Invoke(nameof(GetPokemonsData), 0.1f);
		}

		private async UniTask GetPokemonsData()
		{
			await UniTask.Delay(1000);

			await GetPokemonResults();
			await GetPokemonFromAPI(pokemonResults.results[5].url);
			await GetPokemonEvolutions();
			//await GetPokemonTexture();


		}
		private async Task GetPokemonResults()
		{
			initialPokemonResultsJson =
				(await UnityWebRequest.Get(pokemonResultsUrl).SendWebRequest()).downloadHandler.text;
			pokemonResults = JsonConvert.DeserializeObject<PokemonResults>(initialPokemonResultsJson);
		}

		private async Task GetPokemonFromAPI(string url)
		{
			pokemonFromApiJson =
				(await UnityWebRequest.Get(url).SendWebRequest()).downloadHandler.text;
			var newPokemon = JsonConvert.DeserializeObject<Pokemon>(pokemonFromApiJson);
			pokemons.Add(newPokemon);
		}

		private async Task GetPokemonEvolutions()
		{
			var pokemonSpeciesJson0 =
				(await UnityWebRequest.Get(pokemons[0].species.url).SendWebRequest()).downloadHandler.text;
			pokemonSpecies = JsonConvert.DeserializeObject<PokemonSpecies>(pokemonSpeciesJson0);

			// Get and Set Evolution chain
			var evolutionChainJson = (await UnityWebRequest.Get(pokemonSpecies.evolution_chain.url).SendWebRequest()).downloadHandler.text;
			evolutionChainRoot = JsonConvert.DeserializeObject<EvolutionChainRoot>(evolutionChainJson);


			var evol1 = evolutionChainRoot.chain.evolves_to;
			if (evol1 != null)
			{
				//foreach (Chain chainItem in evol1)
				//{
				//	var newItemUrl = chainItem;
				//	await GetPokemonFromAPI(newItemUrl);
				//}
			}
		}
	}
}


//pokeApiV2Url = "https://pokeapi.co/api/v2/pokemon?offset=300&limit=200";
//pokeSpeciesUrl = "https://pokeapi.co/api/v2/pokemon-species/300";
//pokeApiV2Url = "https://pokeapi.co/api/v2/pokemon/ditto";
//pokeApiV2Url = "https://pokeapi.co/api/v2/pokemon/305";

//private async Task GetPokemonTexture()
//{
//	rawImageUrl = pokemons.sprites.front_default;
//	var pokemonTextureRequest = await UnityWebRequestTexture.GetTexture(rawImageUrl).SendWebRequest();
//	newPokeTexture = ((DownloadHandlerTexture)pokemonTextureRequest.downloadHandler).texture;

//	await SetOutputTextArea("Setting Pokemon Texture...", 500);
//}

//private void SetNewPokemonPrefab(int i)
//{
//	newPokemon = Instantiate(pokeInfoPrefab);

//	newPokemon.GetComponent<PokemonInfo>().nameText.text = pokemonFromApi.name;
//	newPokemon.GetComponent<PokemonInfo>().exp.text = pokemonFromApi.base_experience.ToString();
//	newPokemon.GetComponent<PokemonInfo>().image.texture = newPokeTexture;

//	if (i == 0) newPokemon.GetComponent<PokemonInfo>().SetPokemonCardData();

//	if (i < itemsCountArray[0]) SetParentHandler(0);
//	else if (i < itemsCountArray[1]) SetParentHandler(1);
//	else if (i < itemsCountArray[2]) SetParentHandler(2);

//	void SetParentHandler(int index)
//	{
//		newPokemon.transform.SetParent(parentHandlers[index].transform);
//		parentHandlers[index].AddPokemonToList(newPokemon);
//	}
//}
