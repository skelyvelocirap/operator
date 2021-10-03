using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card
{
    public Manager.FuelGrade grade;
    public Manager.Type type;
    public int power;
    public int cost;

    public Card(Manager.Type _type, int _power, int _cost)
    {
        type = _type;
        grade = Manager.FuelGrade.NONE;
        power = _power;
        cost = _cost;
    }

    public Card(Manager.FuelGrade _grade, int _power, int _cost)
    {
        type = Manager.Type.FUEL;
        grade = _grade;
        power = _power;
        cost = _cost;
    }
}
