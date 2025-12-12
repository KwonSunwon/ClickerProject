using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MineResultItem : MonoBehaviour
{
    [SerializeField] private Image _image;
    [SerializeField] private TMP_Text _name;
    [SerializeField] private TMP_Text _count;

    public void Set(MineralType type, int count)
    {
        _image.sprite = Resources.Load<Sprite>($"Art/Ore/{type}");
        _name.text = type.ToString();
        _count.text = count.ToString();
    }
}
