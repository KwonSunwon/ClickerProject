using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Main_Secne_Shop_Panel : MonoBehaviour
{
    int id;

    BigNumber testCost = new BigNumber(1);
    int testLevel = 1;


    [SerializeField] private Image Skill_Image;
    [SerializeField] private TextMeshProUGUI Skill_Text;
    [SerializeField] private TextMeshProUGUI Gold_Text;

    public void Init(int id)
    {
        this.id = id;
        string path = $"Art/Main_Shop/Image_{id}";
        Sprite sprite = Resources.Load<Sprite>(path);
        Skill_Image.sprite = sprite;

        testCost = new BigNumber(1);
        testLevel = 1;

        Gold_Text.text = $"{testCost}";
        Skill_Text.text = $"Level: {testLevel}";
    }

    public bool LevelUp()
    {
        if (Managers.Stat.Gold >= testCost) {
            Managers.Stat.Gold -= testCost;
            testLevel++;
            testCost = Cost(testLevel);
            Gold_Text.text = $"{testCost}";
            Skill_Text.text = $"Level: {testLevel}";

            switch (id) {
                case 1:
                    Managers.Stat.Base_Click_Damage = testLevel;
                    break;


				case 101:
                    MineManager.Instance.SpawnWorker();
                    break;
                default:
                    Debug.Log($"@Main_Scene_Shop_Panel -  ID:{id}, No Skill Assignment");
                    break;
            }
            return true;
        }
        else {
            return false;
        }
    }

    BigNumber Cost(int level)
    {
        BigNumber baseCost = new BigNumber(10);      // 초기 비용
        double growth = 1.15;                        // 15% 증가

        return baseCost * Math.Pow(growth, level);
    }

}
