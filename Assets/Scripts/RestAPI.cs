using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using System.Collections.Generic;
using System;

public class RestAPI : MonoBehaviour
{
	public string pokeApiUrl1;
	public string pokeApiUrl2;
	public string pokeApiUrl3;

	public string textureUrl1;
	public string textureUrl2;
	public string textureUrl3;

	public Pokemon pokemon1;
	public Pokemon pokemon2;
	public Pokemon pokemon3;

	public TextMeshProUGUI pokemonName1;
	public TextMeshProUGUI pokemonName2;
	public TextMeshProUGUI pokemonName3;

	public TextMeshProUGUI pokemonExp1;
	public TextMeshProUGUI pokemonExp2;
	public TextMeshProUGUI pokemonExp3;

	public Texture2D texture2DFromUrl1;
	public Texture2D texture2DFromUrl2;
	public Texture2D texture2DFromUrl3;

	private void Start()
	{
		pokeApiUrl1 = "https://pokeapi.co/api/v2/pokemon/25/";
		pokeApiUrl2 = "https://pokeapi.co/api/v2/pokemon/30/";
		pokeApiUrl3 = "https://pokeapi.co/api/v2/pokemon/35/";

		StartCoroutine(nameof(GetData));
	}

	private IEnumerator GetData()
	{
		using (UnityWebRequest request = UnityWebRequest.Get(pokeApiUrl1))
		{
			yield return request.SendWebRequest();

			if (request.result == UnityWebRequest.Result.ConnectionError) Debug.LogError("Request error message: " + request.error);
			else
			{
				var jsonText = request.downloadHandler.text;
				pokemon1 = JsonUtility.FromJson<Pokemon>(jsonText);
			}
		}

		using (UnityWebRequest request = UnityWebRequest.Get(pokeApiUrl2))
		{
			yield return request.SendWebRequest();

			if (request.result == UnityWebRequest.Result.ConnectionError) Debug.LogError("Request error message: " + request.error);
			else
			{
				var jsonText = request.downloadHandler.text;
				pokemon2 = JsonUtility.FromJson<Pokemon>(jsonText);
			}
		}

		using (UnityWebRequest request = UnityWebRequest.Get(pokeApiUrl3))
		{
			yield return request.SendWebRequest();

			if (request.result == UnityWebRequest.Result.ConnectionError) Debug.LogError("Request error message: " + request.error);
			else
			{
				var jsonText = request.downloadHandler.text;
				pokemon3 = JsonUtility.FromJson<Pokemon>(jsonText);
			}
		}

		CheckPokemons();

		// Dispose();
	}

	public void CheckPokemons()
	{
		pokemonName1.text = pokemon1.name;
		pokemonName2.text = pokemon2.name;
		pokemonName3.text = pokemon3.name;

		pokemonExp1.text = pokemon1.base_experience.ToString();
		pokemonExp2.text = pokemon2.base_experience.ToString();
		pokemonExp3.text = pokemon3.base_experience.ToString();

		StopAllCoroutines();
	}

	//public void Dispose() => Destroy(texture2);// memory released, leak otherwise
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

