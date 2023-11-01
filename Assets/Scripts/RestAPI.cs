using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;
using TMPro;
using Cysharp.Threading.Tasks;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;
using static System.Net.WebRequestMethods;

namespace Pokemon.API
{
	public class RestAPI : MonoBehaviour
	{
		public string initialPokemonResultsJson;
		public string pokeUrl;
		public string pokemonResultsUrl;
		public string pokemonFromApiJson;
		public string rawImageUrl;
		public int pokeMaxCount;
		public int pokeChainIdsCounter;
		public int[] itemsCountArray;

		public GameObject pokemonCardGroupPrefab;
		public GameObject newPokemon;
		public ParentHandler[] parentHandlers = new ParentHandler[3];
		public Texture2D newPokeTexture;

		public EvolutionChainRoot evolutionChainRoot;
		public List<Pokemon> pokemons;
		public List<int> pokemonsIdList;
		public List<int> evolutionChainsIdList;
		public Pokemon pokemon;
		public PokemonResults pokemonResults;
		public PokemonSpecies pokemonSpecies;

		private void Start()
		{
			pokeUrl = "https://pokeapi.co/api/v2/pokemon/1";
			pokemonResultsUrl = "https://pokeapi.co/api/v2/pokemon?offset=299&limit=200";

			Invoke(nameof(GetPokemonsData), 0.1f);
		}

		private async UniTask GetPokemonsData()
		{
			await GetPokemonResults();
			//await GetPokemonFromAPI(pokemonResults.results[1].url);
			//await GetPokemonsFromAPI(pokeUrl);
			//await GetPokemonEvolutions(pokemons[0].species.url);
			//await GetPokemonEvolutions(pokemons[0].species.url);
			//await GetPokemonTexture();


		}
		private async Task GetPokemonResults()
		{
			initialPokemonResultsJson =
				(await UnityWebRequest.Get(pokemonResultsUrl).SendWebRequest()).downloadHandler.text;

			pokemonResults = JsonConvert.DeserializeObject<PokemonResults>(initialPokemonResultsJson);

			for (int i = 0; i < 20; i++)
				await GetPokemonsFromAPI(pokemonResults.results[i].url);
		}

		private async Task GetPokemonsFromAPI(string url)
		{
			pokemonFromApiJson =
				(await UnityWebRequest.Get(url).SendWebRequest()).downloadHandler.text;

			pokemon = JsonConvert.DeserializeObject<Pokemon>(pokemonFromApiJson);
			await GetPokemonEvolutions(pokemon.species.url);
		}

		private async Task<Pokemon> GetPokemonFromSpeciesUrl(string stringSuffix)
		{
			var newUrl = "https://pokeapi.co/api/v2/pokemon/" + stringSuffix;
			var json = (await UnityWebRequest.Get(newUrl).SendWebRequest()).downloadHandler.text;

			return pokemon = JsonConvert.DeserializeObject<Pokemon>(json);
		}

		private async Task<Texture2D> GetPokemonTexture(string url)
		{
			var pokemonTextureRequest = await UnityWebRequestTexture.GetTexture(url).SendWebRequest();
			return newPokeTexture = ((DownloadHandlerTexture)pokemonTextureRequest.downloadHandler).texture;
		}

		private async Task GetPokemonEvolutions(string url)
		{
			var pokemonSpeciesJson =
				(await UnityWebRequest.Get(url).SendWebRequest()).downloadHandler.text;

			pokemonSpecies = JsonConvert.DeserializeObject<PokemonSpecies>(pokemonSpeciesJson);

			// Get and Set Evolution chain
			var evolutionChainJson =
				(await UnityWebRequest.Get(pokemonSpecies.evolution_chain.url).SendWebRequest()).downloadHandler.text;

			evolutionChainRoot = JsonConvert.DeserializeObject<EvolutionChainRoot>(evolutionChainJson);

			// If list contains id, it means the pokemons were already created, return
			if (evolutionChainsIdList.Contains(evolutionChainRoot.id)) return;
			else
			{
				evolutionChainsIdList.Add(evolutionChainRoot.id);
				pokeChainIdsCounter++;
			}

			if (pokeChainIdsCounter < 101) Instantiate(pokemonCardGroupPrefab, parentHandlers[0].transform);
			else if (pokeChainIdsCounter < 201) Instantiate(pokemonCardGroupPrefab, parentHandlers[1].transform);
			else if (pokeChainIdsCounter < 301) Instantiate(pokemonCardGroupPrefab, parentHandlers[2].transform);


			var chain1 = evolutionChainRoot.chain;

			if (chain1.evolves_to.Count > 0)
			{
				await SetNewPokemonCard(chain1.evolves_to[0].species.name, 0);
				
				if (chain1.evolves_to[0].evolves_to.Count > 0)
				{
					await SetNewPokemonCard(chain1.evolves_to[0].evolves_to[0].species.name, 1);

					if (chain1.evolves_to[0].evolves_to[0].evolves_to.Count > 0)
					{
						await SetNewPokemonCard(chain1.evolves_to[0].evolves_to[0].evolves_to[0].species.name, 2);
						
					}
				}
			}
			Debug.Log("End GetPokemonEvolutions");

			async Task SetNewPokemonCard(string pokeSpeciesName, int pokemonCardsIndex)
			{
				var newPokemon1 = await GetPokemonFromSpeciesUrl(pokeSpeciesName);

				var newPokemonCard = pokemonCardGroupPrefab.GetComponent<PokemonCardGroup>().pokemonCards[pokemonCardsIndex];
				
				newPokemonCard.gameObject.SetActive(true);
				newPokemonCard.GetComponent<PokemonCard>().name = newPokemon1.name;
				newPokemonCard.GetComponent<PokemonCard>().image.texture = await GetPokemonTexture(newPokemon1.sprites.front_default);
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
