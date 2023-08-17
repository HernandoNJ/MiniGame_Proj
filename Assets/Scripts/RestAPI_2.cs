using UnityEngine;
using Newtonsoft.Json;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using System;

public class RestAPI_2 : MonoBehaviour
{
	public string pokeApiV2Url = "https://pokeapi.co/api/v2/pokemon/?limit=100";

	public GameObject pokePanelPrefab;
	public PokeApiObj pokeApiObjs;
	public Transform canvasParent;

	private void Start()
	{
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
				// DeserializeObject<Fact> converts the text into a Fact object
				pokeApiObjs = JsonConvert.DeserializeObject<PokeApiObj>(request.downloadHandler.text);
			}
			else Debug.LogError("Error when trying to ge info from web request");
		}

		for (int i = 0; i < pokeApiObjs.results.Count; i++)
		{
			using (UnityWebRequest request = UnityWebRequest.Get(pokeApiObjs.results[i].url))
			{
				yield return request.SendWebRequest();

				Pokemon pokemonFromApi = JsonUtility.FromJson<Pokemon>(request.downloadHandler.text);
				var rawImageUrl = pokemonFromApi.sprites.front_default;
				Texture2D newPokeTexture;

				using (UnityWebRequest rawImagerequest = UnityWebRequestTexture.GetTexture(rawImageUrl))
				{
					yield return rawImagerequest.SendWebRequest();

					newPokeTexture = ((DownloadHandlerTexture)rawImagerequest.downloadHandler).texture;
				}

				Instantiate(pokePanelPrefab, canvasParent);

				pokePanelPrefab.GetComponent<PokePanel>().pokePanelName.text = pokemonFromApi.name;
				pokePanelPrefab.GetComponent<PokePanel>().pokePanelExp.text = pokemonFromApi.base_experience.ToString();
				pokePanelPrefab.GetComponent<PokePanel>().pokePanelImage.texture = newPokeTexture; 
			}
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
