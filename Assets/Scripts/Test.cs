using Newtonsoft.Json;
using Pokemon.API;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class Test : MonoBehaviour
{
	public string pokemonSpeciesUrl = "https://pokeapi.co/api/v2/pokemon-species/132";
	public PokemonSpecies pokemonSpecies;

	private void Start()
	{
		pokemonSpecies = new();
		StartCoroutine(nameof(GetDataFromApi));
	}

	private IEnumerator GetDataFromApi()
	{
		Debug.Log("url: " + pokemonSpeciesUrl);

		using (UnityWebRequest request1 = UnityWebRequest.Get(pokemonSpeciesUrl))
		{
			yield return request1.SendWebRequest();
			Debug.Log("request: " + request1);
			pokemonSpecies = JsonConvert.DeserializeObject<PokemonSpecies>(request1.downloadHandler.text);
			Debug.Log("species 1: " + pokemonSpecies.Base_happiness);
		}
	}
}
