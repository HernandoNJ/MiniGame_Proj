using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine.UI;

namespace Pokemon.API
{
	public class RestAPI : MonoBehaviour
	{
		public string pokeUrl;
		public string pokemonResultsUrl;
		public int pokeMaxCount;
		public int maxPanelItems;
		public int pokemonsCounter;
		public int pokeChainsCount;
		public int itemsCountForTest;
		public bool isCardInit;

		public GameObject pokemonCard;
		public GameObject pokemonCardGameobject;
		public RawImage pokeRawImage;
		public Texture2D pokeTexture;
		public Pokemon pokemon;
		public PokemonCardMain2 pokemonCardMain2;
		public PokemonResults pokemonResults;
		public PokemonSpecies pokemonSpecies;
		public ParentHandler[] parentHandlers = new ParentHandler[3];
		public List<PokemonCard> pokemonCardsList;
		public Pokemon[] tempPokemons = new Pokemon[3];
		public Chain currentChain;
		public EvolutionChainRoot evolutionChainRoot;
		public List<int> evolutionChainIds;

		private string initialPokemonResultsJson;
		private string pokemonFromApiJson;
		private PokemonCard[] evolutionItems;

		//private string pcName;
		//private string pcExp;
		//private string textureUrl;
		//private int parentIndex;
		//private Texture2D pcTexture;

		private void Start()
		{
			pokeUrl = "https://pokeapi.co/api/v2/pokemon/300";
			pokemonResultsUrl = "https://pokeapi.co/api/v2/pokemon?offset=0&limit=300";

			Invoke(nameof(GetPokemonsData), 0.1f);
		}

		private async UniTask GetPokemonsData()
		{
			await GetPokemonResults();
		}

		private async UniTask GetPokemonResults()
		{
			initialPokemonResultsJson =
				(await UnityWebRequest.Get(pokemonResultsUrl).SendWebRequest()).downloadHandler.text;

			pokemonResults = JsonConvert.DeserializeObject<PokemonResults>(initialPokemonResultsJson);

			for (int i = 0; i < pokeMaxCount; i++)
				await GetPokemonFromAPI(pokemonResults.results[i].url);
		}

		private async UniTask GetPokemonFromAPI(string url)
		{
			pokemonFromApiJson =
				(await UnityWebRequest.Get(url).SendWebRequest()).downloadHandler.text;

			pokemon = JsonConvert.DeserializeObject<Pokemon>(pokemonFromApiJson);

			pokemonsCounter++;
			var parentIndex = 0;

			var pcName = pokemon.name;
			var pcExp = pokemon.base_experience.ToString();

			var textureUrl = $"https://raw.githubusercontent.com/PokeAPI/sprites/master/sprites/pokemon/{pokemon.id}.png";
			var pcTexture = await GetPokemonTexture(textureUrl);

			if (pokemonsCounter < maxPanelItems) parentIndex = 0;
			else if (pokemonsCounter < (maxPanelItems * 2)) parentIndex = 1;
			else if (pokemonsCounter < (maxPanelItems * 3)) parentIndex = 2;
			else Debug.Log("*** Check out if poke chains count and max panel items");

			pokemonCardGameobject = Instantiate(pokemonCard, parentHandlers[parentIndex].transform);
			parentHandlers[parentIndex].AddPokemonToList(pokemonCardGameobject);

			PokemonCard pc = pokemonCard.GetComponent<PokemonCard>();
			pc.nameText.text = pcName;
			pc.exp.text = pcExp;
			pc.image.texture = pcTexture;

			//if (pokemon != null) await InstantiatePokemonCard(pokemon);

			//if(!string.IsNullOrEmpty(pokemon.species.url)) 
			//	await GetPokemonEvolutions(pokemon.species.url);
		}

		private async Task<Texture2D> GetPokemonTexture(string url)
		{
			var pokemonTextureRequest = await UnityWebRequestTexture.GetTexture(url).SendWebRequest();
			return pokeTexture = ((DownloadHandlerTexture)pokemonTextureRequest.downloadHandler).texture;
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
			if (evolutionChainIds.Contains(evolutionChainRoot.id)) return;

			evolutionChainIds.Add(evolutionChainRoot.id);
			pokeChainsCount++;
			Debug.Log("evolution chain id: " + evolutionChainRoot.id);
			Debug.Log("chains count: " + pokeChainsCount);
		}



		//private async UniTask InstantiatePokemonCard(Pokemon pokemon)
		//{
		//	pokemonsCounter++;
		//	parentIndex = 0;

		//	pcName = pokemon.name;
		//	pcExp = pokemon.base_experience.ToString();

		//	textureUrl = pokemon.sprites.front_default;
		//	pcTexture = await GetPokemonTexture(textureUrl);

		//	if (pokemonsCounter < maxPanelItems) parentIndex = 0;
		//	else if (pokemonsCounter < (maxPanelItems * 2)) parentIndex = 1;
		//	else if (pokemonsCounter < (maxPanelItems * 3)) parentIndex = 2;
		//	else Debug.Log("*** Check out if poke chains count and max panel items");

		//	pokemonCardGameobject = Instantiate(pokemonCard, parentHandlers[parentIndex].transform);
		//	parentHandlers[parentIndex].AddPokemonToList(pokemonCardGameobject);

		//	PokemonCard pc = pokemonCard.GetComponent<PokemonCard>();
		//	pc.nameText.text = pcName;
		//	pc.exp.text = pcExp;
		//	pc.image.texture = pcTexture;
		//}

		//private void InstantiatePokemonCards()
		//{

		//	foreach (var item in parentHandlers)
		//	{
		//		if (item == null) Debug.LogError("*** Parent handler is null ");
		//	}

		//	if (pokeChainsCount < maxPanelItems)
		//	{
		//		pokemonCardGameobject = Instantiate(pokemonCard, parentHandlers[0].transform);
		//		parentHandlers[0].AddPokemonToList(pokemonCardGameobject);
		//	}
		//	else if (pokeChainsCount < (maxPanelItems * 2))
		//	{
		//		pokemonCardGameobject = Instantiate(pokemonCard, parentHandlers[1].transform);
		//		parentHandlers[1].AddPokemonToList(pokemonCardGameobject);
		//	}
		//	else if (pokeChainsCount < (maxPanelItems * 3))
		//	{
		//		pokemonCardGameobject = Instantiate(pokemonCard, parentHandlers[2].transform);
		//		parentHandlers[2].AddPokemonToList(pokemonCardGameobject);
		//	}
		//	else Debug.Log("*** Check out if poke chains count and max panel items");


		//}

		//currentChain = evolutionChainRoot.chain;
		//int chainItemsCounter = 0;
		//string url0, url1, url2;

		//url0 = currentChain.species.name;
		//await GetPokemonEvolutionObjects(url0, 0);


		//if (currentChain.evolves_to.Count > 0)
		//{
		//	url1 = currentChain.evolves_to[0].species.name;
		//	await GetPokemonEvolutionObjects(url1, 1);

		//	if (currentChain.evolves_to[0].evolves_to.Count > 0)
		//	{
		//		url2 = currentChain.evolves_to[0].evolves_to[0].species.name;
		//		await GetPokemonEvolutionObjects(url2, 2);
		//	}
		//}

		//await InstantiatePokemonEvolutions();

		//async Task InstantiatePokemonEvolutions()
		//{
		//	foreach (var item in parentHandlers)
		//	{
		//		if (item == null) Debug.LogError("*** Parent handler is null ");
		//	}

		//	if (pokeChainsCount < maxPanelItems)
		//	{
		//		pokemonCardGameobject = Instantiate(pokemonCard, parentHandlers[0].transform);
		//		parentHandlers[0].AddPokemonToList(pokemonCardGameobject);
		//	}
		//	else if (pokeChainsCount < (maxPanelItems * 2))
		//	{
		//		pokemonCardGameobject = Instantiate(pokemonCard, parentHandlers[1].transform);
		//		parentHandlers[1].AddPokemonToList(pokemonCardGameobject);
		//	}
		//	else if (pokeChainsCount < (maxPanelItems * 3))
		//	{
		//		pokemonCardGameobject = Instantiate(pokemonCard, parentHandlers[2].transform);
		//		parentHandlers[2].AddPokemonToList(pokemonCardGameobject);
		//	}
		//	else Debug.Log("*** Check out if poke chains count and max panel items");

		//	await SetPokemonEvolutionsItems();
		//}

		//async Task SetPokemonEvolutionsItems()
		//{
		//	evolutionItems = pokemonCardGameobject.GetComponent<PokemonEvolutions>().GetPokemonCardItems();
		//	for (int i = 0; i < chainItemsCounter; i++)
		//	{
		//		evolutionItems[i].gameObject.SetActive(true);
		//		evolutionItems[i].GetComponent<PokemonCard>().nameText.text = tempPokemons[i].name;
		//		evolutionItems[i].GetComponent<PokemonCard>().exp.text = tempPokemons[i].base_experience.ToString();

		//		var textureUrl = tempPokemons[i].sprites.front_default;
		//		evolutionItems[i].GetComponent<PokemonCard>().image.texture = await GetPokemonTexture(textureUrl);

		//		if (isCardInit == false)
		//		{
		//			isCardInit = true;

		//			// Set PokemonMainCard data for the first time
		//			pokeRawImage.texture = await GetPokemonTexture(pokemon.sprites.front_default);

		//			pokemonCardMain2.SetPokemonCardMainData(
		//				pokemon.name,
		//				pokemon.base_experience.ToString(), 
		//				pokeRawImage);
		//		}

		//	}
		//}

		//// Create Pokemon evolution objects
		//async Task GetPokemonEvolutionObjects(string url, int index)
		//{
		//	chainItemsCounter++;
		//	var newPokemon = await GetPokemonFromSpeciesUrl(url);

		//	if (pokemon != null)
		//	{
		//		tempPokemons[index] = newPokemon;
		//		Debug.Log("pokemon name: " + newPokemon.name + ", index: " + index);
		//	}
		//}





	}
}