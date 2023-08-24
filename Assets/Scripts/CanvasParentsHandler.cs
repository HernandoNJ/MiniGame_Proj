using System.Collections.Generic;
using UnityEngine;

public class CanvasParentsHandler : MonoBehaviour
{
	public List<GameObject> pokePanelsList;
	public ButtonHandler buttonsHandler;
	public int currentPanelIndex;
	public int nextPanelIndex;
	public int previousPanelIndex;
	public bool isFirstPanel;
	public bool isLastPanel;

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
			if (i == currentPanelIndex) pokePanelsList[i].SetActive(true);
			else pokePanelsList[i].SetActive(false);
		}

		if (currentPanelIndex == 0)
		{
			isFirstPanel = true;
			isLastPanel = false;
			buttonsHandler.EnableButtons(false, true);
		}
		else if (currentPanelIndex == 5)
		{
			isFirstPanel = false;
			isLastPanel = true;
			buttonsHandler.EnableButtons(true, false);
		}
		else
		{
			isFirstPanel = true;
			isLastPanel = true;
			buttonsHandler.EnableButtons(true, true);
		}
	}
}
