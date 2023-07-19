using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class LoadTextureFromURL : MonoBehaviour
{

	public Texture2D texture2DFromUrl;

	public void GetNewImage(string textureUrl)
	{
		StartCoroutine(DownloadImage(textureUrl));
	}

	IEnumerator DownloadImage(string textureUrl)
	{
		UnityWebRequest request = UnityWebRequestTexture.GetTexture(textureUrl);
		
		yield return request.SendWebRequest();
		
		if (request.result == UnityWebRequest.Result.ConnectionError)
			Debug.Log(request.error);
		else
		{
			var newTexture = ((DownloadHandlerTexture)request.downloadHandler).texture;
			texture2DFromUrl = newTexture;
			gameObject.GetComponent<Renderer>().material.mainTexture = texture2DFromUrl;
		}
	}
}