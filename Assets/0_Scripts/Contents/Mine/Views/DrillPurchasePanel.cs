using UnityEngine;
using UnityEngine.EventSystems;

public class DrillPurchasePanel : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] GameObject[] HideObjects;


	public void OnPointerClick(PointerEventData eventData)
	{
		Debug.Log("Click");
		foreach (GameObject go in HideObjects)
		{
			go.SetActive(false);
		}
	}

	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
