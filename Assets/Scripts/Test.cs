using UnityEngine;

public class Test : MonoBehaviour
{
	public GameObject pokePanelPrefab;
	public Transform canvasParent;

	private void Start()
	{
		InvokeRepeating(nameof(InstantiatePokePrefabs), 1, 0.1f);
	}

	private void InstantiatePokePrefabs()
	{
		Instantiate(pokePanelPrefab, canvasParent);
	}
}
