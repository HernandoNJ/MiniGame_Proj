using System.Collections.Generic;
using UnityEngine;

public class ObjectsToView : MonoBehaviour
{
	public List<GameObject> pokeCardsList0;
	public List<GameObject> pokeCardsList1;
	public List<GameObject> pokeCardsList2;

	public int currentPokeList;

	public void AddItemToPokeCardsList(GameObject pokeObj, int listNumber)
	{
		if (listNumber == 0) pokeCardsList0.Add(pokeObj);
		else if (listNumber == 1) pokeCardsList1.Add(pokeObj);
		else if (listNumber == 2) pokeCardsList2.Add(pokeObj);
	}

	public void SetCurrentPokeList(int n) => currentPokeList = n;

	public void SetVisiblePokemons(int count)
	{
		if (currentPokeList == 0)
		{
			for (int i = 0; i < pokeCardsList0.Count; i++)
			{
				if (i < count) pokeCardsList0[i].SetActive(true);
				else pokeCardsList0[i].SetActive(false);
			}
		}
		else if (currentPokeList == 1)
		{
			for (int i = 0; i < pokeCardsList1.Count; i++)
			{
				if (i < count) pokeCardsList1[i].SetActive(true);
				else pokeCardsList1[i].SetActive(false);
			}
		}
		else if (currentPokeList == 2)
		{
			for (int i = 0; i < pokeCardsList2.Count; i++)
			{
				if (i < count) pokeCardsList2[i].SetActive(true);
				else pokeCardsList2[i].SetActive(false);
			}
		}
	}
}
