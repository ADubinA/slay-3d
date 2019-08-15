using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hexGround : MonoBehaviour
{
    public bool isLand;
    public Player ownByPlayer;
    public hexableUnit unit;
    private Vector2Int index;
    // Start is called before the first frame update
    void Start()
    {
        if (unit != null)
        {
            unit.transform.position = transform.position;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void setLand(bool isLand)
    {
        if (isLand)
        {
            GetComponent<Renderer>().material.color = Color.red;


        }
        else
        {
            GetComponent<Renderer>().material.color = Color.blue;


        }
        this.isLand = isLand;

    }
    public void setPlayer(Player player)
    {
        ownByPlayer = player;
        GetComponent<Renderer>().material.color = player.playerColor;
    }
    public void setIndex(Vector2Int value)
    {
        index = value;
    }
    public Vector2Int getIndex()
    {
        return index;
    }
    public void setUnit(hexableUnit unit)
    {
        this.unit = unit;
    }
}
