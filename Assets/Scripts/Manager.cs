using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Manager : MonoBehaviour
{
    public UIManager ui_manager;
    public Canvas operator_canvas;


    public int DAILY_QUOTA;
    public Text quota_text;
    public FuelGrade nextFuel = FuelGrade.NONE;
    public Image nextFuelImage;
    public Text nextFuelText;

    public HandManager manager;
    public Text highscore;

    public int DAYS_WITH_POWER = 0;

    //STATS

    public int PLAYER_ENERGY = 3;

    public static int maxFuelRod = 5;
    public Queue<FuelGrade> fuelStorage = new Queue<FuelGrade>();
    public float HEAT = 0.3F;

    public int POWER = 0;
    public int DAYS_SINCE_POWER = 0;
    public int highScore = 0;

    public bool lost = false;

    public void Awake()
    {
        if (PlayerPrefs.HasKey("highscore"))
        {
            highScore = PlayerPrefs.GetInt("highscore");
        }
    }
    private void Start()
    {
        quota_text.text = "QUOTA: " + DAILY_QUOTA + " POWER";
        for (int i = 0; i < manager.hand.Length; i++)
        {
            manager.drawOnScreen(i, null);
        }
    }


    public bool updateStats(Type type, int power, int cost)
    {
        if(type == Type.OPERATOR)
        {
            operator_canvas.enabled = true;
            operator_canvas.GetComponent<Operator>().addCard(power);
            return true;
        }


        if(PLAYER_ENERGY - cost >= 0)
        {
            if (type == Type.COOLING)
            {
                if(HEAT <= 0)
                {
                    return false;
                }

                HEAT -= 0.1F * power;
                if(HEAT < 0)
                {
                    HEAT = 0;
                    return true;
                }
            }
            else if (type == Type.ENERGY)
            {
                PLAYER_ENERGY += power;
                ui_manager.setEnergy(PLAYER_ENERGY);
            }
            else if(type == Type.POWER)
            {
                bool consumed = false;
                for (int i = 0; i < power; i++)
                {
                    if(nextFuel != FuelGrade.NONE)
                    {
                        generatePower(1, nextFuel);
                        nextFuel = FuelGrade.NONE;
                        ui_manager.grade = nextFuel;
                        ui_manager.count -= 1;
                        consumed = true;
                    }

                    if (fuelStorage.Count > 0)
                    {
                        nextFuel = fuelStorage.Dequeue();
                        ui_manager.grade = nextFuel;
                    }
                    else
                    {
                        return consumed;
                    }
                }
            }else if(type == Type.DRAW)
            {
                manager.drawFromBank(power);
            }
            PLAYER_ENERGY -= cost;
            ui_manager.setEnergy(PLAYER_ENERGY);
            return true;
        }
        return false;
    }

    public bool updateStatsBypass(Type type, int power)
    {
        if (type == Type.OPERATOR)
        {
            operator_canvas.enabled = true;
            operator_canvas.GetComponent<Operator>().addCard(power);
            return true;
        }


        if (type == Type.COOLING)
        {
            HEAT -= 0.1F * power;
            if (HEAT < 0)
            {
                HEAT = 0;
                return true;
            }
        }
        else if (type == Type.ENERGY)
        {
            PLAYER_ENERGY += power;
            ui_manager.setEnergy(PLAYER_ENERGY);
        }
        else if (type == Type.POWER)
        {
            for (int i = 0; i < power; i++)
            {
                if (nextFuel != FuelGrade.NONE)
                {
                    generatePower(1, nextFuel);
                }
            }
        }
        else if (type == Type.DRAW)
        {
            manager.drawFromBank(power);
        }
        //PLAYER_ENERGY -= cost;
        return true;
    }

    public bool updateFuel(FuelGrade grade, int amount, int cost)
    {
        if (PLAYER_ENERGY - cost >= 0)
        {
            if(fuelStorage.Count < maxFuelRod)
            {
                for(int i = 0; i < amount; i++)
                {
                    if(nextFuel != FuelGrade.NONE)
                    {
                        fuelStorage.Enqueue(grade);
                    }
                    else
                    {
                        nextFuel = grade;
                    }
                    ui_manager.count += 1;
                    ui_manager.grade = nextFuel;
                }
                PLAYER_ENERGY -= cost;
                ui_manager.setEnergy(PLAYER_ENERGY);
                return true;
            }
        }
        return false;
    }
    public bool updateFuelBypass(FuelGrade grade, int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            if (nextFuel != FuelGrade.NONE)
            {
                fuelStorage.Enqueue(grade);
            }
            else
            {
                nextFuel = grade;
                ui_manager.grade = grade;
            }
            ui_manager.count += 1;
        }
        return true;
    }


    private void Update()
    {
        ui_manager.temperaturePercent = Mathf.RoundToInt(this.HEAT * 100.0F);

        if(HEAT >= 1.0F)
        {
            lose(UIManager.death_type.MELTDOWN);
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    public void endShift()
    {
        POWER -= DAILY_QUOTA;
        if(POWER < 0)
        {
            DAYS_SINCE_POWER += 1;
            ui_manager.setDaysWithoutPower(DAYS_SINCE_POWER);
            POWER = 0;
        }
        else {
            DAYS_WITH_POWER += 1;
        }
        ui_manager.setPower(POWER);

        if (DAYS_SINCE_POWER > 5)
        {
            lose(UIManager.death_type.NO_POWER);
        }
        DAILY_QUOTA = Random.Range(2,5);
        quota_text.text = "QUOTA: " + DAILY_QUOTA + " POWER";
        PLAYER_ENERGY = 3;
        ui_manager.setEnergy(PLAYER_ENERGY);
        manager.resetBank();
        highscore.text = "Days Providing Power: " + DAYS_WITH_POWER;
    }

    public void lose(UIManager.death_type type)
    {
        if (!lost)
        {
            lost = true;
            if (highScore < DAYS_WITH_POWER)
            {
                highScore = DAYS_WITH_POWER;
                PlayerPrefs.SetInt("highscore", highScore);
            }
            ui_manager.setLose(type, DAYS_WITH_POWER);
        }
    }

    public void generatePower(int rods_consumed, FuelGrade grade)
    {
        for (int i = 0; i < rods_consumed; i++)
        {
            if (fuelStorage.Count - 1 >= 0)
            {
                if (grade == FuelGrade.S)
                {
                    HEAT += 0.2F;
                }
                else if (grade == FuelGrade.A)
                {
                    HEAT += (0.2F + (Mathf.Round(Random.Range(-0.03F, 0.03F) * 100) / 100));
                }
                else if (grade == FuelGrade.B)
                {
                    HEAT += (0.2F + (Mathf.Round(Random.Range(-0.05F, 0.05F) * 100) / 100));
                }
                else if (grade == FuelGrade.C)
                {
                    HEAT += (0.2F + (Mathf.Round(Random.Range(-0.08F, 0.08F) * 100) / 100));
                }
                else if (grade == FuelGrade.D)
                {
                    HEAT += (0.2F + (Mathf.Round(Random.Range(-0.12F, 0.12F) * 100) / 100));
                }
                else if (grade == FuelGrade.F)
                {
                    HEAT += (0.1F + (Mathf.Round(Random.Range(-0.3F, 0.3F) * 100) / 100));
                }

                if (HEAT < 0.0F)
                {
                    HEAT = 0.0F;
                }


                if (HEAT >= 0.9F)
                {
                    POWER += 5;
                }
                else if (HEAT >= 0.7)
                {
                    POWER += 4;
                }
                else if (HEAT >= 0.5)
                {
                    POWER += 3;
                }
                else if (HEAT >= 0.3F)
                {
                    POWER += 2;
                }
                else if (HEAT >= 0.1F)
                {
                    POWER += 1;
                }
                ui_manager.setPower(POWER);
            }
        }
    }

    public enum Type
    {
        OPERATOR,
        COOLING,
        FUEL,
        ENERGY,
        POWER,
        DRAW
    }

    public enum FuelGrade
    {
        NONE,
        S,
        A,
        B,
        C,
        D,
        F
    }
}
