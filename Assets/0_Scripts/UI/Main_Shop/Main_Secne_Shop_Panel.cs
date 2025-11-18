using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Main_Secne_Shop_Panel : MonoBehaviour
{
    int id;
    [SerializeField] private Image Skill_Image;
    [SerializeField] private TextMeshProUGUI Skill_Text;
    [SerializeField] private TextMeshProUGUI Gold_Text;
    
    public void Init(int id)
    {
        this.id = id;
		string path = $"Art/Main_Shop/Image_{id}";
		Sprite sprite = Resources.Load<Sprite>(path);
		Skill_Image.sprite = sprite;
	}

}
