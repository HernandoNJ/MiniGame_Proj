using System.Collections.Generic;
using UnityEngine;

public class ParentHandler : MonoBehaviour
{
	public int panelIndex;
	public bool isFirst;
	public bool isLast;
	public List<GameObject> pokemonsList;

	public void AddPokemonToList(GameObject obj)
	{
		pokemonsList.Add(obj);
		obj.SetActive(true);
	}

	public void EnablePokemons(int count)
	{
		for (int i = 0; i < pokemonsList.Count; i++)
		{
			if (i < count) pokemonsList[i].SetActive(true);
			else pokemonsList[i].SetActive(false);
		}
	}
}
