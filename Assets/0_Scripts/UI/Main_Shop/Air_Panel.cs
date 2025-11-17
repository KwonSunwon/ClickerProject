using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Air_Panel : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI Remain_Air_Text;
    [SerializeField] Slider Remain_Air_Slider;


    void Start()
    {

        Remain_Air_Text.text = $"{(int)Managers.Stat.Air}/{(int)Managers.Stat.MaxAir}";
		Remain_Air_Slider.value = Managers.Stat.Air / Managers.Stat.MaxAir;


    }
	private void FixedUpdate()
	{
		Remain_Air_Text.text = $"{(int)Managers.Stat.Air}/{(int)Managers.Stat.MaxAir}";
		Remain_Air_Slider.value = Managers.Stat.Air /Managers.Stat.MaxAir;
	}
}
