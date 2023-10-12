using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using Newtonsoft.Json;

namespace Pokemon.API
{
	public class RestAPI : MonoBehaviour
	{
		public string pokeApiV2Url;
		public string pokeSpeciesUrl;
		public string pokeEvolutionChainUrl;
		public int pokeMaxCount;
		public int[] itemsCountArray;

		public GameObject pokeInfoPrefab;
		public GameObject newPokemon;
		public Texture2D newPokeTexture;

		public PokeApiObj pokeApiObjs;
		public Pokemon pokemonFromApi;
		public PokemonSpecies pokemonSpecies1 = new();
		public EvolutionChain evolutionChain = new();
		public ParentHandler[] parentHandlers = new ParentHandler[3];

		private void Start()
		{
			pokeApiV2Url = "https://pokeapi.co/api/v2/pokemon?offset=300&limit=300";
			pokeSpeciesUrl = "https://pokeapi.co/api/v2/pokemon-species/300";
			//pokeApiV2Url = "https://pokeapi.co/api/v2/pokemon/ditto";

			StartCoroutine(GetDataFromApi());
		}

		// TO DO: Turn into Tasks
		private IEnumerator GetDataFromApi()
		{
			using (UnityWebRequest request = UnityWebRequest.Get(pokeApiV2Url))
			{
				yield return request.SendWebRequest();
				pokeApiObjs = JsonConvert.DeserializeObject<PokeApiObj>(request.downloadHandler.text);
			}

			for (int i = 0; i < pokeMaxCount; i++)
			{
				// Set Pokemon and texture (image)
				using (UnityWebRequest request = UnityWebRequest.Get(pokeApiObjs.results[i].url))
				{
					yield return request.SendWebRequest();

					pokemonFromApi = JsonUtility.FromJson<Pokemon>(request.downloadHandler.text);

					var rawImageUrl = pokemonFromApi.sprites.front_default;
					using UnityWebRequest rawImagerequest = UnityWebRequestTexture.GetTexture(rawImageUrl);
					yield return rawImagerequest.SendWebRequest();

					newPokeTexture = ((DownloadHandlerTexture)rawImagerequest.downloadHandler).texture;
				}

				// Get evolution chain url
				pokeSpeciesUrl = "https://pokeapi.co/api/v2/pokemon-species/" + pokemonFromApi.id;
				using (UnityWebRequest request1 = UnityWebRequest.Get(pokeSpeciesUrl))
				{
					yield return request1.SendWebRequest();
					pokemonSpecies1 = JsonConvert.DeserializeObject<PokemonSpecies>(request1.downloadHandler.text);
					pokeEvolutionChainUrl = pokemonSpecies1.evolution_chain.url;
				}


				//using (UnityWebRequest request2 = UnityWebRequest.Get(pokeEvolutionChainUrl))
				//{
				//	yield return request2.SendWebRequest();
				//	pokemonSpecies1 = JsonConvert.DeserializeObject<PokemonSpecies>(request2.downloadHandler.text);
				//	pokeEvolutionChainUrl = pokemonSpecies1.evolution_chain.url;
				//}


				/*	Del evolution chain consigo las url de pokemon species y el orden de las evoluciones
				 *	Obtener sprites con url arregladas de pokemon species
				 *	Ordenarlos segun evolucion
				 *	Acomodar los 3 sprites en cada espacio de UI
				 *	*/

				SetNewPokemonPrefab(i);
			}

			StopAllCoroutines();
		}

		private void SetNewPokemonPrefab(int i)
		{
			newPokemon = Instantiate(pokeInfoPrefab);

			newPokemon.GetComponent<PokemonInfo>().nameText.text = pokemonFromApi.name;
			newPokemon.GetComponent<PokemonInfo>().exp.text = pokemonFromApi.base_experience.ToString();
			newPokemon.GetComponent<PokemonInfo>().image.texture = newPokeTexture;

			if (i == 0) newPokemon.GetComponent<PokemonInfo>().SetPokemonCardData();

			if (i < itemsCountArray[0]) SetParentHandler(0);
			else if (i < itemsCountArray[1]) SetParentHandler(1);
			else if (i < itemsCountArray[2]) SetParentHandler(2);

			void SetParentHandler(int index)
			{
				newPokemon.transform.SetParent(parentHandlers[index].transform);
				parentHandlers[index].AddPokemonToList(newPokemon);
			}
		}
	}
}