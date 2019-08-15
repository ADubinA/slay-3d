using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hexableUnit : MonoBehaviour
{
    public Player player;
    protected int defence = 0;// a unit must be of greater attack to get overtake the hexable unit
    protected int income = 0; // how much a unit gives to the city, can be negative.
    protected int costToCreate = -1; // how much does it cost to create the unit
    protected bool createable = false;
    private bool selected = false;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    void setSelected(bool option)
    {
        selected = option;
    }
    public int getIncome()
    {
        return income;
    }
}
