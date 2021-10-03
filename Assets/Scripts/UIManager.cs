using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Manager manager;

    //HEAT
    public Animator SIREN_ANIMATOR;
    public Text TEMPERATURE_LCD;
    public int temperaturePercent = 30;

    public int temperatureThreshold = 90;
    public bool isSirenEnabled = true;

    //Temperature
    public Image URANIUM_LCD;
    public Text URANIUM_GRADE;
    public Text TOTAL_COUNT;

    public Sprite uranium;
    public Sprite empty;

    public Manager.FuelGrade grade;
    public int count = 0;

    //Player Energy
    public Text PLAYER_ENERGY;

    //DAYS WITHOUT POWER
    public GameObject DAYS_WITHOUT_POWER;
    public Sprite on;
    public Sprite off;

    //POWER
    public Text POWER_TEXT;

    //LOSE
    public Canvas LOSE_CANVAS;
    public Text LOSE_MESSAGE;
    public Text HIGHSCORE;
    public Text SCORE;

    private void Update()
    {
        if(grade != Manager.FuelGrade.NONE)
        {
            URANIUM_LCD.sprite = uranium;
            if (grade == Manager.FuelGrade.S)
            {
                URANIUM_GRADE.text = "GRADE: S";
            }
            else if (grade == Manager.FuelGrade.A)
            {
                URANIUM_GRADE.text = "GRADE: A";
            }
            else if (grade == Manager.FuelGrade.B)
            {
                URANIUM_GRADE.text = "GRADE: B";
            }
            else if (grade == Manager.FuelGrade.C)
            {
                URANIUM_GRADE.text = "GRADE: C";
            }
            else if (grade == Manager.FuelGrade.D)
            {
                URANIUM_GRADE.text = "GRADE: D";
            }
            else if (grade == Manager.FuelGrade.F)
            {
                URANIUM_GRADE.text = "GRADE: F";
            }
        } else
        {
            URANIUM_LCD.sprite = empty;
            URANIUM_GRADE.text = "GRADE: N/A";
        }
        if(count > 0)
        {
            TOTAL_COUNT.text = "TOTAL: " + count;
        }
        else
        {
            TOTAL_COUNT.text = "TOTAL: NaN";
        }


        TEMPERATURE_LCD.text = "Temperature: " + temperaturePercent + "%";
        if(temperaturePercent > temperatureThreshold)
        {
            SIREN_ANIMATOR.SetBool("isAboveThreshold", true);
        }
        else
        {
            SIREN_ANIMATOR.SetBool("isAboveThreshold", false);
        }
    }

    public void togleEnabled()
    {
        isSirenEnabled = !SIREN_ANIMATOR.GetBool("isEnabled");
        SIREN_ANIMATOR.SetBool("isEnabled", isSirenEnabled);
    }

    public void setEnergy(int energy)
    {
        PLAYER_ENERGY.text = "Energy: " + energy;
    }

    public void setDaysWithoutPower(int days)
    {
        for (int i = 0; i <= 5; i++)
        {
            Transform go = DAYS_WITHOUT_POWER.transform.Find(i + "");
            if(go != null)
            {
                Image screen = go.GetComponent<Image>();
                if (screen != null)
                {
                    if (i <= days)
                    {
                        screen.sprite = on;
                    }
                    else
                    {
                        screen.sprite = off;
                    }
                }
            }
        }
    }

    public void setPower(int power)
    {
        if(power == 0)
        {
            POWER_TEXT.text = "STORED: NaN";
        }
        else
        {
            POWER_TEXT.text = "STORED: " + power;
        }
    }

    public void setLose(death_type death, int score)
    {
        HIGHSCORE.text = "Most Days Providing Power: " + manager.highScore;
        SCORE.text = "Days Providing Power: " + score;
        if (death == death_type.MELTDOWN)
        {
            string[] messages = new string[] {
                "Damn, the reactor had a meltdown.",
                "Umm... What did you just do? Why is everything burning?",
                "... Why did we send in an untrained civilian for this? Of course it would blow up!",
                "You are fired! Litteraly!",
                "What are we? Cavemen?"
            };
            LOSE_MESSAGE.text = messages[Random.Range(0, messages.Length)];
        }
        else if (death == death_type.NO_POWER) {
            string[] messages = new string[] {
                "\"\"",
                "Fun fact! 100% of successful reactors produce power.",
                "No power, no pay.",
                "We produce power here sir."
            };
            LOSE_MESSAGE.text = messages[Random.Range(0, messages.Length)];
        }
        LOSE_CANVAS.enabled = true;
    }


    public enum death_type
    {
        MELTDOWN,
        NO_POWER
    }
}
