using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum FloorType {Space, Damaged, Floor};

public enum Direction { North, East, South, West, Center };

public class Room : MonoBehaviour
{

    Grid grid;
    [HideInInspector]
    public Vector2Int pos;

    [HideInInspector]
    public FloorType floorType;

    [HideInInspector]
    public bool edge, spawn;


    Dictionary<Direction, Room> neighbors = new Dictionary<Direction, Room>();
    Dictionary<Direction, Wall> walls = new Dictionary<Direction, Wall>();

    [HideInInspector]
    public Item item;

    public GameObject FloorPrefab, WallPrefab;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Initialize(int x, int y, Grid grid, bool edge, bool spawn, FloorType floorType)
    {
        this.grid = grid;
        pos = new Vector2Int(x, y);
        this.edge = edge;
        this.spawn = spawn;
        this.floorType = floorType;
    }

    public void InitializeWalls()
    {
        InitializeNeighbors();

        if (edge || spawn)
            return;

        if (floorType == FloorType.Space)
            return;

        for (int i = 0; i < 4; i++)
        {
            Room neighbor = neighbors[(Direction)i];
            if (neighbor.floorType == FloorType.Space)
            {
                Wall wall = null;
                walls[(Direction)i] = wall; //Instantiate WallPrefab and get a ref to it
                neighbor.CreateObverseWall((Direction)i, wall);
            }
            
        }

        foreach (Room room in neighbors.Values)
            if (room.spawn)
                return;

        for (int i = 0; i < 3; i++) 
        {
            Direction wall_dir = (Direction)UnityEngine.Random.Range(0, 4);
            if (walls[wall_dir])
                continue;
            bool is_door = UnityEngine.Random.value < grid.door_chance;
            Wall wall = null;
            if (is_door)
                wall = null; // Instantiate door
            else
                wall = null; // Instantiate wall
            walls[wall_dir] = wall;
            neighbors[wall_dir].CreateObverseWall(wall_dir, wall);
        }
    }

    void InitializeNeighbors()
    {
        for (int i = 0; i < 4; i++)
        {
            walls[(Direction)i] = null;
            Vector2Int neighbor_pos = DirectionTransform.dir_to_trans[(Direction)i];
            try
            {
                neighbors[(Direction)i] = grid.rooms[neighbor_pos.x, neighbor_pos.y];
            }
            catch (Exception e)
            {
                neighbors[(Direction)i] = this;
            }
        }
    }

    void CreateObverseWall(Direction dir, Wall wall)
    {
        Direction wall_dir = DirectionTransform.Reverse(dir);
        walls[wall_dir] = wall;
    }
}
