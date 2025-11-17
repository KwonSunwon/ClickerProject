using TMPro;
using UnityEngine;

public class Gold_Panel : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI Gold_Text;

	private void FixedUpdate()
	{
		Gold_Text.text = Managers.Stat.Gold.ToString();
	}
}
