using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using SimpleJSON;
using Newtonsoft.Json;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;

public class RestAPI : MonoBehaviour
{

	public LoadTextureFromURL textureUrlClass;
	public string textureUrl;

	public string pokeApiUrl;

	public Pokemon pokemon;
	public Text LevelText;
	public Text ExpText;

	private void Start()
	{
		pokeApiUrl = "https://pokeapi.co/api/v2/pokemon/25/";

		StartCoroutine(nameof(GetData));
	}

	private IEnumerator GetData()
	{
		using (UnityWebRequest request = UnityWebRequest.Get(pokeApiUrl))
		{
			yield return request.SendWebRequest();

			if (request.result == UnityWebRequest.Result.ConnectionError)
			{
				Debug.LogError("Request error message: " + request.error);
			}
			else
			{
				var jsonText = request.downloadHandler.text;
				Pokemon pokemon1 = JsonUtility.FromJson<Pokemon>(jsonText);

				Debug.Log("name: " + pokemon1.name);
				Debug.Log("base experience: " + pokemon1.base_experience);
				Debug.Log("id: " + pokemon1.id);
				Debug.Log("is default: " + pokemon1.is_default);
				Debug.Log("sprites: " + pokemon1.sprites.back_default);
				textureUrl = pokemon1.sprites.front_default;
			}
		}

		textureUrlClass.GetNewImage(textureUrl);

		// Dispose();
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

/*
 * // Wrapper Library https://github.com/mtrdp642/PokeApiNet 
//apiUrl = "https://pokeapi.co/api/v2/";

// [{"level":28,"exp":25,"id":"1"},{"level":91,"exp":42,"id":"2"}]
//apiUrl = "https://637614ce7e93bcb006c28f1f.mockapi.io/Character";

//apiUrl = "https://pokeapi.co/api/v2/pokemon/ditto";
//apiUrl = "https://pokeapi.co/api/v2/pokemon-species/aegislash"; 

//https://pokeapi.co/api/v2/pokemon-species/aegislash


//apiUrl = "https://pokeapi.co/api/v2/evolution-chain/10";
//apiUrl = "https://pokeapi.co/api/v2/pokemon-form/10041";
 * 
 */


/* Internally, PokeApiClient uses an instance of the HttpClient class. As such, instances of PokeApiClient are meant to be instantiated once and re-used throughout the life of an application.
 
 * Navigation URLs: PokeAPI uses navigation urls for many of the resource's properties to keep requests lightweight, but require subsequent requests in order to resolve this data. Example:
	Pokemon pikachu = await pokeClient.GetResourceAsync<Pokemon>("pikachu");
 	
 *	pikachu.Species only has a Name and Url property. In order to load this data, an additonal request is needed; this is more of a problem when the property is a list of navigation URLs, such as the pikachu.Moves.Move collection.
 	
 *	GetResourceAsync includes overloads to assist with resolving these navigation properties. Example:
 
 *	// to resolve a single navigation url property
	PokemonSpecies species = await pokeClient.GetResourceAsync(pikachu.Species);
 
 *	// to resolve a list of them
	List<Move> allMoves = await pokeClient.GetResourceAsync(pikachu.Moves.Select(move => move.Move));
 
//Pokemon pokemon1 = JsonConvert.DeserializeObject<Pokemon>((string)request);

// json: object returned from the web request
//var json = request.downloadHandler.text;
//Debug.Log(json);

//newJson2 = JsonConvert.SerializeObject(json, Formatting.Indented);
//Debug.Log("Json2: \n" + newJson2);

//// Parse data to JSON Node
//JSONNode stats = JSON.Parse(json);

//Debug.Log(stats);

//// Assign the parsed data to a text object
//LevelText.text = "Level: " + stats[index]["ability"];
//ExpText.text = "Exp: " + stats[index]["name"];

//using static UnityEditor.Progress;
//using PokeApiNet;

 */

/*
--- Type ---

public class Account
{
    public string Email { get; set; }
    public bool Active { get; set; }
    public DateTime CreatedDate { get; set; }
    public IList<string> Roles { get; set; }
}

--- Usage ---

Account account = new Account
{
    Email = "james@example.com",
    Active = true,
    CreatedDate = new DateTime(2013, 1, 20, 0, 0, 0, DateTimeKind.Utc),
    Roles = new List<string>
    {
        "User",
        "Admin"
    }
};

string json = JsonConvert.SerializeObject(account, Formatting.Indented);
// json Result: 

// {
//   "Email": "james@example.com",
//   "Active": true,
//   "CreatedDate": "2013-01-20T00:00:00Z",
//   "Roles": [
//     "User",
//     "Admin"
//   ]
// }

Console.WriteLine(json);

 
 

 
 
 */

