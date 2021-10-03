using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractiveCard : MonoBehaviour
{
    public bool clicked = false;
    public Manager manager;
    public Manager.Type type;
    public int handIndex;
    public int power;
    public int cost;

    public void Start()
    {
        manager = GameObject.FindGameObjectWithTag("Manager").GetComponent<Manager>();
        gameObject.GetComponent<Button>().onClick.AddListener(onClick);
    }

    public virtual void fromCard(Card card)
    {
        type = card.type;
        power = card.power;
        cost = card.cost;
    }

    public virtual void onClick()
    {
        if (!clicked)
        {
            manager.manager.hand[handIndex].Item2 = null;
            bool success = manager.updateStats(type, power, cost);
            if (success)
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

                clicked = true;
            }
        }
    }
}
