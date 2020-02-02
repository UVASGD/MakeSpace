using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum FloorType {Space, Damaged, Floor};

public enum Direction { North, East, South, West, Center };

public class Room : MonoBehaviour
{

    public static float ROOM_SIZE = 3;
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
    public GameObject item;

    public GameObject floor_prefab, damaged_prefab, wall_prefab, door_prefab;

    Dictionary<Direction, Transform> wall_pivots = new Dictionary<Direction, Transform>();

    // Start is called before the first frame update
    void Awake()
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
        switch(floorType)
        {
            case FloorType.Floor:
                Instantiate(floor_prefab, transform.position, Quaternion.identity).transform.parent = transform;
                break;
            case FloorType.Damaged:
                Instantiate(damaged_prefab, transform.position, Quaternion.identity).transform.parent = transform;
                break;
        }
        GetPivots();
    }

    void GetPivots()
    {
        wall_pivots[Direction.North] = transform.FindDeepChild("north_pivot");
        wall_pivots[Direction.East] = transform.FindDeepChild("east_pivot");
        wall_pivots[Direction.South] = transform.FindDeepChild("south_pivot");
        wall_pivots[Direction.West] = transform.FindDeepChild("west_pivot");
    }

    public void InitializeWalls()
    {
        InitializeNeighbors();

        if (edge)
            return;

        if (floorType == FloorType.Space)
            return;

        for (int i = 0; i < 4; i++)
        {
            Room neighbor = neighbors[(Direction)i];
            if (neighbor.floorType == FloorType.Space)
            {
                CreateWall(wall_prefab, (Direction)i);
            }
            
        }

        if (spawn)
            return;

        foreach (Room room in neighbors.Values)
            if (room.spawn)
            {
                print("AHAHAAH");
                return;
            }

        if (floorType == FloorType.Damaged)
            return;

        for (int i = 0; i < 3; i++) 
        {
            Direction wall_dir = (Direction)UnityEngine.Random.Range(0, 4);
            if (walls[wall_dir])
                continue;
            bool is_door = UnityEngine.Random.value < grid.door_chance;
            if (is_door)
                CreateWall(door_prefab, wall_dir);
            else
                CreateWall(wall_prefab, wall_dir);
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
                neighbors[(Direction)i] = grid.rooms[neighbor_pos.x+pos.x, neighbor_pos.y+pos.y];
            }
            catch (Exception e)
            {
                neighbors[(Direction)i] = this;
            }
        }
    }

    Wall CreateWall(GameObject wall, Direction dir)
    {
        walls[dir] = Instantiate(wall, wall_pivots[dir].position, Quaternion.identity).GetComponent<Wall>();
        walls[dir].transform.parent = grid.wall_holder;
        walls[dir].orientation = DirectionTransform.dir_to_orient[dir];
        neighbors[dir].CreateObverseWall(dir, walls[dir]);
        return walls[dir];
    }

    void CreateObverseWall(Direction dir, Wall wall)
    {
        Direction wall_dir = DirectionTransform.Reverse(dir);
        walls[wall_dir] = wall;
    }

    public void PopulateItem(GameObject item)
    {
        if (this.item)
            return;
        this.item = Instantiate(item); //additional parameters to put it in the right place n stuff
    }
}
