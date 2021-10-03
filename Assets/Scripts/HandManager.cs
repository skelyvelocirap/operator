using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HandManager : MonoBehaviour
{
    public GameObject cardCanvas;
    Dictionary<Card, Object> card_library = new Dictionary<Card, Object>();
    public (Object, Card)[] hand = new (Object, Card)[7];

    public int startCards = 5;

    Bank permaBank = new Bank();
    private Bank bank = new Bank();
    void Start()
    {
        /*CREATE LIBRARY*/

        //FUEL
        Card URANIUM_235_S = new Card(Manager.FuelGrade.S, 1, 1);
        addCardToLib(URANIUM_235_S, getCardPrefab("URANIUM"));

        Card URANIUM_235_A = new Card(Manager.FuelGrade.A, 1, 1);
        addCardToLib(URANIUM_235_A, getCardPrefab("URANIUM"));

        Card URANIUM_235_B = new Card(Manager.FuelGrade.B, 1, 1);
        addCardToLib(URANIUM_235_B, getCardPrefab("URANIUM"));

        Card URANIUM_235_C = new Card(Manager.FuelGrade.C, 1, 1);
        addCardToLib(URANIUM_235_C, getCardPrefab("URANIUM"));

        Card URANIUM_235_D = new Card(Manager.FuelGrade.D, 1, 1);
        addCardToLib(URANIUM_235_D, getCardPrefab("URANIUM"));

        Card URANIUM_235_F = new Card(Manager.FuelGrade.F, 1, 1);
        addCardToLib(URANIUM_235_F, getCardPrefab("URANIUM"));

        Card URANIUM_235_S_2 = new Card(Manager.FuelGrade.S, 2, 1);
        addCardToLib(URANIUM_235_S_2, getCardPrefab("URANIUM_2"));

        Card URANIUM_235_A_2 = new Card(Manager.FuelGrade.A, 2, 1);
        addCardToLib(URANIUM_235_A_2, getCardPrefab("URANIUM_2"));

        Card URANIUM_235_B_2 = new Card(Manager.FuelGrade.B, 2, 1);
        addCardToLib(URANIUM_235_B_2, getCardPrefab("URANIUM_2"));

        Card URANIUM_235_C_2 = new Card(Manager.FuelGrade.C, 2, 1);
        addCardToLib(URANIUM_235_C_2, getCardPrefab("URANIUM_2"));

        Card URANIUM_235_D_2 = new Card(Manager.FuelGrade.D, 2, 1);
        addCardToLib(URANIUM_235_D_2, getCardPrefab("URANIUM_2"));

        Card URANIUM_235_F_2 = new Card(Manager.FuelGrade.F, 2, 1);
        addCardToLib(URANIUM_235_F_2, getCardPrefab("URANIUM_2"));





        //PLAYER_ENERGY
        Card COFFEE = new Card(Manager.Type.ENERGY, 2, 0);
        addCardToLib(COFFEE, getCardPrefab("COFFEE"));
        //POWER        
        Card BATTERY_PARTIAL = new Card(Manager.Type.POWER, 1, 1);
        addCardToLib(BATTERY_PARTIAL, getCardPrefab("BATTERY_PARTIAL"));

        Card BATTERY_FULL = new Card(Manager.Type.POWER, 2, 1);
        addCardToLib(BATTERY_FULL, getCardPrefab("BATTERY_FULL"));
        //COOLING
        Card SNOWFLAKE = new Card(Manager.Type.COOLING, 1, 1);
        addCardToLib(SNOWFLAKE, getCardPrefab("SNOWFLAKE"));
        //DRAW
        Card DRAW = new Card(Manager.Type.DRAW, 2, 1);
        addCardToLib(DRAW, getCardPrefab("DRAW"));
        //OPERATOR
        Card OPERATOR = new Card(Manager.Type.OPERATOR, 1, 0);
        addCardToLib(OPERATOR, getCardPrefab("OPERATOR"));

        Card OPERATOR_2 = new Card(Manager.Type.OPERATOR, 2, 0);
        addCardToLib(OPERATOR_2, getCardPrefab("OPERATOR_2"));

        /*CREATE BANK*/
        addCards(URANIUM_235_S, 10);
        addCards(URANIUM_235_A, 10);
        addCards(URANIUM_235_B, 10);
        addCards(URANIUM_235_C, 10);
        addCards(URANIUM_235_D, 10);
        addCards(URANIUM_235_F, 10);

        addCards(URANIUM_235_S_2, 10);
        addCards(URANIUM_235_A_2, 10);
        addCards(URANIUM_235_B_2, 10);
        addCards(URANIUM_235_C_2, 10);
        addCards(URANIUM_235_D_2, 10);
        addCards(URANIUM_235_F_2, 10);

        addCards(BATTERY_FULL, 30);
        addCards(BATTERY_PARTIAL, 80);
        addCards(SNOWFLAKE, 100);
        addCards(COFFEE, 50);
        addCards(DRAW, 20);
        addCards(OPERATOR, 5);
        addCards(OPERATOR_2, 1);
        resetBank();
    }

    private Object getCardPrefab(string name)
    {
        return Resources.Load("Cards/" + name);
    }

    public void resetBank()
    {
        foreach((Object, Card) pair in hand)
        {
            if(pair.Item1 != null)
            {
                Destroy(pair.Item1);
            }
        }

        hand = new (Object, Card)[7];

        bank = new Bank(permaBank.copy());

        drawFromBank(5);
    }

    public void drawOnScreen(int index, Card card)
    {
        if (card != null)
        {
            if (card_library.ContainsKey(card))
            {
                Object cardObject = card_library[card];
                if (cardObject != null)
                {
                    GameObject go = (GameObject)Instantiate(cardObject, cardCanvas.transform);
                    go.transform.localPosition = new Vector2(120 + (index * 280), 180);
                    InteractiveCard interactive;
                    if (card.grade != Manager.FuelGrade.NONE)
                    {
                        interactive = go.AddComponent<InteractiveFuelCard>();
                        string name = "URANIUM (GRADE ";
                        if (card.grade == Manager.FuelGrade.S)
                        {
                            name += "S";
                        }
                        else if (card.grade == Manager.FuelGrade.A)
                        {
                            name += "A";
                        }
                        else if (card.grade == Manager.FuelGrade.B)
                        {
                            name += "B";
                        }
                        else if (card.grade == Manager.FuelGrade.C)
                        {
                            name += "C";
                        }
                        else if (card.grade == Manager.FuelGrade.D)
                        {
                            name += "D";
                        }
                        else if (card.grade == Manager.FuelGrade.F)
                        {
                            name += "F";
                        }
                        name += ")";

                        Text fuelName = go.transform.Find("Name").GetComponent<Text>();
                        fuelName.text = name;
                    }
                    else
                    {
                        interactive = go.AddComponent<InteractiveCard>();
                    }
                    interactive.fromCard(card);
                    interactive.handIndex = index;
                    hand[index] = (go, card);
                }
            }
        }
        else
        {
            GameObject go = (GameObject)Instantiate(getCardPrefab("EMPTY"), cardCanvas.transform);
            go.transform.localPosition = new Vector2(120 + (index * 280), 180);
        }
    }

    private void addCardToLib(Card card, Object prefab)
    {
        card_library.Add(card, prefab);
    }

    private void addCards(Card card, int count)
    {
        for(int i = 0; i < count; i++)
        {
            permaBank.addCard(card);
        }
    }

    public void drawFromBank(int ammount)
    {
        int cardsLeft = 0;
        for(int i = 0; i < hand.Length; i++)
        {
            if(hand[i].Item2 == null)
            {
                cardsLeft += 1;
            }
        }
        int count = Mathf.Min(cardsLeft, ammount);
        for (int i = 0; i < count; i++)
        {
            for(int j = 0; j < hand.Length; j++)
            {
                if (hand[j].Item2 == null)
                {
                    drawOnScreen(j, bank.pickRandom());
                    break;
                }
            }
        }
    }

    public Bank getBank()
    {
        return bank;
    }
}
