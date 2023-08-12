using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OnHover : MonoBehaviour
{
    public BattleDialogBox box;
    public TMP_Text buttonText;
    public Color hoverColor;
    // Start is called before the first frame update
    public string moveName, type;
    public int pp, maxpp;
    public int dmg;

    public void hovering()
    {
        if (box.state == BattleState.PlayerAction)
        {
           buttonText.color = hoverColor;
        }
        else if(box.state == BattleState.PlayerMove)
        {
            buttonText.color = hoverColor;
            if(type != "")
            {
                box.ppText.text = $"PP {pp}/{maxpp}";
            }
            else
            {
                box.ppText.text = "";
            }
            
            box.typeText.text = $"{type}";
        }
    }

    public void nothovering()
    {
        buttonText.color = Color.black;
    }

    public void clicked()
    {
        if (box.state == BattleState.PlayerAction)
        {
            box.PlayerMove();
        }
        else if(box.state == BattleState.PlayerMove && pp > 0)
        {
            pp--;
            box.EnableMoveSelector(false);
            box.EnableDialogText(true);
            box.PerformPlayerMove(moveName,dmg);
        }
    }

    public void runClick()
    {
        if (box.state == BattleState.PlayerAction)
        {
            box.Run();
        }
    }
}
