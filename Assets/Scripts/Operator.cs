using UnityEngine;

public class Operator : MonoBehaviour
{
    public Manager manager;
    public int power = 0;
    
    public void activateCard(int type)
    {
        if(type == (int)Manager.Type.FUEL)
        {
            manager.updateFuelBypass(Manager.FuelGrade.S, power);
        }
        else
        {
            manager.updateStatsBypass((Manager.Type)type, power);
        }
        fade();
    }

    public void fade()
    {
        Canvas canvas = gameObject.GetComponent<Canvas>();
        canvas.enabled = false;
    }

    public void addCard(int power)
    {
        this.power = power;
    }
}
