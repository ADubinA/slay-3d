using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;

public class hexMap : MonoBehaviour
{
    public int height=30;
    public int width=30;
    public float hexScale=0.95f;
    public hexGround hexPreFab;
    public City cityPreFab;
    public float generateProbability = 0.2f;
    public Player[] playersList;
    private hexGround[,] cells;
    private int numOfLandHex = 0;
    private List<City> cities;
    private readonly Vector2Int[] directions = {
        new Vector2Int(0,1),
        new Vector2Int(1,0),
        new Vector2Int(-1,1),
        new Vector2Int(0,-1),
        new Vector2Int(-1,0),
        new Vector2Int(-1,-1)
    };
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
        updateCities();
    }
    void updateCities()
    {
        cities = new List<City>();
        // create a list of indeces that represent the entire land
        Dictionary<int,Vector2Int> index_dict = new Dictionary<int,Vector2Int >();
        for (int h = 1; h < height - 1; h++)
        {
            for (int w = 1; w < width - 1; w++)
            {
                if (cells[h, w].isLand)
                {
                    index_dict.Add(h*width+w,new Vector2Int(h, w));
                }
            }
        }

        // loop on the collection of hex points
        while (index_dict.Count>0)
        {
            // take a first, random value and create a list of hexpoints for the city
            Vector2Int starting_point;
            List<Vector2Int> tmp_city_hexs = new List<Vector2Int>();
            List<Vector2Int> unprocessed_city_hexs = new List<Vector2Int>();

            //append the starting point and remove it from the dictionary
            unprocessed_city_hexs.Add(index_dict.ElementAt(0).Value);
            index_dict.Remove((unprocessed_city_hexs[0]).x * width + (unprocessed_city_hexs[0]).y);

            // for every good neighbor, add it to the city and remove from the collection
            while (unprocessed_city_hexs.Count > 0)
            {
                starting_point = unprocessed_city_hexs[0];
                tmp_city_hexs.Add(starting_point);
                unprocessed_city_hexs = unprocessed_city_hexs.Skip(1).ToList();

                foreach (Vector2Int dir in directions)// only look up, not down
                {
                    int neighbor_key = (starting_point + dir).x * width + (starting_point + dir).y;
                    if (checkNeighborIsSamePlayer(starting_point,dir) && index_dict.ContainsKey(neighbor_key))
                    {
                        unprocessed_city_hexs.Add(starting_point + dir);
                        index_dict.Remove(neighbor_key);
                    }
                }

            }

            //convert the points to a proper city
            List<hexGround> tmp_hexground = new List<hexGround>();
            //cities.Add(Instantiate(cityPreFab));
            foreach(Vector2Int hex_point in tmp_city_hexs)
            {
                tmp_hexground.Add(cells[hex_point.x, hex_point.y]);
            }
            tmp_hexground[UnityEngine.Random.Range(0, tmp_hexground.Count)].setUnit(Instantiate(cityPreFab));
            //cities[cities.Count - 1].addGround(tmp_hexground);
        }
    }
    bool checkNeighborIsSamePlayer(Vector2Int hex, Vector2Int direction)
    {
        Player current_hex_player = cells[hex.x, hex.y].ownByPlayer;

        Vector2Int neighbor = new Vector2Int(0, 1);
        Player neighbor_player = cells[(hex + direction).x, (hex + direction).y].ownByPlayer;
        if (neighbor_player == current_hex_player)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    void CreateCell(int x, int z)
    {
        Vector3 position;
        position.x = (x + z * 0.5f - z / 2);
        position.y = 0f;
        position.z = z *0.866025404f;

        hexGround cell = cells[x,z] = Instantiate(hexPreFab);
        cell.setLand(false);
        cell.transform.SetParent(transform, false);
        cell.transform.localPosition = position;
        cell.transform.localScale = cell.transform.localScale * hexScale;
    }
    // generate landmass from the water
    void GenerateLandMass()
    {
        cells[1, 1].setLand(true);
        for (int h = 1; h < height-1; h++)
        {
            for (int w = 1; w < width-1; w++)
            {
                float prob  = UnityEngine.Random.Range(0.0f, 1.0f);
                if (prob <= generateProbability && (cells[h - 1, w].isLand || cells[h, w - 1].isLand))
                {
                    cells[h, w].setLand(true);
                    numOfLandHex++;
                }
                else if (w!=1 && h!=1)
                {

                    cells[h, w].setLand(false);
                }


            }
        }
    }

    //randomly assign player with equal landmass
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
