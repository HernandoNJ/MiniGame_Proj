using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using Newtonsoft.Json;
using System.Collections.Generic;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using TMPro;

namespace Pokemon.API
{
	public class RestAPI : MonoBehaviour
	{
		public string pokeUrl = "https://pokeapi.co/api/v2/pokemon/300";
		public string pokeApiV2Url = "https://pokeapi.co/api/v2/pokemon?offset=300&limit=200";
		public string pokeSpeciesUrl;
		public string pokeEvolutionChainUrl;
		public int pokeMaxCount;
		public int[] itemsCountArray;

		public GameObject pokeInfoPrefab;
		public GameObject newPokemon;
		public Texture2D newPokeTexture;

		public PokeApiObj pokeApiObjs;
		public Pokemon pokemonFromApi;
		public PokemonSpecies pokemonSpecies1;
		public EvolutionChain evolutionChain;
		public EvolutionChainRoot evolutionChainRoot;
		public ParentHandler[] parentHandlers = new ParentHandler[3];



		[SerializeField] private TMP_InputField outputArea;


		private void Start()
		{
			StartCoroutine(GetPokemonsArray());
		}

		private IEnumerator GetPokemonsArray()
		{
			outputArea.text = "Loading...";

			using UnityWebRequest request = UnityWebRequest.Get(pokeApiV2Url);
			yield return request.SendWebRequest();
			
			DisplayRequest(request);

			pokeApiObjs = JsonConvert.DeserializeObject<PokeApiObj>(request.downloadHandler.text);

			StartCoroutine(GetPokemonFromPokeApiObj());
		}

		private IEnumerator GetPokemonFromPokeApiObj()
		{
			outputArea.text = "Loading...";

			using UnityWebRequest request2 = UnityWebRequest.Get(pokeApiObjs.results[0].url);
			yield return request2.SendWebRequest();
			DisplayRequest(request2);

			pokemonFromApi = JsonConvert.DeserializeObject<Pokemon>(request2.downloadHandler.text);
			//var abc = JsonConvert.DeserializeObject(request2.downloadHandler.text);
			//Debug.Log("abc: " + abc.ToString());
		}

		private void DisplayRequest(UnityWebRequest request)
		{
			var connectionError = UnityWebRequest.Result.ConnectionError;
			var protocolError = UnityWebRequest.Result.ProtocolError;

			if (request.result == connectionError || request.result == protocolError)
				outputArea.text = "**** error in request: " + request.error;
			else outputArea.text = request.downloadHandler.text;
		}
	}
}


//using (UnityWebRequest request = UnityWebRequest.Get(pokeUrl))
//{
//	outputArea.text = "Loading...";



//	yield return request.SendWebRequest();

//	if (request.result == connectionError || request.result == protocolError)
//		outputArea.text = "**** error in request: " + request.error;
//	else outputArea.text = request.downloadHandler.text;

//	pokemonFromApi = JsonConvert.DeserializeObject<Pokemon>(request.downloadHandler.text);
//}



//using (UnityWebRequest request2 = UnityWebRequest.Get(pokeApiObjs.results[0].url))
//{
//	yield return request2.SendWebRequest();
//	Debug.Log("request2: " + request2.downloadHandler.text);

//	pokemonFromApi = JsonConvert.DeserializeObject<Pokemon>(request2.downloadHandler.text);
//}


//var requestResults = new List<Result>();

//var requestObj = new PokeApiObj 
//				{ 
//				count = 0, 
//				next = "", 
//				previous = 0, 
//				results = requestResults 
//				};
// List<string> errors = new List<string>();
// pokeApiObjs = JsonConvert.DeserializeObject<PokeApiObj>(
//	request.downloadHandler.text, 
//	new JsonSerializerSettings 
//	{
//		Error = delegate (object sender, ErrorEventArgs args)
//		{
//			errors.Add(args.ErrorContext.Error.Message);
//			args.ErrorContext.Handled = true;
//		}
//	});






//pokeApiV2Url = "https://pokeapi.co/api/v2/pokemon?offset=300&limit=200";
//pokeSpeciesUrl = "https://pokeapi.co/api/v2/pokemon-species/300";
//pokeApiV2Url = "https://pokeapi.co/api/v2/pokemon/ditto";

// TO DO: Turn into Tasks
//private IEnumerator GetDataFromApi()
//{
//	using (UnityWebRequest request = UnityWebRequest.Get(pokeApiV2Url))
//	{
//		yield return request.SendWebRequest();
//		Debug.Log("request: " + request.downloadHandler.text);
//		Debug.Break();
//		pokeApiObjs = JsonConvert.DeserializeObject<PokeApiObj>(request.downloadHandler.text);
//	}

//	for (int i = 0; i < pokeMaxCount; i++)
//	{
//		// Set Pokemon and texture (image)
//		using (UnityWebRequest request2 = UnityWebRequest.Get(pokeApiObjs.results[i].url))
//		{
//			yield return request2.SendWebRequest();
//			Debug.Log("request2: " + request2.downloadHandler.text);
//			Debug.Break();

//			pokemonFromApi = JsonConvert.DeserializeObject<Pokemon>(request2.downloadHandler.text);

//			//pokeSpeciesUrl = "https://pokeapi.co/api/v2/pokemon-species/" + pokemonFromApi.id;

//			var rawImageUrl = pokemonFromApi.sprites.front_default;
//			using UnityWebRequest rawImagerequest = UnityWebRequestTexture.GetTexture(rawImageUrl);
//			yield return rawImagerequest.SendWebRequest();

//			newPokeTexture = ((DownloadHandlerTexture)rawImagerequest.downloadHandler).texture;
//		}

//		// Get evolution chain url
//		pokeSpeciesUrl = "https://pokeapi.co/api/v2/pokemon-species/" + pokemonFromApi.id;
//		using (UnityWebRequest request3 = UnityWebRequest.Get(pokeSpeciesUrl))
//		{
//			yield return request3.SendWebRequest();
//			Debug.Log("request3: " + request3.downloadHandler.text);
//			Debug.Break();

//			pokemonSpecies1 = JsonConvert.DeserializeObject<PokemonSpecies>(request3.downloadHandler.text);
//			pokeEvolutionChainUrl = pokemonSpecies1.evolution_chain.url;

//			//using UnityWebRequest evolChainReq = UnityWebRequest.Get(pokeEvolutionChainUrl);

//		}

//		using (UnityWebRequest request4 = UnityWebRequest.Get(pokeEvolutionChainUrl))
//		{
//			yield return request4.SendWebRequest();
//			Debug.Log("request4: " + request4.downloadHandler.text);
//			Debug.Break();

//			evolutionChainRoot = JsonConvert.DeserializeObject<EvolutionChainRoot>(request4.downloadHandler.text);
//			Debug.Log("evol chain root id: " + evolutionChainRoot.id); ;
//			Debug.Log("evol chain root: " + evolutionChainRoot.chain);
//		}

//using (UnityWebRequest evolChainReq = UnityWebRequestTexture.GetTexture(pokeEvolutionChainUrl))
//{
//	yield return evolChainReq.SendWebRequest();
//	evolutionChainRoot = JsonUtility.FromJson<EvolutionChainRoot>(evolChainReq.downloadHandler.text);
//	Debug.Log("evol chain root: " + evolutionChainRoot.id);
//}



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

//		SetNewPokemonPrefab(i);
//	}

//	StopAllCoroutines();
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
