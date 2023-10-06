using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using Newtonsoft.Json;

namespace Pokemon.API
{
	public class RestAPI : MonoBehaviour
	{
		public string pokeApiV2Url;

		public GameObject pokeInfoPrefab;
		public GameObject newPokemon;
		public Texture2D newPokeTexture;

		public PokeApiObj pokeApiObjs;
		public Pokemon pokemonFromApi;
		public Species pokemonSpecies1;
		public EvolutionChain evolutionChain = new();
		public ParentHandler[] parentHandlers = new ParentHandler[3];

		/*	Del Pokemon saco la url
			Crear url de pokemon-species
			Conseguir evolution chain
			Si tiene, conseguir las url de las evoluciones
			con esas url, conseguir los sprites 

			Mejorar la logica del ciclo for */

		private void Start()
		{
			pokeApiV2Url = "https://pokeapi.co/api/v2/pokemon?offset=300&limit=300";
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

			for (int i = 0; i < 300; i++)
			{
				using (UnityWebRequest request = UnityWebRequest.Get(pokeApiObjs.results[i].url))
				{
					yield return request.SendWebRequest();

					pokemonFromApi = JsonUtility.FromJson<Pokemon>(request.downloadHandler.text);
					
					var rawImageUrl = pokemonFromApi.sprites.front_default;
					pokemonSpecies1 = pokemonFromApi.species;

					using UnityWebRequest rawImagerequest = UnityWebRequestTexture.GetTexture(rawImageUrl);
					yield return rawImagerequest.SendWebRequest();

					newPokeTexture = ((DownloadHandlerTexture)rawImagerequest.downloadHandler).texture;
				}

				SetNewPokemonPrefab(i);
			}

			StopAllCoroutines();
		}

		private void SetNewPokemonPrefab(int i)
		{
			//var newPokemon = Instantiate(pokeInfoPrefab, parentHandler1.transform);
			newPokemon = Instantiate(pokeInfoPrefab);

			newPokemon.GetComponent<PokemonInfo>().nameText.text = pokemonFromApi.name;
			newPokemon.GetComponent<PokemonInfo>().exp.text = pokemonFromApi.base_experience.ToString();
			newPokemon.GetComponent<PokemonInfo>().image.texture = newPokeTexture;

			if (i == 0) newPokemon.GetComponent<PokemonInfo>().SetPokemonCardData();

			if (i < 100) SetParentHandler(0);
			else if (i < 200) SetParentHandler(1); 
			else if (i < 300) SetParentHandler(2);

			void SetParentHandler(int index)
			{
				newPokemon.transform.SetParent(parentHandlers[index].transform);
				parentHandlers[index].AddPokemonToList(newPokemon);
			}
		}
	}
}





//}

//for (int i = 101; i < 200; i++)
//{
//	using (UnityWebRequest request = UnityWebRequest.Get(pokeApiObjs.results[i].url))
//	{
//		yield return request.SendWebRequest();

//		pokemonFromApi = JsonUtility.FromJson<Pokemon>(request.downloadHandler.text);
//		var rawImageUrl = pokemonFromApi.sprites.front_default;

//		using (UnityWebRequest rawImagerequest = UnityWebRequestTexture.GetTexture(rawImageUrl))
//		{
//			yield return rawImagerequest.SendWebRequest();

//			newPokeTexture = ((DownloadHandlerTexture)rawImagerequest.downloadHandler).texture;
//		}
//	}

//	//var newPokemon = Instantiate(pokePanelPrefab, canvasParentsHandler.pokePanelsList[0].transform);

//	var newPokemon = Instantiate(pokeInfoPrefab, parentHandler2.transform);

//	pokeInfoPrefab.GetComponent<PokemonInfo>().nameText.text = pokemonFromApi.name;
//	pokeInfoPrefab.GetComponent<PokemonInfo>().exp.text = pokemonFromApi.base_experience.ToString();
//	pokeInfoPrefab.GetComponent<PokemonInfo>().image.texture = newPokeTexture;

//	parentHandler2.AddPokemonToList(newPokemon);
//	ShowI(i);
//}

//for (int i = 201; i < 300; i++)
//{
//	using (UnityWebRequest request = UnityWebRequest.Get(pokeApiObjs.results[i].url))
//	{
//		yield return request.SendWebRequest();

//		pokemonFromApi = JsonUtility.FromJson<Pokemon>(request.downloadHandler.text);
//		var rawImageUrl = pokemonFromApi.sprites.front_default;

//		using (UnityWebRequest rawImagerequest = UnityWebRequestTexture.GetTexture(rawImageUrl))
//		{
//			yield return rawImagerequest.SendWebRequest();

//			newPokeTexture = ((DownloadHandlerTexture)rawImagerequest.downloadHandler).texture;
//		}
//	}

//	//var newPokemon = Instantiate(pokePanelPrefab, canvasParentsHandler.pokePanelsList[0].transform);

//	var newPokemon = Instantiate(pokeInfoPrefab, parentHandler3.transform);

//	pokeInfoPrefab.GetComponent<PokemonInfo>().nameText.text = pokemonFromApi.name;
//	pokeInfoPrefab.GetComponent<PokemonInfo>().exp.text = pokemonFromApi.base_experience.ToString();
//	pokeInfoPrefab.GetComponent<PokemonInfo>().image.texture = newPokeTexture;

//	parentHandler3.AddPokemonToList(newPokemon);
//}


/*
	if (request.result == UnityWebRequest.Result.Success)
	{
		// request.downloadHandler.text converts the request into text (Json)
		// DeserializeObject<Fact> converts the text into a <Type> object
		pokeApiObjs = JsonConvert.DeserializeObject<PokeApiObj>(request.downloadHandler.text);
	}
*/


/*
public class RestAPI : MonoBehaviour
{
	public string pokeApiV2Url;

	public GameObject pokeInfoPrefab;
	public Texture2D newPokeTexture;

	public PokeApiObj pokeApiObjs;
	public Pokemon pokemonFromApi;
	public Species pokemonSpecies1;
	public EvolutionChain evolutionChain = new();

	public ParentHandler parentHandler1;
	public ParentHandler parentHandler2;
	public ParentHandler parentHandler3;

		Del Pokemon saco la url
		Crear url de pokemon-species
		Conseguir evolution chain
		Si tiene, conseguir las url de las evoluciones
		con esas url, conseguir los sprites 

		Mejorar la logica del ciclo for 


	private void Start()
	{
		//pokeApiV2Url = "https://pokeapi.co/api/v2/pokemon?offset=300&limit=300";
		pokeApiV2Url = "https://pokeapi.co/api/v2/pokemon/ditto";

		StartCoroutine(GetDataFromApi());
	}

	private IEnumerator GetDataFromApi()
	{
		using (UnityWebRequest request = UnityWebRequest.Get(pokeApiV2Url))
		{
			yield return request.SendWebRequest();

			if (request.result == UnityWebRequest.Result.Success)
			{
				// request.downloadHandler.text converts the request into text (Json)
				// DeserializeObject<Fact> converts the text into a <Type> object
				pokeApiObjs = JsonConvert.DeserializeObject<PokeApiObj>(request.downloadHandler.text);
			}
			else Debug.LogError("Error when trying to ge info from web request");
		}

		// pokeapi.co/api/v2/pokemon/600/
		// offset + 1 + results[299] 
		//Debug.Log("result 300 url: " + pokeApiObjs.results[299].url);


		//change 3 for 100
		//for (int i = 0; i < 100; i++)
		//	{
		//using (UnityWebRequest request = UnityWebRequest.Get(pokeApiObjs.results[i].url))
		using (UnityWebRequest request = UnityWebRequest.Get(pokeApiV2Url))
		{
			yield return request.SendWebRequest();

			pokemonFromApi = JsonUtility.FromJson<Pokemon>(request.downloadHandler.text);
			Debug.Log("calling request " + pokemonFromApi.ToString());
			var rawImageUrl = pokemonFromApi.sprites.front_default;
			pokemonSpecies1 = pokemonFromApi.species;

			using (UnityWebRequest rawImagerequest = UnityWebRequestTexture.GetTexture(rawImageUrl))
			{
				yield return rawImagerequest.SendWebRequest();

				newPokeTexture = ((DownloadHandlerTexture)rawImagerequest.downloadHandler).texture;
			}
		}

		var newPokemon = Instantiate(pokePanelPrefab, canvasParentsHandler.pokePanelsList[0].transform);

		var newPokemon = Instantiate(pokeInfoPrefab, parentHandler1.transform);

		newPokemon.GetComponent<PokemonInfo>().nameText.text = pokemonFromApi.name;
		newPokemon.GetComponent<PokemonInfo>().exp.text = pokemonFromApi.base_experience.ToString();
		newPokemon.GetComponent<PokemonInfo>().image.texture = newPokeTexture;

		species
		Create Pokemon Species
		 Create Evolution Chain

		if (pokemonFromApi.species == null)
		{
			Debug.Log("Pokemon species is null. Continue");
		}
		else
		{
			*************
		   evolutionChain = pokemonSpecies.evolution_chain; // Error NullReferenceException: Object reference not set to an instance of an object

			Debug.Log("pokemonSpecies: " + pokemonSpecies); // ok, but empty
			Debug.Log("capture rate: " + pokemonSpecies.Capture_rate); // Error NullReferenceException: Object reference not set to an instance of an object

			Debug.Log("evolution chain: " + evolutionChain);
		}

		parentHandler1.AddPokemonToList(newPokemon);

		if (i == 0) newPokemon.GetComponent<PokemonInfo>().SetPokemonCardData();

		ShowI(i);


	}

		for (int i = 101; i< 200; i++)
		{
			using (UnityWebRequest request = UnityWebRequest.Get(pokeApiObjs.results[i].url))
			{
				yield return request.SendWebRequest();

				pokemonFromApi = JsonUtility.FromJson<Pokemon>(request.downloadHandler.text);
				var rawImageUrl = pokemonFromApi.sprites.front_default;

				using (UnityWebRequest rawImagerequest = UnityWebRequestTexture.GetTexture(rawImageUrl))
{
	yield return rawImagerequest.SendWebRequest();

	newPokeTexture = ((DownloadHandlerTexture)rawImagerequest.downloadHandler).texture;
}
			}

			//var newPokemon = Instantiate(pokePanelPrefab, canvasParentsHandler.pokePanelsList[0].transform);

			var newPokemon = Instantiate(pokeInfoPrefab, parentHandler2.transform);

pokeInfoPrefab.GetComponent<PokemonInfo>().nameText.text = pokemonFromApi.name;
pokeInfoPrefab.GetComponent<PokemonInfo>().exp.text = pokemonFromApi.base_experience.ToString();
pokeInfoPrefab.GetComponent<PokemonInfo>().image.texture = newPokeTexture;

parentHandler2.AddPokemonToList(newPokemon);
ShowI(i);
		}

		for (int i = 201; i < 300; i++)
{
	using (UnityWebRequest request = UnityWebRequest.Get(pokeApiObjs.results[i].url))
	{
		yield return request.SendWebRequest();

		pokemonFromApi = JsonUtility.FromJson<Pokemon>(request.downloadHandler.text);
		var rawImageUrl = pokemonFromApi.sprites.front_default;

		using (UnityWebRequest rawImagerequest = UnityWebRequestTexture.GetTexture(rawImageUrl))
		{
			yield return rawImagerequest.SendWebRequest();

			newPokeTexture = ((DownloadHandlerTexture)rawImagerequest.downloadHandler).texture;
		}
	}

	//var newPokemon = Instantiate(pokePanelPrefab, canvasParentsHandler.pokePanelsList[0].transform);

	var newPokemon = Instantiate(pokeInfoPrefab, parentHandler3.transform);

	pokeInfoPrefab.GetComponent<PokemonInfo>().nameText.text = pokemonFromApi.name;
	pokeInfoPrefab.GetComponent<PokemonInfo>().exp.text = pokemonFromApi.base_experience.ToString();
	pokeInfoPrefab.GetComponent<PokemonInfo>().image.texture = newPokeTexture;

	parentHandler3.AddPokemonToList(newPokemon);
}

StopAllCoroutines();
	}
}
}
*/