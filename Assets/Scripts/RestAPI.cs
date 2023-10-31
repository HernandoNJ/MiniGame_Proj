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
		public string pokeUrl;
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
			pokeUrl = "https://pokeapi.co/api/v2/pokemon/1";
			Invoke(nameof(GetPokemonsData), 0.1f);
		}

		private async UniTask GetPokemonsData()
		{
			await UniTask.Delay(1000);

			//await GetPokemonResults();
			//await GetPokemonFromAPI(pokemonResults.results[1].url);
			await GetPokemonFromAPI(pokeUrl);
			//await GetPokemonEvolutions(pokemons[0].species.url);
			await GetPokemonEvolutions(pokemons[0].species.url);
			//await GetPokemonTexture();


		}
		private async Task GetPokemonResults()
		{
			Debug.Log("Init GetPokemonResults");

			initialPokemonResultsJson =
				(await UnityWebRequest.Get(pokemonResultsUrl).SendWebRequest()).downloadHandler.text;
			pokemonResults = JsonConvert.DeserializeObject<PokemonResults>(initialPokemonResultsJson);

			Debug.Log("End GetPokemonResults");
		}

		private async Task GetPokemonFromAPI(string url)
		{

			Debug.Log("Init GetPokemonFromApi");
			Debug.Log("url: " + url);

			pokemonFromApiJson =
				(await UnityWebRequest.Get(url).SendWebRequest()).downloadHandler.text;
			var newPokemon = JsonConvert.DeserializeObject<Pokemon>(pokemonFromApiJson);
			pokemons.Add(newPokemon);

			Debug.Log("new Pokemon name: " + newPokemon.name);
			Debug.Log("pokemons[0] name: " + pokemons[0].name);
			Debug.Log("End GetPokemonFromApi");
		}

		private async Task GetPokemonEvolutions(string url)
		{
			Debug.Log("Init GetPokemonEvolutions");

			var pokemonSpeciesJson0 =
				(await UnityWebRequest.Get(url).SendWebRequest()).downloadHandler.text;
			pokemonSpecies = JsonConvert.DeserializeObject<PokemonSpecies>(pokemonSpeciesJson0);

			Debug.Log("pokemonSpecies.name: " + pokemonSpecies.name);

			// Get and Set Evolution chain
			var evolutionChainJson = (await UnityWebRequest.Get(pokemonSpecies.evolution_chain.url).SendWebRequest()).downloadHandler.text;
			evolutionChainRoot = JsonConvert.DeserializeObject<EvolutionChainRoot>(evolutionChainJson);
			Debug.Log("evolutionChainRoot id: " + evolutionChainRoot.id);

			var chain1 = evolutionChainRoot.chain;
			if (chain1.evolves_to.Count > 0)
			{
				// Level 0-
				Debug.Log("chain1.evolves_to.Count > 0");
				Debug.Log("chain1.evolves_to[0], species name: " + chain1.evolves_to[0].species.name);

				// Level 0-0
				if (chain1.evolves_to[0].evolves_to.Count > 0)
				{
					Debug.Log("chain1.evolves_to.Count > 0");
					Debug.Log("chain1.evolves_to[0].evolves_to[0], level 0-2, species name: " + chain1.evolves_to[0].evolves_to[0].species.name);

					// Level 0-0-0
					if (chain1.evolves_to[0].evolves_to[0].evolves_to.Count > 0)
					{
						Debug.Log("chain1.evolves_to.Count > 0");
						Debug.Log("chain1.evolves_to[0].evolves_to[0].evolves_to.Count > 0, level 0-3, species name: " + chain1.evolves_to[0].evolves_to[0].evolves_to[0].species.name);

						// Level 0-0-0-0
						if (chain1.evolves_to[0].evolves_to[0].evolves_to[0].evolves_to.Count > 0)
						{
							Debug.Log("chain1.evolves_to[0].evolves_to[0].evolves_to[0].evolves_to.Count > 0, level 0-4, species name: " + chain1.evolves_to[0].evolves_to[0].evolves_to[0].evolves_to[0].species.name);

							// Level 0-0-0-0-0
							if (chain1.evolves_to[0].evolves_to[0].evolves_to[0].evolves_to[0].evolves_to.Count > 0)
							{
								Debug.Log("chain1.evolves_to[0].evolves_to[0].evolves_to[0].evolves_to[0].evolves_to.Count > 0, level 0-5, species name: " + chain1.evolves_to[0].evolves_to[0].evolves_to[0].evolves_to[0].evolves_to[0].species.name);
							}
							else { Debug.Log("quit level 5, return;"); return; }
						}
						else { Debug.Log("quit level 4, return;"); return; }
					}
					else { Debug.Log("quit level 3, return;"); return; }
				}
				else { Debug.Log("quit level 2, return;"); return; }
			}
			else { Debug.Log("quit level 1, return;"); return; }

			Debug.Log("End GetPokemonEvolutions");

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
