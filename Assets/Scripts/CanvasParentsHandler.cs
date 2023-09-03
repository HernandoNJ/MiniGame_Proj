using System.Collections.Generic;
using UnityEngine;

public class CanvasParentsHandler : MonoBehaviour
{
	public List<GameObject> parentsHandlerList;
	public ButtonHandler buttonsHandler;
	public int initialPanelIndex;
	public int lastPanelIndex;
	public int currentPanelIndex;

	private void Start()
	{
		currentPanelIndex = 0;
		EnablePanel();
		buttonsHandler.EnableButtons(false, true);
	}

	public void EnablePanel()
	{
		for (int i = 0; i < parentsHandlerList.Count; i++)
		{
			if (i == currentPanelIndex) parentsHandlerList[i].SetActive(true);
			else parentsHandlerList[i].SetActive(false);
		}

		if (currentPanelIndex == initialPanelIndex) buttonsHandler.EnableButtons(false, true);
		else if (currentPanelIndex == lastPanelIndex) buttonsHandler.EnableButtons(true, false);
		else buttonsHandler.EnableButtons(true, true);
	}

	public void EnablePokemonsList(int count) => parentsHandlerList[currentPanelIndex].GetComponent<ParentHandler>().EnablePokemons(count);
}
