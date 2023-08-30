using System.Collections.Generic;
using UnityEngine;

public class ObjectsToView : MonoBehaviour
{
	public int currentPanel;
	public CanvasParentsHandler canvasParentsHandler;
	public List<PokePanel> pokemonCards;

	public void SetCurrentPanel(int activePanelInt)
	{
		currentPanel = activePanelInt;
		//canvasParentsHandler.pokePanelsList[]
		//activePokeList[currentPanel].SetActive(true);
	}

	public void SetVisiblePokemons(int count)
	{
		//for (int i = 0; i < activePokeList.Count; i++)
		//{

		//}
	}
}
