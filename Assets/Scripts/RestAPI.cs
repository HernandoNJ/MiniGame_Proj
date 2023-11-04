using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;

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

		public Chain currentChain;
		public EvolutionChainRoot evolutionChainRoot;
		public List<Pokemon> pokemonsList;
		public List<Pokemon> checkedPokemons;
		public Pokemon[] pokemonsGroup;
		public List<int> pokemonsIdList;
		public List<int> evolutionChainsIdList;
		public Pokemon pokemon;
		public PokemonResults pokemonResults;
		public PokemonSpecies pokemonSpecies;

		private void Start()
		{
			pokeUrl = "https://pokeapi.co/api/v2/pokemon/300";
			pokemonResultsUrl = "https://pokeapi.co/api/v2/pokemon?offset=1&limit=200";

			Invoke(nameof(GetPokemonsData), 0.1f);
		}

		private async UniTask GetPokemonsData()
		{
			await GetPokemonResults();
			//await GetPokemonFromAPI(pokeUrl);
			//await GetPokemonFromAPI(pokemonResults.results[1].url);
			//await GetPokemonEvolutions(pokemons[0].species.url);
			//await GetPokemonEvolutions(pokemons[0].species.url);
			//await GetPokemonTexture();


		}

		private async UniTask GetPokemonResults()
		{
			initialPokemonResultsJson =
				(await UnityWebRequest.Get(pokemonResultsUrl).SendWebRequest()).downloadHandler.text;

			pokemonResults = JsonConvert.DeserializeObject<PokemonResults>(initialPokemonResultsJson);

			for (int i = 0; i < 20; i++)
				await GetPokemonFromAPI(pokemonResults.results[i].url);
		}

		private async UniTask GetPokemonFromAPI(string url)
		{
			pokemonFromApiJson =
				(await UnityWebRequest.Get(url).SendWebRequest()).downloadHandler.text;

			pokemon = JsonConvert.DeserializeObject<Pokemon>(pokemonFromApiJson);
			await GetPokemonEvolutions(pokemon.species.url);
		}

		private async UniTask<Pokemon> GetPokemonFromSpeciesUrl(string stringSuffix)
		{
			var newUrl = "https://pokeapi.co/api/v2/pokemon/" + stringSuffix;
			var json = (await UnityWebRequest.Get(newUrl).SendWebRequest()).downloadHandler.text;
			var pokemonFromSpecies = JsonConvert.DeserializeObject<Pokemon>(json);

			return pokemonFromSpecies;
		}

		private async UniTask GetPokemonEvolutions(string url)
		{
			var pokemonSpeciesJson =
				(await UnityWebRequest.Get(url).SendWebRequest()).downloadHandler.text;

			pokemonSpecies = JsonConvert.DeserializeObject<PokemonSpecies>(pokemonSpeciesJson);

			// Get and Set Evolution chain root
			var evolutionChainJson =
				(await UnityWebRequest.Get(pokemonSpecies.evolution_chain.url).SendWebRequest()).downloadHandler.text;

			evolutionChainRoot = JsonConvert.DeserializeObject<EvolutionChainRoot>(evolutionChainJson);

			// If list contains id, it means the pokemons were already created, return
			if (evolutionChainsIdList.Contains(evolutionChainRoot.id)) return;

			evolutionChainsIdList.Add(evolutionChainRoot.id);
			Debug.Log("evolution chain id: " + evolutionChainRoot.id);
			currentChain = evolutionChainRoot.chain;

			string url0, url1, url2;

			url0 = currentChain.species.name;
			await AddPokemonToLists(url0, 0);

			if (currentChain.evolves_to.Count > 0)
			{
				url1 = currentChain.evolves_to[0].species.name;
				await AddPokemonToLists(url1, 1);

				if (currentChain.evolves_to[0].evolves_to.Count > 0)
				{
					url2 = currentChain.evolves_to[0].evolves_to[0].species.name;
					await AddPokemonToLists(url2, 2);
				}
			}

			Debug.Log("Returning to Get Pokemon Evolutions ");

			// Local function
			async UniTask<Object> AddPokemonToLists(string url, int index)
			{
				var pokemon = await GetPokemonFromSpeciesUrl(url);
				if (pokemon != null)
				{
					pokemonsGroup[index] = pokemon;
					pokemonsList.Add(pokemon);
					checkedPokemons.Add(pokemon);
					Debug.Log("pokemon name" + index + " : " + pokemonsGroup[index].name);
				}

				return null;
			}


			//pokemonsArray[0] = await GetPokemonFromSpeciesUrl(speciesName0);
			//pokemonsArray[1] = await GetPokemonFromSpeciesUrl(speciesName1);
			//pokemonsArray[2] = await GetPokemonFromSpeciesUrl(speciesName2);
			//Debug.Log($"pk0: {pokemonsArray[0].name}, pk1: {pokemonsArray[1].name}, pk2: {pokemonsArray[2].name}");
			//Debug.Log("Returning to Get Pokemon Evolutions ");
		}

		private async UniTask<Texture2D> GetPokemonTexture(string url)
		{
			var pokemonTextureRequest = await UnityWebRequestTexture.GetTexture(url).SendWebRequest();
			return newPokeTexture = ((DownloadHandlerTexture)pokemonTextureRequest.downloadHandler).texture;
		}



		//private async UniTask SetNewPokemonCard(string url, int pokemonCardsIndex)
		//{
		//	var newPokemon1 = await GetPokemonFromSpeciesUrl(url);

		//	var newPokemonCard = pokemonCardGroupPrefab.GetComponent<PokemonCardGroup>().pokemonCards[pokemonCardsIndex];

		//	newPokemonCard.gameObject.SetActive(true);
		//	newPokemonCard.GetComponent<PokemonCard>().name = newPokemon1.name;
		//	newPokemonCard.GetComponent<PokemonCard>().image.texture = await GetPokemonTexture(newPokemon1.sprites.front_default);
		//}
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
