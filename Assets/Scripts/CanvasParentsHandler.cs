using System.Collections.Generic;
using UnityEngine;

public class CanvasParentsHandler : MonoBehaviour
{
	public List<GameObject> pokePanelsList;
	public ButtonHandler buttonsHandler;
	public ObjectsToView objectsToView;
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
		for (int i = 0; i < pokePanelsList.Count; i++)
		{
			if (i == currentPanelIndex)
			{
				pokePanelsList[i].SetActive(true);
				objectsToView.SetCurrentPokeList(i);
			}
			else pokePanelsList[i].SetActive(false);
		}

		if (currentPanelIndex == 0) buttonsHandler.EnableButtons(false, true);
		else if (currentPanelIndex == 2) buttonsHandler.EnableButtons(true, false);
		else buttonsHandler.EnableButtons(true, true);
	}
}
