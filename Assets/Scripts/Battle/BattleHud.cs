using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BattleHud : MonoBehaviour
{
    [SerializeField] TMP_Text nameText;
    [SerializeField] TMP_Text levelText;
    [SerializeField] HPBar hpBar;
    public string name;
    public int level, hp, maxhp;
    public List<string> moveNames;
    //public List<int> damage;
    //public List<int> pp;
    //public List<int> maxpp;
    //public List<string> type;

    public void SetData()
    {
        nameText.text = name;
        levelText.text = "Lvl " + level;
        hpBar.SetHP((float)hp / maxhp);

    }

    void Start()
    {
        SetData();
    }

    public IEnumerator UpdateHP()
    {
        yield return hpBar.SetHPSmooth((float)hp / maxhp);
    }

    public void setHP()
    {
        hpBar.SetHP((float)hp / maxhp);
    }

}
