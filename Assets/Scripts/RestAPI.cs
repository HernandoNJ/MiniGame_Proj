using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;
using Cysharp.Threading.Tasks;
using System.Threading.Tasks;
using UnityEngine.UI;
using System;

namespace Pokemon.API
{
	public class RestAPI : MonoBehaviour
	{
		public string pokeUrl;
		public string pokemonResultsUrl;
		public string textureUrlPrefix = $"https://raw.githubusercontent.com/PokeAPI/sprites/master/sprites/pokemon/";
		public int pokeMaxCount;
		public int maxPanelItems;
		public int pokemonsCounter;

		public GameObject pokemonCard;
		public GameObject pokemonCardGameobject;
		public RawImage pokeRawImage;
		public Texture2D pokeTexture;
		public Pokemon pokemon;
		public PokemonCardMain pokemonCardMain;
		public PokemonResults pokemonResults;
		public ParentHandler[] parentHandlers = new ParentHandler[3];
		public PokemonSpecies[] pokemonSpeciesArray = new PokemonSpecies[3];
		public Texture2D[] speciesTextures = new Texture2D[3];
		public MainCardInfo[] mainCardItems = new MainCardInfo[3];

		private string initialPokemonResultsJson;
		private string pokemonFromApiJson;
		private string speciesUrl;

		private void OnEnable() => PokemonCardMain.OnSetPokemonEvolutions += RaiseSetPokemonEvolutions;
		private void OnDisable() => PokemonCardMain.OnSetPokemonEvolutions -= RaiseSetPokemonEvolutions;

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
			var pcSpeciesUrl = pokemon.species.url;

			var textureUrl = $"{textureUrlPrefix}{pokemon.id}.png";
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
			pc.speciesUrl = pcSpeciesUrl;
		}

		private async Task<Texture2D> GetPokemonTexture(string url)
		{
			var pokemonTextureRequest = await UnityWebRequestTexture.GetTexture(url).SendWebRequest();
			pokeTexture = ((DownloadHandlerTexture)pokemonTextureRequest.downloadHandler).texture;
			if (pokeTexture != null) return pokeTexture;
			else
			{
				Debug.Log("*** problem with poke texture");
				return null;
			}
		}

		private async UniTask<Pokemon> GetPokemonFromSpeciesUrl(string stringSuffix)
		{
			var newUrl = "https://pokeapi.co/api/v2/pokemon/" + stringSuffix;
			var json = (await UnityWebRequest.Get(newUrl).SendWebRequest()).downloadHandler.text;
			var pokemonFromSpecies = JsonConvert.DeserializeObject<Pokemon>(json);

			return pokemonFromSpecies;
		}

		private async UniTask<PokemonSpecies> GetPokemonSpecies(string url)
		{
			var json = (await UnityWebRequest.Get(url).SendWebRequest()).downloadHandler.text;
			var pokemonSpecies = JsonConvert.DeserializeObject<PokemonSpecies>(json);
			
			return pokemonSpecies;
		}

		private void RaiseSetPokemonEvolutions(string url)
		{
			speciesUrl = url;
			Invoke(nameof(SetPokemonEvolutions), 0.1f);
		}

		private async Task SetPokemonEvolutions() => await GetPokemonEvolutions(speciesUrl);

		private async Task GetPokemonEvolutions(string url)
		{
			var pokemonSpeciesJson =
				(await UnityWebRequest.Get(url).SendWebRequest()).downloadHandler.text;

			var pokemonSpecies = JsonConvert.DeserializeObject<PokemonSpecies>(pokemonSpeciesJson);

			// Get and Set Evolution chain root
			var evolutionChainJson =
				(await UnityWebRequest.Get(pokemonSpecies.evolution_chain.url).SendWebRequest()).downloadHandler.text;

			var evolutionChainRoot = JsonConvert.DeserializeObject<EvolutionChainRoot>(evolutionChainJson);

			var currentChain = evolutionChainRoot.chain;

			string url0, url1, url2;

			url0 = currentChain.species.url;
			await SetNewCardItem(url0,0);

			if (currentChain.evolves_to.Count > 0)
			{
				mainCardItems[1].gameObject.SetActive(true);
				url1 = currentChain.evolves_to[0].species.url;
				await SetNewCardItem(url1, 1);

				if (currentChain.evolves_to[0].evolves_to.Count > 0)
				{
					mainCardItems[2].gameObject.SetActive(true);
					url2 = currentChain.evolves_to[0].evolves_to[0].species.url;
					await SetNewCardItem(url2, 2);
				}
				else mainCardItems[2].gameObject.SetActive(false);
			}
			else mainCardItems[1].gameObject.SetActive(false);

			async Task SetNewCardItem(string url, int index)
			{
				var newSpecies = await GetPokemonSpecies(url);

				var textureUrl = $"{textureUrlPrefix}{newSpecies.id}.png";
				var newTexture = await GetPokemonTexture(textureUrl);

				mainCardItems[index].nameText.text = newSpecies.name;
				mainCardItems[index].idText.text = newSpecies.id.ToString();
				mainCardItems[index].image.texture = newTexture;
			}
		}
	}
}