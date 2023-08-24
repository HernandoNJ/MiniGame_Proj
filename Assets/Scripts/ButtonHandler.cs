using UnityEngine;

public class ButtonHandler : MonoBehaviour
{
	public CanvasParentsHandler canvasParentsHandler;
	public GameObject previousButton;
	public GameObject nextButton;
	public bool isPrevious;
	public bool isNext;

	public void EnableButtons(bool prevEnabled, bool nextEnabled)
	{
		previousButton.SetActive(prevEnabled);
		nextButton.SetActive(nextEnabled);
	}

	public void ButtonClicked()
	{
		if (isNext) canvasParentsHandler.currentPanelIndex++;
		else if(isPrevious) canvasParentsHandler.currentPanelIndex--;
		canvasParentsHandler.EnablePanel();
	}
}
