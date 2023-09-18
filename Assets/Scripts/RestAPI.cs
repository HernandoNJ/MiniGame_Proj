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
	public PokemonSpecies pokemonSpecies = new();
	public EvolutionChain evolutionChain = new();

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

		for (int i = 0; i < 3; i++)
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

			// species
			// Create Pokemon Species
			// Create Evolution Chain
			pokemonSpecies = pokemonFromApi.species;
			
			// *************
			// evolutionChain = pokemonSpecies.evolution_chain; // Error NullReferenceException: Object reference not set to an instance of an object


			parentHandler1.AddPokemonToList(newPokemon);

			if (i == 0) newPokemon.GetComponent<PokemonInfo>().SetPokemonCardData();

		}

		Debug.Log("pokemonSpecies: " + pokemonSpecies); // ok, but empty
		Debug.Log("capture rate: " + pokemonSpecies.capture_rate); // Error NullReferenceException: Object reference not set to an instance of an object

		Debug.Log("evolution chain: " + evolutionChain);

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
		public PokemonSpecies species;
		public List<PokemonStat> stats;
		public List<PokemonType> types;
	}

	public class Chain
	{
		public List<object> evolution_details { get; set; }
		public List<EvolvesTo> evolves_to { get; set; }
		public bool is_baby { get; set; }
		public Species species { get; set; }
	}

	public class EvolutionDetail
	{
		public object gender { get; set; }
		public object held_item { get; set; }
		public object item { get; set; }
		public object known_move { get; set; }
		public object known_move_type { get; set; }
		public object location { get; set; }
		public object min_affection { get; set; }
		public object min_beauty { get; set; }
		public object min_happiness { get; set; }
		public int min_level { get; set; }
		public bool needs_overworld_rain { get; set; }
		public object party_species { get; set; }
		public object party_type { get; set; }
		public object relative_physical_stats { get; set; }
		public string time_of_day { get; set; }
		public object trade_species { get; set; }
		public Trigger trigger { get; set; }
		public bool turn_upside_down { get; set; }
	}

	public class EvolvesTo
	{
		public List<EvolutionDetail> evolution_details { get; set; }
		public List<object> evolves_to { get; set; }
		public bool is_baby { get; set; }
		public Species species { get; set; }
	}

	public class Species
	{
		public string name { get; set; }
		public string url { get; set; }
	}

	public class Trigger
	{
		public string name { get; set; }
		public string url { get; set; }
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

	public class Color
	{
		public string name { get; set; }
		public string url { get; set; }
	}

	public class EggGroup
	{
		public string name { get; set; }
		public string url { get; set; }
	}

	public class EvolutionChain
	{
		public string url { get; set; }
	}

	public class EvolvesFromSpecies
	{
		public string name { get; set; }
		public string url { get; set; }
	}

	public class FlavorTextEntry
	{
		public string flavor_text { get; set; }
		public Language language { get; set; }
		public Version version { get; set; }
	}

	public class Genera
	{
		public string genus { get; set; }
		public Language language { get; set; }
	}

	public class Generation
	{
		public string name { get; set; }
		public string url { get; set; }
	}

	public class GrowthRate
	{
		public string name { get; set; }
		public string url { get; set; }
	}

	public class Habitat
	{
		public string name { get; set; }
		public string url { get; set; }
	}

	public class Language
	{
		public string name { get; set; }
		public string url { get; set; }
	}

	public class Name
	{
		public Language language { get; set; }
		public string name { get; set; }
	}

	public class PalParkEncounterArea
	{
		public PalParkArea area { get; set; }
		public int base_score { get; set; }
		public int rate { get; set; }
	}

	public class PokemonEncounter
	{
		public int base_score { get; set; }
		public PokemonSpecies pokemon_species { get; set; }
		public int rate { get; set; }
	}

	public class PalParkArea
	{
		public int id { get; set; }
		public string name { get; set; }
		public List<Name> names { get; set; }
		public List<PalParkEncounterSpecies> pokemon_encounters { get; set; }
	}

	public class PalParkEncounterSpecies
	{
		public int base_score { get; set; }
		public int rate { get; set; }
		public PokemonSpecies pokemon_species { get; set; }
	}

	public class Pokedex
	{
		public string name { get; set; }
		public string url { get; set; }
	}

	public class PokedexNumber
	{
		public int entry_number { get; set; }
		public Pokedex pokedex { get; set; }
	}

	public class PokemonBasic
	{
		public string name { get; set; }
		public string url { get; set; }
	}

	public class PokemonSpecies
	{
		public int base_happiness { get; set; }
		public int capture_rate { get; set; }
		public Color color { get; set; }
		public List<EggGroup> egg_groups { get; set; }
		public EvolutionChain evolution_chain { get; set; }
		public EvolvesFromSpecies evolves_from_species { get; set; }
		public List<FlavorTextEntry> flavor_text_entries { get; set; }
		public List<object> form_descriptions { get; set; }
		public bool forms_switchable { get; set; }
		public int gender_rate { get; set; }
		public List<Genera> genera { get; set; }
		public Generation generation { get; set; }
		public GrowthRate growth_rate { get; set; }
		public Habitat habitat { get; set; }
		public bool has_gender_differences { get; set; }
		public int hatch_counter { get; set; }
		public int id { get; set; }
		public bool is_baby { get; set; }
		public bool is_legendary { get; set; }
		public bool is_mythical { get; set; }
		public string name { get; set; }
		public List<Name> names { get; set; }
		public int order { get; set; }
		public List<PalParkEncounterArea> pal_park_encounters { get; set; }
		public List<PokedexNumber> pokedex_numbers { get; set; }
		public Shape shape { get; set; }
		public List<Variety> varieties { get; set; }
	}

	public class Shape
	{
		public string name { get; set; }
		public string url { get; set; }
	}

	public class Variety
	{
		public bool is_default { get; set; }
		public Pokemon pokemon { get; set; }
	}

	public class Version
	{
		public string name { get; set; }
		public string url { get; set; }
	}

	// GetDataFromApi()
	//var newPokUrl = "https://pokeapi.co/api/v2/evolution-chain/300";

	//using (UnityWebRequest request = UnityWebRequest.Get(newPokUrl))
	//{
	//	yield return request.SendWebRequest();

	//	if (request.result == UnityWebRequest.Result.Success)
	//	{
	//		//PokeApiObj pokApi = JsonConvert.DeserializeObject<PokeApiObj>(request.downloadHandler.text);

	//	}
	//}



	//public class Root
	//{
	//	public object baby_trigger_item { get; set; }
	//	public Chain chain { get; set; }
	//	public int id { get; set; }
	//}


	//public class EvolutionChain
	//{

	//}
}

