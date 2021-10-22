using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Buildings
{
    int amount;
    IdleNumber mult0;
    IdleNumber mult1;
    string name;

    float crit_chance;
    IdleNumber crit_mult;

    IdleNumber card_mult;
    float card_haste;

    const float baseCost = 10;

    public Buildings(string n)
    {
        name = n;
        amount = 0;
        mult0 = new IdleNumber(1, 0);
        mult1 = new IdleNumber(1, 0);
    }


    public string getName()
    {
        return name;
    }

    public int getAmount()
    {
        return amount;
    }

    public IdleNumber getValue()
    {
        IdleNumber i = new IdleNumber(amount,0);
        i.mult_equal(mult0);
        i.mult_equal(mult1);
        return i;
    }

    public void addAmount(int i)
    {
        amount += i;
    }

    public IdleNumber getCost(int a)
    {
        IdleNumber i = new IdleNumber(baseCost, 0);

        //IdleNumber ai = new IdleNumber(a, 0);

        return i;
    }





    public IdleNumber getNextCost(int a)
    {
        IdleNumber ii= new IdleNumber(0, 0);

        for (int i = 1; i <= a; i++)
        {
            IdleNumber t = getCost(amount + i);
            ii.plus_equal(t);
        }
        return ii;
    }

}
