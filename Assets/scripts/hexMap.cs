using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class hexMap : MonoBehaviour
{
    public int height=30;
    public int width=30;
    public float hexScale=0.95f;
    public hexGround hexPreFab;
    public float generateProbability = 0.2f;
    public Player[] playersList;
    private hexGround[,] cells;
    private int numOfLandHex = 0;
    void Start()
    {
        cells = new hexGround[height,width];

        for (int z = 0; z < height; z++)
        {
            for (int x = 0; x < width; x++)
            {
                CreateCell(x, z);
            }
        }
        GenerateLandMass();
        subdivideLandMass();
    }

    void CreateCell(int x, int z)
    {
        Vector3 position;
        position.x = (x + z * 0.5f - z / 2);
        position.y = 0f;
        position.z = z *0.866025404f;

        hexGround cell = cells[x,z] = Instantiate(hexPreFab);
        cell.isLand = false;
        cell.transform.SetParent(transform, false);
        cell.transform.localPosition = position;
        cell.transform.localScale = cell.transform.localScale * hexScale;
    }
    void GenerateLandMass()
    {
        cells[1, 1].setLand(true);
        for (int h = 1; h < height-1; h++)
        {
            for (int w = 1; w < width-1; w++)
            {
                float prob  = UnityEngine.Random.Range(0.0f, 1.0f);
                if(prob>generateProbability&&(cells[h-1,w].isLand|| cells[h, w-1].isLand))
                {
                    cells[h, w].setLand(true);
                    numOfLandHex++;
                }

                    
            }
        }
        

    }
    void subdivideLandMass()
    {
        // set variables
        int num_of_players = playersList.Length;
        int num_of_land_per_player =(int)Math.Ceiling((float)numOfLandHex / (float)num_of_players)+1;
        System.Random rand = new System.Random();
        // set a list of distribution, this will determine how the random land assignment will work
        List<int> player_distribution_list = new List<int>();
        int loop_player_index = 0; 
        for(int i = 1; i<numOfLandHex+2;i++)
        {
            if(i%num_of_land_per_player==0){loop_player_index++;}

            player_distribution_list.Add(loop_player_index);
        }

        // loop on every landmass
        for (int h = 1; h < height - 1; h++)
        {
            for (int w = 1; w < width - 1; w++)
            {
                if (cells[h, w].isLand)
                {
                    //random select from the distribution list, set and delete the item.
                    int random_index = rand.Next(player_distribution_list.Count);
                    cells[h, w].setPlayer(playersList[player_distribution_list[random_index]]);
                    player_distribution_list.RemoveAt(random_index);

                }

            }
        }

    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
