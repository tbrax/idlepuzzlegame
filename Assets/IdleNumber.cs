using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class IdleNumber
{
    float value;
    int mag;
    int dec = 10;
    int decshow = 2;


    public IdleNumber(float v, int m)
    {
        value = v;
        mag = m;
    }


    string getSuffix()
    {
        return "x10^" + mag.ToString();
    }

    public string displayNumber()
    {
        float ds = Mathf.Pow(10, decshow);

        return value.ToString() + getSuffix();
    }

    public float getValue()
    {
        return value;
    }

    public int getMag()
    {
        return mag;
    }

    public IdleNumber copy()
    {
        return new IdleNumber(value, mag);
    }

    public void set(IdleNumber other)
    {
        value = other.getValue();
        mag = other.getMag();
    }


    public bool greater(IdleNumber other)
    {
        if (mag > other.getMag())
        {
            return true;
        }

        if (mag == other.getMag() && value > other.getValue())
        {
            return true;
        }

        return false;
    }

    public bool equal(IdleNumber other)
    {
        if (other.getValue() == value && other.getMag() == mag)
        {
            return true;
        }


        return false;
    }

    public bool greaterEqual(IdleNumber other)
    {
        if (greater(other))
        {
            return true;
        }

        if (equal(other))
        {
            return true;
        }
        return false;
    }


    public void checkMag()
    {
        while (value >= 1000)
        {
            value = value / 1000;
            mag += 3;
        }

        while (value < 1 && value > 0 && mag >= 3)
        {
            if (mag >= 3)
            {
                value = value * 1000;
                mag -= 3;
            }
        }

        while (value > -1 && value < 0 && mag >= 3)
        {
            if (mag >= 3)
            {
                value = value * 1000;
                mag -= 3;
            }
        }


    }

    public void plus_equal(IdleNumber other)
    {

        IdleNumber greater = copy();
        IdleNumber less = other.copy();

        if (other.getMag() > getMag())
        {
            IdleNumber temp = greater;
            greater = less;
            less = temp;
        }

        float ds = Mathf.Pow(10, dec);
        float lv = less.getValue();

        for (int i = 0; i < (greater.getMag() - less.getMag()); i++)
        {
            lv = lv / 10;
            lv = Mathf.Round(lv * ds) / ds;
        }

        value = (greater.getValue() + lv);
        mag = greater.getMag();
        checkMag();
    }

    public void mult_equal(IdleNumber other)
    {
        value = value * other.getValue();
        mag = mag + other.getMag();
        checkMag();
    }

    public void mult_equal(float o)
    {
        IdleNumber i = new IdleNumber(o, 0);

        mult_equal(i);
    }

    public void sub_equal(IdleNumber other)
    {
        IdleNumber temp = other.copy();
        temp.mult_equal(new IdleNumber(-1, 0));
        plus_equal(temp);
    }



}
