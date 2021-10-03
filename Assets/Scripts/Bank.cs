using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bank
{
    public List<Card> cards = new List<Card>();

    public Bank()
    {

    }
    public Bank(List<Card> _cards)
    {
        cards = new List<Card>(_cards);
    }
    public Card pickRandom()
    {
        if (cards.Count > 0)
        {
            int max = cards.Count;
            int index = Random.Range(0, max);
            Card card = cards[index];
            cards.Remove(card);
            Card[] temp = new Card[cards.Count];
            cards.CopyTo(temp);
            cards.Clear();
            cards.AddRange(temp);
            return card;
        }
        return null;
    }

    public List<Card> copy()
    {
        return new List<Card>(cards);
    }

    public void addCard(Card card)
    {
        cards.Add(card);
    }
}
