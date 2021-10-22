using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainLogic : MonoBehaviour
{

    List<gridScript> gridScripts;
    // Start is called before the first frame update

    public Transform emptygridScript;

    public Transform grid;

    public Transform buildingBuyGUI;

    public List<Sprite> ImageSprites;

    void Start()
    {
        setupGame();


    }


    List<string> types = new List<string>();


    SaveData saved;

    const int min_x = 1;
    const int max_x = 9;
    const int min_y = 1;
    const int max_y = 9;

    float fallTimer = 0;
    float explodeTimer = 0;
    float fallAnimTimer = 0;
    const float fallTimerMax = 0.5f;
    const float explodeTimerMax = 0.5f;
    const float fallAnimTimerMax = 0.5f;
    bool wantWaitExplode = false;
    bool wantWaitFall = false;
    bool wantWaitFallAnim = false;

    const float updateInterval = 0.2f;
    float updateTimer = 0;

    const float gridFallMovement = 0.5f;

    public Transform moneyText;

    IdleNumber money;

    List<Buildings> builds;

    int costBuyAmt = 1;

    List<gridScript> needFall;

    List<gridScript> totalExplode;

    int comboNum = 0;

    void setupGame()
    {
        setupBuildings();
        setupMoney();
        makeSave();
        setupEmptyGrid();
        blankGrid();
        makeTypes();
        randomizeGrid();
        resetAllGUI();

    }


    void setupBuildings()
    {

        builds = new List<Buildings>();
        
        Buildings b0 = new Buildings("Red");
        builds.Add(b0);
        Buildings b1 = new Buildings("Orange");
        builds.Add(b1);
        Buildings b2 = new Buildings("Yellow");
        builds.Add(b2);
        Buildings b3 = new Buildings("Green");
        builds.Add(b3);
        Buildings b4 = new Buildings("Blue");
        builds.Add(b4);
        Buildings b5 = new Buildings("Indigo");
        builds.Add(b5);
        Buildings b6 = new Buildings("Violet");
        builds.Add(b6);
        Buildings b7 = new Buildings("White");
        builds.Add(b7);
        Buildings b8 = new Buildings("Black");
        builds.Add(b8);
        Buildings b9 = new Buildings("Grey");
        builds.Add(b9);
        Buildings b10 = new Buildings("Brown");
        builds.Add(b10);
        Buildings b11 = new Buildings("Pink");
        builds.Add(b11);

    }


    IdleNumber getTotalValue()
    {
        IdleNumber i = new IdleNumber(0, 0);
        foreach(Buildings b in builds)
        {
            i.plus_equal(b.getValue());
        }
        return i;
    }

    Buildings getBuildingsByName(string s)
    {
        foreach(Buildings b in builds)
        {
            if (b.getName() == s)
            {
                return b;
            }
        }

        return null;
    }


    IdleNumber getValueByColor(string s)
    {
        return getBuildingsByName(s).getValue();
    }

    void setupMoney()
    {
        money = new IdleNumber(0, 0);
    }

    void makeSave()
    {
        saved = new SaveData();
        
    }

    bool checkCost(IdleNumber n)
    {

        return money.greaterEqual(n);
    }

    void subMoney(IdleNumber n)
    {
        money.sub_equal(n);
    }

    bool doBuy(IdleNumber n)
    {

        if (checkCost(n))
        {
            subMoney(n);
            return true;
        }

        return false;
    }

    void resetAllGUI()
    {
        resetBuildingBuyGUI();
    }

    void resetBuildingBuyGUI()
    {
        foreach (Buildings b in builds)
        {
            string c = b.getName();

            Transform t = buildingBuyGUI.Find("BuildingBuy" + c);
            if (t)
            {
                string amt = b.getAmount().ToString();
                string n = b.getNextCost(costBuyAmt).displayNumber();
                t.Find("Click").Find("Amount").GetComponent<TMPro.TextMeshProUGUI>().text = amt;
                t.Find("Click").Find("Cost").GetComponent<TMPro.TextMeshProUGUI>().text = n;
            }

        }
    }

    public void clickBuildingBuy(string color)
    {
        Buildings b = getBuildingsByName(color);


        
        if (doBuy(b.getNextCost(costBuyAmt)))
        {
            b.addAmount(costBuyAmt);
        }

        resetBuildingBuyGUI();
    }

    int getNumBuilding(string color)
    {
        return 0;
    }

    IdleNumber getBuildingCost(string color)
    {
        IdleNumber n = new IdleNumber(0, 0);
        return n;
    }

    public void saves()
    {
        saved.save(money, builds);
    }

    public void loads()
    {
        saved = saved.load();

        money.set(saved.getMoney());
        List<Buildings> bs = saved.getBuilds();

        for(int i = 0; i < bs.Count; i++)
        {
            for (int j = 0; j < builds.Count; j++)
            {
                if (bs[i].getName() == builds[j].getName())
                {
                    builds[j] = bs[i];
                }
            }

        }
        resetBuildingBuyGUI();
    }

    public void addMoney(IdleNumber i)
    {
        money.plus_equal(i);
    }

    public void displayMoney()
    {
        moneyText.GetComponent<TMPro.TextMeshProUGUI>().text = money.displayNumber();
    }

    void makeTypes()
    {
        types.Add("Red");
        types.Add("Orange");
        types.Add("Yellow");
        types.Add("Green");
        types.Add("Blue");
        types.Add("Indigo");
        types.Add("Violet");
    }

    gridScript heldGrid;
    bool haveHeldGrid = false;


    void swapGridSpots(gridScript a, gridScript b)
    {
        string type = a.getType();
        setSpot(a, b.getType());
        setSpot(b, type);

        checkSpots();
    }

    public void hoverSpot(int x, int y)
    {
        //Debu.Log(string.Format("{0}{1}",x,y));
    }

    public void clickSpot(int x, int y)
    {
        if (haveHeldGrid)
        {
            if (heldGrid)
            {
                gridScript g = getSpotScript(x, y);

                swapGridSpots(heldGrid, g);
                haveHeldGrid = false;
            }
        }
        else
        {
            haveHeldGrid = true;
            heldGrid = getSpotScript(x, y);
        }


        //Debu.Log(string.Format("{0}{1}",x,y));
        //getSpotScript(x, y).setType("Yellow", ImageSprites[0]);
        //setSpot(x,y, "Yellow");
        //checkSpots();
    }

    List<string> validTypes()
    {
        return types;
    }

    int randNum(int mi, int ma)
    {
        return  Random.Range(mi, ma);
    }

    int randIndex<T>(List<T> l)
    {
        return randNum(0, l.Count - 1);
    }

    T randInList<T>(List<T> l)
    {
        return l[randIndex(l)];
    }


    string randomType()
    {
        return randInList(types);
    }


    void randomizeGridSpace(gridScript g)
    {
        setSpot(g, randomType());
    }

    void randomizeGrid()
    {

        foreach (gridScript f in gridScripts)
        {
            randomizeGridSpace(f);
        }

        checkSpots();

    }


    Sprite getSpriteFromType(string t)
    {
        switch (t)
        {
            case "Red":
                return ImageSprites[1];
            case "Orange":
                return ImageSprites[2];
            case "Yellow":
                return ImageSprites[3];
            case "Green":
                return ImageSprites[4];
            case "Blue":
                return ImageSprites[5];
            case "Indigo":
                return ImageSprites[6];
            case "Violet":
                return ImageSprites[7];
            case "":
                return ImageSprites[0];
            default:
                return ImageSprites[0];
        } 
    }



    public void setSpot(gridScript g, string t)
    {
        g.setType(t, getSpriteFromType(t));
        
    }

    void resetSpot(gridScript g)
    {
        setSpot(g, "");
    }


    public void setSpot(int x, int y, string t)
    {
        getSpotScript(x, y).setType(t, getSpriteFromType(t));
        
    }




    void fallGrid(gridScript g)
    {
        if (g.getY() + 1 <= max_y)
        {
            gridScript above = getSpotScript(g.getX(), g.getY() + 1);

            string s = above.getType();
            setSpot(g, s);
            resetSpot(above);
        }
        else
        {
            setSpot(g, randomType());
        }

    }

    void fallGridAnimation(gridScript g, float t)
    {
        g.transform.Find("Image").Translate(0, -gridFallMovement * t / fallAnimTimerMax,0);

    }

    void fallGridAnimation(float t)
    {
        foreach (gridScript f in needFall)
        {

            fallGridAnimation(f, t);
            
        }


    }

    void moveFallUp(List<gridScript> g)
    {
        foreach(gridScript gs in g)
        {
            gs.transform.Find("Image").Translate(0, gridFallMovement, 0);
        }
    }

    void startFallAnim()
    {
        Debug.Log("setup");
        
        fallDown();

    }

    void endFallAnim()
    {

        foreach (gridScript gs in needFall)
        {
            gs.transform.Find("Image").localPosition = new Vector3(0, 0, 0);
        }
    }

  

    void fallDown()
    {

        bool wantFall = true;
        float waitTime = 0f;
        float waitTimeAdd = 0.5f;
        bool didFall = false;
        while (wantFall)
        {
            wantFall = false;
            foreach (gridScript f in gridScripts)
            {
                if (f.getType() == "")
                {
                    needFall.Add(f);
                    wantFall = true;
                    fallGrid(f);
                }
            }
            didFall = true;
            waitTime += waitTimeAdd;
        }
        if (didFall)
        {
            setupWaitFall();
        }

        moveFallUp(needFall);
    }

    void blankGrid()
    {
        foreach (gridScript f in gridScripts)
        {
            resetSpot(f);
        }
    }

    void resetSpot(int x, int y)
    {
        setSpot(x, y, "");
    }

    void checkSpots()
    {
        List<List<gridScript>> a = checkAllMatches();
        dealWithMatches(a);
        
    }




    void resetHoverSpots()
    {

    }

    Transform getSpotTransform(int x, int y)
    {
        return  grid.Find("Grids").Find(y.ToString()).Find(x.ToString()).Find("GridSpot");
    }

    gridScript getSpotScript(int x, int y)
    {
        return getSpotTransform(x, y).GetComponent<gridScript>();
    }


    gridScript getSpot(int x, int y)
    {
        foreach(gridScript f in gridScripts)
        {
            if (f.getX() == x && f.getY() == y)
            {
                return f;
            }
        }

        return null;
    }

    List<gridScript> checkLineForSpot(gridScript f, int dx, int dy)
    {
        int x = f.getX();
        int y = f.getY();
        bool match = true;
        List<gridScript> matchTouch = new List<gridScript>();
        while(match)
        {
            x += dx;
            y += dy;
            if (x < min_x || x > max_x || y < min_y || y > max_y)
            {
                match = false;
            }
            if (match)
            {
                gridScript f1 = getSpot(x, y);
                if (f.matchMe(f1))
                {
                    matchTouch.Add(f1);
                }
                else
                {
                    match = false;
                }
            }
        }
        return matchTouch;
    }

    List<List<gridScript>> checkSpot(gridScript f)
    {
        List<gridScript> left = checkLineForSpot(f, -1, 0);
        List<gridScript> right = checkLineForSpot(f, 1, 0);
        List<gridScript> up = checkLineForSpot(f, 0, 1);
        List<gridScript> down = checkLineForSpot(f, 0, -1);

        List<gridScript> row = new List<gridScript>();
        List<gridScript> col = new List<gridScript>();

        foreach(gridScript f0 in left)
        {
            if (!row.Contains(f0))
            {
                row.Add(f0);
            }
        }
        foreach (gridScript f0 in right)
        {
            if (!row.Contains(f0))
            {
                row.Add(f0);
            }
        }
        row.Add(f);

        foreach (gridScript f0 in up)
        {
            if (!col.Contains(f0))
            {
                col.Add(f0);
            }
        }
        foreach (gridScript f0 in down)
        {
            if (!col.Contains(f0))
            {
                col.Add(f0);
            }
        }
        col.Add(f);

        List<List<gridScript>> ret = new List<List<gridScript>>();
        ret.Add(row);
        ret.Add(col);

        return ret;
    }

    List<List<gridScript>> checkAllMatches()
    {
        List<List<gridScript>> allMatches = new List<List<gridScript>>();

        foreach (gridScript f in gridScripts)
        {
            List<List<gridScript>> r = checkSpot(f);

            foreach (List<gridScript> r0 in r)
            {
                allMatches.Add(r0);
            }
        }

        return allMatches;
        //dealWithMatches(allMatches);
    }

    static int SortBySpotLength(List<gridScript> p1, List<gridScript> p2)
    {
        return p1.Count.CompareTo(p2.Count);
    }


    float getMultFromLineSize(float line)
    {
        switch (line)
        {
        case 3:
            return 1;

        default:
                return 1;
        }

    }

    float multFromCombo()
    {


        return 1.0f + comboNum;
    }

    void giveSingleScore(gridScript g, float lineMult, string color)
    {
        IdleNumber add = new IdleNumber(1, 0);

        add.plus_equal(getValueByColor(color));

        float lines = getMultFromLineSize(lineMult);
        add.mult_equal(lines);
        add.mult_equal(multFromCombo());

        addMoney(add);
    }

    void giveScore(List<gridScript> l, string color)
    {
        int lineSize = l.Count;
        foreach (gridScript g in l)
        {
            giveSingleScore(g,lineSize, color);
        }
    }

    void explodeLine(List<gridScript> l)
    {

        


        if (l.Count < 1)
        {
            return;
        }
        string s = l[0].getType();
       
        foreach(gridScript g in l)
        {
            if (g.getType() != s)
            {
                return;
            }
        }


        giveScore(l, s);

        foreach (gridScript g in l)
        {
            if (!totalExplode.Contains(g))
            {
                totalExplode.Add(g);
            }

            
        }


    }




    void dealWithMatches(List<List<gridScript>> matches)
    {
        totalExplode = new List<gridScript>();

        bool haveMatch = false;
        matches.Sort(SortBySpotLength);
        for (int i = matches.Count - 1; i >= 0; i--)
        {
            if (matches[i].Count >= 3)
            {
                haveMatch = true;
                string type = matches[i][0].getType();

                if (type != "")
                {
                    explodeLine(matches[i]);
                }
                
            }
            
        }
        foreach(gridScript g in totalExplode)
        {
            resetSpot(g);
        }
        

        if (haveMatch)
        {
            comboNum += 1;
            setupWaitExplode();          
        }
        else
        {
            comboNum = 0;
        }
        

    }


    void checkLinesMatch()
    {


        for (int i = min_x; i <= max_x; i++)
        {

        }

    }


    void setupEmptyGrid()
    {
        gridScripts = new List<gridScript>();
        for (int i = min_y; i <= max_y; i++)
        {
            for (int j = min_x; j <= max_x; j++)
            {
                //string s = string.Format("{0}/{1}/GridSpot", i, j);


                gridScript g = getSpotScript(j, i);

                gridScripts.Add(g);
                //Transform g =  grid.Find("Grids").Find(i.ToString()).Find(j.ToString()).Find("GridSpot");
                //Transform g = getSpotTransform(j, i);
                //g.GetComponent<gridScript>().setupMain(this,j,i);
                g.setupMain(this, j, i);
            }
        }

    }

    // Update is called once per frame


    void setupWaitExplode()
    {
        wantWaitExplode = true;
        explodeTimer = explodeTimerMax;
    }

    void setupWaitFall()
    {
        wantWaitFall = true;
        fallTimer = fallTimerMax;

    }

    void setupWaitFallAnim()
    {
        needFall = new List<gridScript>();
        wantWaitFallAnim = true;
        fallAnimTimer = fallAnimTimerMax;
        startFallAnim();
    }

    void doWaitExplode()
    {
        wantWaitExplode = false;
        setupWaitFallAnim();
        //fallDown();
    }

    void doWaitFall()
    {
        wantWaitFall = false;
        checkSpots();
        
    }

    void doWaitFallAnim()
    {

        wantWaitFallAnim = false;
        endFallAnim();

    }

    void checkWaitExplode(float t) 
    {

        if (wantWaitExplode)
        {
            explodeTimer -= t;
            if (explodeTimer <= 0)
            {
                doWaitExplode();
            }
        }
    }

    void checkWaitFall(float t)
    {
        if (wantWaitFall)
        {
            fallTimer -= t;
            if (fallTimer <= 0)
            {
                doWaitFall();
            }
        }
    }

    void checkWaitFallAnim(float t)
    {
        if (wantWaitFallAnim)
        {
            fallAnimTimer -= t;
            fallGridAnimation(t);

            if (fallAnimTimer <= 0)
            {
                doWaitFallAnim();
            }
        }
    }

    void checkTimes(float t)
    {
        checkWaitExplode(t);
        checkWaitFall(t);
        checkWaitFallAnim(t);
    }


    void doUpdates()
    {
        displayMoney();
    }

    void checkUpdates(float t)
    {

        updateTimer += t;

        if (updateTimer >= updateInterval)
        {
            updateTimer = 0;
            doUpdates();
        }

    }

    void Update()
    {
        var fps = 1.0f / Time.deltaTime;
        if (fps < 1)
        {
            Debug.Break();
        }

        float t = Time.deltaTime;
        checkTimes(t);

        checkUpdates(t);
    }
}
