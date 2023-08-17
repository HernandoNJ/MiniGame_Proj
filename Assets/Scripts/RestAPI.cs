using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using TMPro;
using System.Collections.Generic;
using System;
using Newtonsoft.Json;

public class RestAPI : MonoBehaviour
{
	public string textureUrl1;
	public string textureUrl2;
	public string textureUrl3;
	public string pokemonsApiUrl;
	public string pokemonsApiUrl2;
	public string pokemonsApiUrl3;

	public Dictionary<string, Pokemon> pokemonsList = new();
	public List<string> pokeApiUrls = new();
	public List<Result> pokemonFromApiResultsListObjs = new();

	public Pokemon pokemon;

	public PokemonFromApiRoot pokeApiObj;
	public PokemonFromApiRoot pokeApiObj2;
	public PokemonFromApiRoot pokeApiObj3;

	public PokeInfo pokeInfo1;
	public PokeInfo pokeInfo2;
	public PokeInfo pokeInfo3;

	public RawImage rawImage1;
	public RawImage rawImage2;
	public RawImage rawImage3;

	public TextMeshProUGUI pokemonName1;
	public TextMeshProUGUI pokemonName2;
	public TextMeshProUGUI pokemonName3;

	public TextMeshProUGUI pokemonExp1;
	public TextMeshProUGUI pokemonExp2;
	public TextMeshProUGUI pokemonExp3;

	public Texture2D pokemonTexture1;
	public Texture2D pokemonTexture2;
	public Texture2D pokemonTexture3;

	public GameObject pokePanelPrefab;
	public Transform canvasParent;

	private void Start()
	{
		pokeApiUrls.Add("https://pokeapi.co/api/v2/pokemon/25/");
		pokeApiUrls.Add("https://pokeapi.co/api/v2/pokemon/30/");
		pokeApiUrls.Add("https://pokeapi.co/api/v2/pokemon/35/");

		// Used to get a PokemonFromApiRoot object
		pokemonsApiUrl = "https://pokeapi.co/api/v2/pokemon/?limit=100";

		StartCoroutine(nameof(GetPokemonData));
		
	}

	private IEnumerator GetPokemonData()
	{
		using (UnityWebRequest request = UnityWebRequest.Get(pokeApiUrls[0]))
		{
			yield return request.SendWebRequest();

			if (request.result == UnityWebRequest.Result.ConnectionError)
				Debug.LogError("Request error message: " + request.error);
			else
			{
				var jsonText = request.downloadHandler.text;
				AddPokemon(jsonText);
			}
		}

		using (UnityWebRequest request = UnityWebRequest.Get(pokeApiUrls[1]))
		{
			yield return request.SendWebRequest();

			if (request.result == UnityWebRequest.Result.ConnectionError)
				Debug.LogError("Request error message: " + request.error);
			else
			{
				var jsonText = request.downloadHandler.text;
				AddPokemon(jsonText);
			}
		}

		using (UnityWebRequest request = UnityWebRequest.Get(pokeApiUrls[2]))
		{
			yield return request.SendWebRequest();

			if (request.result == UnityWebRequest.Result.ConnectionError)
				Debug.LogError("Request error message: " + request.error);
			else
			{
				var jsonText = request.downloadHandler.text;
				AddPokemon(jsonText);
			}
		}

		StartCoroutine(GetPokemonTextures());
	}

	IEnumerator GetPokemonTextures()
	{
		textureUrl1 = pokemonsList.ElementAt(0).Value.sprites.front_default;
		textureUrl2 = pokemonsList.ElementAt(1).Value.sprites.front_default;
		textureUrl3 = pokemonsList.ElementAt(2).Value.sprites.front_default;

		using (UnityWebRequest request = UnityWebRequestTexture.GetTexture(textureUrl1))
		{
			yield return request.SendWebRequest();

			if (request.result == UnityWebRequest.Result.ConnectionError)
				Debug.LogError("Request error message: " + request.error);
			else
				pokemonTexture1 = ((DownloadHandlerTexture)request.downloadHandler).texture;
		}

		using (UnityWebRequest request = UnityWebRequestTexture.GetTexture(textureUrl2))
		{
			yield return request.SendWebRequest();

			if (request.result == UnityWebRequest.Result.ConnectionError)
				Debug.LogError("Request error message: " + request.error);
			else
				pokemonTexture2 = ((DownloadHandlerTexture)request.downloadHandler).texture;
		}

		using (UnityWebRequest request = UnityWebRequestTexture.GetTexture(textureUrl3))
		{
			yield return request.SendWebRequest();

			if (request.result == UnityWebRequest.Result.ConnectionError)
				Debug.LogError("Request error message: " + request.error);
			else
				pokemonTexture3 = ((DownloadHandlerTexture)request.downloadHandler).texture;
		}

		SetPokemonValues();
		StartCoroutine(GetPokemonFromApiUrl());
	}

	IEnumerator GetPokemonFromApiUrl()
	{
		using (UnityWebRequest request = UnityWebRequest.Get(pokemonsApiUrl))
		{
			yield return request.SendWebRequest();

			// Create pokemonFromApiRootObj object from Json
			pokeApiObj = JsonUtility.FromJson<PokemonFromApiRoot>(request.downloadHandler.text);

			Debug.Log("result 10 name1: " + pokeApiObj.results[9].name);
			Debug.Log("result 10 url1: " + pokeApiObj.results[9].url);

			Debug.Log("result 90 name2: " + pokeApiObj.results[89].name);
			Debug.Log(message: $"result 90 url2: {pokeApiObj.results[89].url}");

			Debug.Log("previous1: " + pokeApiObj.previous); // null
			Debug.Log("next1: " + pokeApiObj.next);
			Debug.Log("count1: " + pokeApiObj.count);
			Debug.Log("results count1: " + pokeApiObj.results.Count);

			pokemonsApiUrl2 = pokeApiObj.next;
		}

		using (UnityWebRequest request = UnityWebRequest.Get(pokemonsApiUrl2))
		{
			yield return request.SendWebRequest();

			// Create pokemonFromApiRootObj object from Json
			pokeApiObj2 = JsonUtility.FromJson<PokemonFromApiRoot>(request.downloadHandler.text);

			// results index is pokemon - 100
			Debug.Log("result 190 name2: " + pokeApiObj2.results[89].name);
			Debug.Log("result 190 url2: " + pokeApiObj2.results[89].url);

			Debug.Log("previous2: " + pokeApiObj2.previous);
			Debug.Log("next2: " + pokeApiObj2.next);
			Debug.Log("count2: " + pokeApiObj2.count);
			Debug.Log("results count2: " + pokeApiObj2.results.Count);

			pokemonsApiUrl3 = pokeApiObj2.next;
		}

		using (UnityWebRequest request = UnityWebRequest.Get(pokemonsApiUrl3))
		{
			yield return request.SendWebRequest();

			// Create pokemonFromApiRootObj object from Json
			pokeApiObj3 = JsonUtility.FromJson<PokemonFromApiRoot>(request.downloadHandler.text);

			// results index is pokemon - 200
			Debug.Log("result 290 name2: " + pokeApiObj3.results[89].name);
			Debug.Log("result 290 url2: " + pokeApiObj3.results[89].url);

			Debug.Log("previous3: " + pokeApiObj3.previous);
			Debug.Log("next3: " + pokeApiObj3.next);
			Debug.Log("count3: " + pokeApiObj3.count);
			Debug.Log("results count3: " + pokeApiObj3.results.Count);
		}

		//StopAllCoroutines();


	}

	public void AddPokemon(string jsonTextArg)
	{
		pokemon = JsonUtility.FromJson<Pokemon>(jsonTextArg);
		pokemonsList.Add(pokemon.name, pokemon);

		Debug.Log("pokemons list count: " + pokemonsList.Count);

		//textureUrl1 = pokemonsList.ElementAt(0).Value.sprites.front_default;

		//StartCoroutine(nameof(AddPokePanels));
	}

	public void SetPokemonValues()
	{
		pokeInfo1.name = pokemonsList.ElementAt(0).Value.name;
		pokeInfo2.name = pokemonsList.ElementAt(1).Value.name;
		pokeInfo3.name = pokemonsList.ElementAt(2).Value.name;

		pokeInfo1.exp = pokemonsList.ElementAt(0).Value.base_experience.ToString();
		pokeInfo2.exp = pokemonsList.ElementAt(1).Value.base_experience.ToString();
		pokeInfo3.exp = pokemonsList.ElementAt(2).Value.base_experience.ToString();

		pokeInfo1.texture = pokemonTexture1;
		pokeInfo2.texture = pokemonTexture2;
		pokeInfo3.texture = pokemonTexture3;

		pokemonName1.text = pokeInfo1.name;
		pokemonName2.text = pokeInfo2.name;
		pokemonName3.text = pokeInfo3.name;

		pokemonExp1.text = pokeInfo1.exp;
		pokemonExp2.text = pokeInfo2.exp;
		pokemonExp3.text = pokeInfo3.exp;

		pokemonTexture1 = pokeInfo1.texture;
		pokemonTexture2 = pokeInfo2.texture;
		pokemonTexture3 = pokeInfo3.texture;

		rawImage1.texture = pokemonTexture1;
		rawImage2.texture = pokemonTexture2;
		rawImage3.texture = pokemonTexture3;
	}

	private IEnumerator AddPokePanels()
	{
		for (int i = 0; i < 100; i++)
		{
			InstantiatePokePrefabs();
			yield return new WaitForSeconds(0.05f);
		}
	}

	private void InstantiatePokePrefabs()
	{
		Instantiate(pokePanelPrefab, canvasParent);
	}

	//public void Dispose() => Destroy(texture2);// memory released, leak otherwise
}



[Serializable]
public class PokemonFromApiRoot
{
	public int count;
	public string next;
	public string previous;
	public List<Result> results;
}

[Serializable]
public class Result
{
	public string name;
	public string url;
}

[Serializable]
public class PokeInfo
{
	public string name;
	public string exp;
	public Texture2D texture;
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


