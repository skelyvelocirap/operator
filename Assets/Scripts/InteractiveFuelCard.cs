using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractiveFuelCard : InteractiveCard
{
    public Manager.FuelGrade grade;

    public override void fromCard(Card card)
    {
        type = card.type;
        grade = card.grade;
        power = card.power;
        cost = card.cost;
    }

    public override void onClick()
    {
        if (!clicked)
        {
            bool success = false;
            if (grade != Manager.FuelGrade.NONE)
            {
                success = manager.updateFuel(grade, power, cost);
            }

            if(success == true)
            {
                for (int i = 0; i < transform.childCount; i++)
                {
                    Transform child = transform.GetChild(i);
                    if (child.name == "Border")
                    {
                        child.GetComponent<Image>().sprite = Resources.LoadAll<Sprite>("UI/Sprites/card_base")[2];
                        child.GetComponent<Image>().color = Color.black;
                    }
                    else
                    {
                        Destroy(child.gameObject);
                    }
                }

                manager.manager.hand[handIndex].Item2 = null;
                clicked = true;
            }
        }
    }
}
