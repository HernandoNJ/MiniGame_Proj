using UnityEngine;
using Newtonsoft.Json;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using System;

public class RestAPI : MonoBehaviour
{
	public string pokeApiV2Url;

	public GameObject pokeInfoPrefab;
	
	public Texture2D newPokeTexture;

	public PokeApiObj pokeApiObjs;
	public Pokemon pokemonFromApi;

	public ParentHandler parentHandler1;
	public ParentHandler parentHandler2;
	public ParentHandler parentHandler3;

	private void Start()
	{
		pokeApiV2Url = "https://pokeapi.co/api/v2/pokemon?offset=300&limit=300";

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
		Debug.Log("result 300 url: " + pokeApiObjs.results[299].url); 
		
		for (int i = 0; i < 100; i++)
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

			var newPokemon = Instantiate(pokeInfoPrefab, parentHandler1.transform);

			newPokemon.GetComponent<PokemonInfo>().nameText.text = pokemonFromApi.name;
			newPokemon.GetComponent<PokemonInfo>().exp.text = pokemonFromApi.base_experience.ToString();
			newPokemon.GetComponent<PokemonInfo>().image.texture = newPokeTexture;

			parentHandler1.AddPokemonToList(newPokemon);

			if (i == 0) newPokemon.GetComponent<PokemonInfo>().SetPokemonCardData();

		}

		for (int i = 101; i < 200; i++)
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

	public class PokeApiObj
	{
		public int count { get; set; }
		public string next { get; set; }
		public object previous { get; set; }
		public List<Result> results { get; set; }
	}

	[Serializable]
	public class PokeInfo2
	{
		public string name;
		public string exp;
		public Texture2D texture;
	}

	public class Result
	{
		public string name { get; set; }
		public string url { get; set; }
	}

	[Serializable]
	public class Pokemon
	{
		public int id;
		public string name;
		public int base_experience;
		public int height;
		public bool is_default;
		public int order;
		public int weight;
		public List<PokemonAbility> abilities;
		public List<NamedAPIResource> forms;
		public List<VersionGameIndex> game_indices;
		public List<PokemonHeldItem> held_items;
		public string location_area_encounters;
		public List<PokemonMove> moves;
		public List<PokemonTypePast> past_types;
		public PokemonSprites sprites;
		public NamedAPIResource species;
		public List<PokemonStat> stats;
		public List<PokemonType> types;
	}

	[Serializable]
	public class PokemonStat
	{
		public NamedAPIResource stat;
		public int effort;
		public int base_stat;
	}

	[Serializable]
	public class PokemonSprites
	{
		public string front_default;
		public string front_shiny;
		public string front_female;
		public string front_shiny_female;
		public string back_default;
		public string back_shiny;
		public string back_female;
		public string back_shiny_female;
	}

	[Serializable]
	public class PokemonType
	{
		public int slot;
		public NamedAPIResource type;
	}

	[Serializable]
	public class PokemonTypePast
	{
		public NamedAPIResource generation;
		public List<PokemonType> types;
	}

	[Serializable]
	public class PokemonMove
	{
		public NamedAPIResource move;
		public PokemonMoveVersion version_group_details;
	}

	[Serializable]
	public class PokemonMoveVersion
	{
		public NamedAPIResource move_learn_method;
		public NamedAPIResource version_group;
		public int level_learned_at;
	}

	[Serializable]
	public class PokemonAbility
	{
		public bool is_hidden;
		public int slot;
		public NamedAPIResource ability;
	}

	[Serializable]
	public class VersionGameIndex
	{
		public int game_index;
		public NamedAPIResource version;
	}

	[Serializable]
	public class PokemonHeldItem
	{
		public NamedAPIResource item;
		public PokemonHeldItemVersion version_details;
	}

	[Serializable]
	public class PokemonHeldItemVersion
	{
		public NamedAPIResource version;
		public int rarity;
	}

	[Serializable]
	public class NamedAPIResource
	{
		public string name;
		public string url;
	}

	[Serializable]
	public class PokemonFormType
	{
		public int slot;
	}
}
