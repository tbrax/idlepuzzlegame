using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gridScript : MonoBehaviour
{
    // Start is called before the first frame update
    MainLogic main;
    int x;
    int y;
    bool setup = false;
    string type;

    void Start()
    {
        type = "";
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public int getY()
    {
        return y;
    }

    public bool matchMe(gridScript other)
    {
        if (other.getType() == type)
        {
            return true;
        }
        return false;
    }


    public int getX()
    {
        return x;
    }


    public void setType(string t, Sprite s)
    {
        type = t;
        transform.Find("Image").GetComponent<SpriteRenderer>().sprite = s;

    }


    public string getType()
    {
        return type;

    }

    public void setupMain(MainLogic m, int xx, int yy)
    {
        x = xx;
        y = yy;
        main = m;
        setup = true;
    }


    void OnMouseDown()
    {
        if (setup)
        {
            main.clickSpot(x, y);
        }
    }

    void OnMouseOver()
    {
        //If your mouse hovers over the GameObject with the script attached, output this message
        if (setup)
        {
            main.hoverSpot(x, y);
        }      
    }

    void OnMouseExit()
    {
        //The mouse is no longer hovering over the GameObject so output this message each frame
        //Debu.Log("Mouse is no longer on GameObject.");
    }

}
