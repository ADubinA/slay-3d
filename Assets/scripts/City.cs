using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class City : hexableUnit
{
    private int cityIncome;
    private int cityTotalMoney;
    private List<hexGround> cityGrounds = new List<hexGround>();
    private hexGround capital_hex;
    // Start is called before the first frame update
    void Start()
    {
        costToCreate = 0;
        income = 1;
        defence = 2;

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void calculateIncome()
    {
        cityIncome = 0;
        foreach(hexGround hex in cityGrounds)
        {
            cityIncome += hex.unit.getIncome();
        }
    }
    // adds hexgrounds to a city. will not do the calculation.
    public void addGround(List<hexGround> ground)
    {
        foreach (hexGround g in ground)
        {
            cityGrounds.Add(g);
        }
        //cityGrounds.
        //cityGrounds.Add(ground);
        //cityGrounds.Sort((x, y) => x.getIndex().sqrMagnitude.CompareTo(y.getIndex().sqrMagnitude));


    }

}
