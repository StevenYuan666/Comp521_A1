using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using D;

public class MazeGenerator : MonoBehaviour
{
    public int row = 10;
    public int column = 6;
    public int width = 4;
    public GameObject walls, tile;
    public class Cell
    {
        // If the cell has been visited
        public bool Visited = false;
        // Check if the neighours have been visited
        public bool East = false;
        public bool South = false;
        public bool West = false;
        public bool North = false;
    }

    // A collection of cells formed a maze
    private Cell[] maze;
    // The visit history
    private int numOfVisited;
    private Stack<(int, int)> stack = new Stack<(int, int)>();
    private List<GameObject> previousCell = new List<GameObject>();
    // A random number generator
    System.Random rnd = new System.Random();


    // Start is called before the first frame update
    void Start()
    {
        // Initialize the maze, and draw the grid
        maze = new Cell[row * column];
        for(int i = 0; i < maze.Length; i++){
            maze[i] = new Cell();
        }
        int rand_x = rnd.Next(row);
        int rand_y = rnd.Next(column);
        stack.Push((rand_x, rand_y));
        maze[rand_y * row + rand_x].Visited = true;
        numOfVisited = 1;
        Initialize();
    }

    void Initialize()
    {
        for(int x = 0; x < row; x ++)
        {
            for(int y = 0; y < column; y ++)
            {
                for(int py = 0; py < width; py ++)
                {
                    for(int px = 0; px < width; px ++)
                    {
                        Vector3 v3 = new Vector3(x * (width + 1) + px, 0, y * (width + 1) + py);
                        Instantiate(tile, v3, Quaternion.identity);
                        Vector3 v3_1 = new Vector3(x * (width + 1) + px, 0, y * (width + 1) + width);
                        Vector3 v3_2 = new Vector3(x * (width + 1) + width, 0, y * (width + 1) + px);
                        Instantiate(walls, v3_1, Quaternion.identity);
                        Instantiate(walls, v3_2, Quaternion.identity);
                    }
                }
                Vector3 cornor = new Vector3((x+1) * (width + 1) - 1, 0, (y+1) * (width + 1) - 1);
                Instantiate(walls, cornor, Quaternion.identity);
            }
        }
        for (int i = 0; i < row * (width+1); i++)
        {
            Instantiate(walls, new Vector3(i, 0, (column - column)* (width + 1) - 1), Quaternion.identity);
        }
        for (int j = -1; j < column * (width + 1); j++)
        {
            Instantiate(walls, new Vector3((row - row) * (width + 1) - 1, 0, j), Quaternion.identity);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Call the trace back algorithm
        RB_Algorithm();
    }

    void RB_Algorithm()
    {
        List<int> neighbours = new List<int>();
        Func<int, int, uint> lookAt = (px, py) => (uint)((stack.Peek().Item1 + px) + (stack.Peek().Item2 + py) * row);

        if (numOfVisited < row * column)
        {
            // Check the north neighbour
            if (stack.Peek().Item2 > 0 && maze[lookAt(0, -1)].Visited == false)
                neighbours.Add(0);
            // Check the east neighbour
            if (stack.Peek().Item1 < row - 1 && maze[lookAt(+1, 0)].Visited == false)
                neighbours.Add(1);
            // Check the south neighbour
            if (stack.Peek().Item2 < column - 1 && maze[lookAt(0, +1)].Visited == false)
                neighbours.Add(2);
            // Check the west neighbour
            if (stack.Peek().Item1 > 0 && maze[lookAt(-1, 0)].Visited == false)
                neighbours.Add(3);

            // Check if there's any neighbors availiable
            if (neighbours.Count > 0)
            {
                // Choose a random neighbour
                int nextCellDir = neighbours[rnd.Next(neighbours.Count)];
                // North
                if (nextCellDir == 0){
                    maze[lookAt(0, -1)].Visited = true; maze[lookAt(0, -1)].South = true;
                    maze[lookAt(0, 0)].North = true;
                    stack.Push(((stack.Peek().Item1 + 0), (stack.Peek().Item2 - 1)));
                }
                // East
                if(nextCellDir == 1){
                    maze[lookAt(+1, 0)].Visited = true; maze[lookAt(+1, 0)].West = true;
                    maze[lookAt(0, 0)].East = true;
                    stack.Push(((stack.Peek().Item1 + 1), (stack.Peek().Item2 + 0)));
                }
                // South
                if(nextCellDir == 2){
                    maze[lookAt(0, +1)].Visited = true; maze[lookAt(0, +1)].North = true;
                    maze[lookAt(0, 0)].South= true;
                    stack.Push(((stack.Peek().Item1 + 0), (stack.Peek().Item2 + 1)));
                }
                // West
                if(nextCellDir == 3){
                    maze[lookAt(-1, 0)].Visited = true; maze[lookAt(-1, 0)].East = true;
                    maze[lookAt(0, 0)].West = true;
                    stack.Push(((stack.Peek().Item1 - 1), (stack.Peek().Item2 + 0)));
                }
                numOfVisited ++; 
            }
            else
            {
                // Trace back otherwise
                stack.Pop();
            }
        }
        if(D.Destroy.CountDestroyed > -1){
            // Draw one line whenever we destroyed a tree
            DrawEverything(process(D.Destroy.CountDestroyed));
        }
    } 

    // Handle the number of destroyed tree
    private int process(int x){
        if(x > 9){
            return 9;
        }
        return x;
    }

    void DrawEverything(int x)
    {
        // Draw Maze on the row specified
        for (int y = 0; y < column; y++)
        {
            for (int p = 0; p < width; p++)
            {
                
                Vector3 v3 = new Vector3(x * (width + 1) + p, 0, y * (width + 1) + width);
                Vector3 v3_2 = new Vector3(x * (width + 1) + width, 0, y * (width + 1) + p);
                if (maze[y * row + x].South && checkIfTilePosEmpty(v3))
                    Instantiate(tile, v3, Quaternion.identity);
                if (maze[y * row + x].East && checkIfTilePosEmpty(v3_2))
                    Instantiate(tile, v3_2, Quaternion.identity);
            }
        }
    }

    private bool checkIfTilePosEmpty(Vector3 targetPos)
    {
        // Destroy the wall we don't need and create a path
        GameObject[] allTilings = GameObject.FindGameObjectsWithTag("Tile");
        foreach (GameObject t in allTilings)
        {
            if (t.transform.position == targetPos)
            {
                Destroy(t);
            }
        }
        return true;
    }
}
