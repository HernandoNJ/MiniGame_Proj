using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager.Requests;

public class RestAPI_2 : MonoBehaviour
{
	/* 1. Get all pokemons from json
	 * 
	 */

	public string textureUrl1;
	public string pokeName;
	public string pokeExp;
	public string pokeApiV2Url = "https://pokeapi.co/api/v2/pokemon/?limit=100";
	
	//public Text text1;
	//public string url = "https://catfact.ninja/fact";
	//public object objFromJsonCached;
	//public Fact factObj;

	private void Start()
	{
		StartCoroutine(GetDataFromApi());
	}

	private IEnumerator GetDataFromApi()
	{
		//using (UnityWebRequest request = UnityWebRequest.Get(url))
		//{
		//	yield return request.SendWebRequest();
			
		//	switch (request.result)
		//	{
		//		case UnityWebRequest.Result.ConnectionError:
		//		case UnityWebRequest.Result.DataProcessingError:
		//			Debug.LogError($"Data processing error {request.error}");
		//			break;
		//		case UnityWebRequest.Result.Success:
		//			// request.downloadHandler.text converts the request into text (Json)
		//			// DeserializeObject<Fact> converts the text into a Fact object
		//			Fact fact = JsonConvert.DeserializeObject<Fact>(request.downloadHandler.text);
		//			text1.text = fact.fact;
		//			break;
		//	}
		//}

		using (UnityWebRequest request = UnityWebRequest.Get(pokeApiV2Url))
		{
			yield return request.SendWebRequest();

			switch (request.result)
			{
				case UnityWebRequest.Result.ConnectionError:
				case UnityWebRequest.Result.DataProcessingError:
					Debug.LogError($"Data processing error {request.error}");
					break;
				case UnityWebRequest.Result.Success:
					// request.downloadHandler.text converts the request into text (Json)
					// DeserializeObject<Fact> converts the text into a Fact object
					PokemonApiUrl pokeApiurl = JsonConvert.DeserializeObject<PokemonApiUrl>(request.downloadHandler.text);

					Debug.Log(pokeApiurl.previous);
					Debug.Log(pokeApiurl.next);
					Debug.Log(pokeApiurl.count);
					Debug.Log(pokeApiurl.results[20].name);
					Debug.Log(pokeApiurl.results[20].url);
					Debug.Log(pokeApiurl.results[50].name);
					Debug.Log(pokeApiurl.results[50].url);
					Debug.Log(pokeApiurl.results[72].name);
					Debug.Log(pokeApiurl.results[72].url);

					Debug.Log("results count: " + pokeApiurl.results.Count);
					break;
			}
		}

		StopAllCoroutines();
	}

	public class Fact
	{
		public string fact { get; set; }
		public int length { get; set; }
	}

	public class Result
	{
		public string name { get; set; }
		public string url { get; set; }
	}

	public class PokemonApiUrl
	{
		public int count { get; set; }
		public string next { get; set; }
		public object previous { get; set; }
		public List<Result> results { get; set; }
	}
}
