using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class generateLabyrinth : MonoBehaviour
{
    public int size;
    public int cellSize;
    public int pathLengthRelativeToSize;
    public float wallThickness;
    public GameObject labyrinthWallPrefab;
    public GameObject cam;
    public Vector3 startPosition;
    private bool[] isWallDeleted;

    void Start()
    {
        Generate();
    }

    Vector4 GetPosAndSizeOfWall(Wall wall) {
        Cell cell1 = wall.GetCell1();
        Cell cell2 = wall.GetCell2();

        // ensure cell1 has smaller position vector
        if (cell1.GetPosX() > cell2.GetPosX() || cell1.GetPosY() > cell2.GetPosY()) {
            Cell tmp = cell1;
            cell1 = cell2;
            cell2 = tmp;
        }

        int x = cell1.GetPosX();
        int y = cell1.GetPosY();
        
        // check if wall is horizontal or vertical
        if (x == cell2.GetPosX()) {
            return new Vector4((float) (cellSize*(x+0.5)), (float) (cellSize*(y+1)), (float) cellSize+wallThickness, wallThickness);
        } else {
            return new Vector4((float) (cellSize*(x+1)), (float) (cellSize*(y+0.5)), (float) wallThickness, cellSize+wallThickness);
        }
    }

    void GenerateRectangle(float posX, float posY, float sizeX, float sizeY) {
        GameObject wall = Instantiate(labyrinthWallPrefab, new Vector2(posX, posY), new Quaternion(0.0f, 0.0f, 0.0f, 0.0f));
        Rigidbody2D rb = wall.GetComponent<Rigidbody2D>();
        wall.transform.localScale = new Vector2(sizeX, sizeY);
    }

    private void Generate()
    {
        Cell[][] cells = new Cell[size][];
        List<Wall> walls = new List<Wall>();

        // add cells
        for (int i = 0; i < size; i++) {
            cells[i] = new Cell[size];
            for (int j = 0; j < size; j++) {
                cells[i][j] = new Cell(i, j);
            }
        }

        // add walls
        for (int i = 0; i < size-1; i++) {
            walls.Add(new Wall(cells[i][size-1], cells[i+1][size-1]));
            walls.Add(new Wall(cells[size-1][i], cells[size-1][i+1]));
            for (int j = 0; j < size-1; j++) {
                Cell cell = cells[i][j];
                walls.Add(new Wall(cell, cells[i+1][j]));
                walls.Add(new Wall(cell, cells[i][j+1]));
            }
        }

        // create sets of connected cells
        HashSet<Cell>[][] connectedCellsSets = new HashSet<Cell>[size][];
        for (int i = 0; i < size; i++) {
            connectedCellsSets[i] = new HashSet<Cell>[size];
            for (int j = 0; j < size; j++) {
                connectedCellsSets[i][j] = new HashSet<Cell>();
                connectedCellsSets[i][j].Add(cells[i][j]);
            }
        }

        // remove Walls randomly until all cells are connected
        List<Wall> wallsTGenerate = new List<Wall>();
        List<Wall> removedWalls = new List<Wall>();
        System.Random rand = new System.Random();
        while (walls.Count != 0) {
            int randomIndex = rand.Next(walls.Count);
            Wall randomWall = walls[randomIndex];

            Cell cell1 = randomWall.GetCell1();
            Cell cell2 = randomWall.GetCell2();

            HashSet<Cell> set1 = connectedCellsSets[cell1.GetPosX()][cell1.GetPosY()];
            HashSet<Cell> set2 = connectedCellsSets[cell2.GetPosX()][cell2.GetPosY()];

            if (set1 != set2) {
                set1.UnionWith(set2);
                foreach (Cell cell in set2) {
                    connectedCellsSets[cell.GetPosX()][cell.GetPosY()] = set1;
                }
                removedWalls.Add(randomWall);
            } else {
                wallsTGenerate.Add(randomWall);
            }
            walls.RemoveAt(randomIndex);
        }

        // create a graph where vertices are cells and edges are removed walls
        UndirectedGraph labyrinth = new UndirectedGraph();
        for (int i = 0; i < size; i++) {
            for (int j = 0; j < size; j++) {
                labyrinth.AddVertex(cells[i][j].GetTag());
            }
        }
        foreach (Wall wall in removedWalls) {
            labyrinth.Connect(wall.GetCell1().GetTag(), wall.GetCell2().GetTag());
        }

        // list cells at the edge of the labyrinth
        List<Cell> outerCells = new List<Cell>();
        outerCells.Add(cells[0][0]);
        outerCells.Add(cells[0][size-1]);
        outerCells.Add(cells[size-1][0]);
        outerCells.Add(cells[size-1][size-1]);
        for (int i = 1; i < size-1; i++) {
            outerCells.Add(cells[i][0]);
            outerCells.Add(cells[i][size-1]);
            outerCells.Add(cells[0][i]);
            outerCells.Add(cells[size-1][i]);
        }

        // get pair of outer cells with path deviating the least from the wanted path length
        List<Tuple<Cell,Cell>> bestCellPairs = new List<Tuple<Cell,Cell>>();
        bestCellPairs.Add(new Tuple<Cell, Cell>(new Cell(0, 0), new Cell(0, 0)));
        int wantedPathLength = pathLengthRelativeToSize*size;
        int minimumDeviation = size*size;
        // consider each outer cell as entrance
        foreach (Cell cell in outerCells) {
            List<KeyValuePair<string, int>> destinations = CalculatePathLengths(labyrinth, cell.GetTag());

            // consider each destination cell as exit
            foreach (KeyValuePair<string, int> destination in destinations) {
                int xDest, yDest;
                Cell.GetPosFromTag(destination.Key, out xDest, out yDest);
                // check if the current destination cell is outer cell
                if (xDest == 0 || xDest == size-1 || yDest == 0 || yDest == size-1) {
                    int currentDeviation = Math.Abs(destination.Value-wantedPathLength);
                    // check if its path length deviates not more from the wanted path length
                    if (currentDeviation <= minimumDeviation) {
                        if (currentDeviation < minimumDeviation) {
                            minimumDeviation = currentDeviation;
                            bestCellPairs.Clear();
                        }
                        bestCellPairs.Add(new Tuple<Cell,Cell>(cell, cells[xDest][yDest]));
                        if (minimumDeviation == 0) {
                            break;
                        }
                    }
                }
            }
            if (minimumDeviation == 0) {
                break;
            }
        }
        Tuple<Cell,Cell> bestCellPair = bestCellPairs[rand.Next(bestCellPairs.Count)];
        Cell cellEntrance = bestCellPair.Item1;
        Cell cellExit = bestCellPair.Item2;

        // add outer walls and save entrance and exit
        List<Wall> entranceAndExit = new List<Wall>();
        foreach (Cell cell in outerCells) {
            List<Wall> wallsToAdd = new List<Wall>();
            if (cell.GetPosX() == 0) {
                wallsToAdd.Add(new Wall(new Cell(-1, cell.GetPosY()), cell));
            }
            if (cell.GetPosX() == size-1) {
                wallsToAdd.Add(new Wall(cell, new Cell(size, cell.GetPosY())));
            }
            if (cell.GetPosY() == 0) {
                wallsToAdd.Add(new Wall(new Cell(cell.GetPosX(), -1), cell));
            }
            if (cell.GetPosY() == size-1) {
                wallsToAdd.Add(new Wall(cell, new Cell(cell.GetPosX(), size)));
            }
            if (cell.Equals(cellExit)) {
                entranceAndExit.Add(wallsToAdd[0]);
                wallsToAdd.RemoveAt(0);
            }
            foreach (Wall wall in wallsToAdd) {
                wallsTGenerate.Add(wall);
            }
        }

        // generate walls
        foreach (Wall wall in wallsTGenerate) {
            Vector4 posAndSize = GetPosAndSizeOfWall(wall);
            GenerateRectangle(posAndSize.x, posAndSize.y, posAndSize.z, posAndSize.w);
        }

        startPosition = cellEntrance.GetCenterPosition(cellSize);
        gameObject.transform.position = startPosition;
        cam.transform.position = startPosition + new Vector3(0.0f, 0.0f, -1.0f);
    }

    private List<KeyValuePair<string, int>> CalculatePathLengths(UndirectedGraph graph, string tagFrom) {
        Dictionary<string, int> distance = new Dictionary<string, int>(graph.GetNumberOfVertices());
        HashSet<string> visited = new HashSet<string>();
        HashSet<string> visiting = new HashSet<string>();

        foreach (string tag in graph.GetVertices()) {
            distance.Add(tag, -1);
        }
        distance[tagFrom] = 0;

        visiting.Add(tagFrom);

        System.Random rand = new System.Random();

        while(visiting.Count != 0) {
            string currentTag = visiting.First();
            int currentDistance;
            distance.TryGetValue(currentTag, out currentDistance);

            List<string> adjacentVertices;

            if (graph.GetAdjacentVertices(currentTag, out adjacentVertices)) {
                foreach (string tag in adjacentVertices) {
                    if (!visited.Contains(tag) && !visiting.Contains(tag)) {
                        visiting.Add(tag);
                        distance.Remove(tag);
                        distance.Add(tag, currentDistance+1);
                    }
                }
            }

            visiting.Remove(currentTag);
            visited.Add(currentTag);
        }

        return distance.ToList();
    }

    private class Cell
    {
        private int posX;
        private int posY;

        public Cell(int posX, int posY) {
            this.posX = posX;
            this.posY = posY;
        }

        public int GetPosX() {
            return posX;
        }

        public int GetPosY() {
            return posY;
        }

        public String GetTag() {
            return GetPosX().ToString() + "," + GetPosY().ToString();
        }

        public bool Equals(Cell cell) {
            return GetPosX() == cell.GetPosX() && GetPosY() == cell.GetPosY();
        }

        public static void GetPosFromTag(string tag, out int x, out int y) {
            string[] indices = tag.Split(',');
            x = Convert.ToInt32(indices[0]);
            y = Convert.ToInt32(indices[1]);
        }

        public Vector2 GetCenterPosition(int cellSize) {
            return new Vector2(GetPosX() + 0.5f, GetPosY() + 0.5f) * cellSize;
        }

        public override string ToString() {
            return '[' + GetPosX().ToString() + ',' +  GetPosY().ToString() + ']';
        }
    }

    private class Wall
    {
        private Cell cell1;
        private Cell cell2;

        public Wall(Cell cell1, Cell cell2) {
            this.cell1 = cell1;
            this.cell2 = cell2;
        }

        public Cell GetCell1() {
            return cell1;
        }

        public Cell GetCell2() {
            return cell2;
        }
    }
    private class UndirectedGraph
    {
        private Dictionary<string,Vertex> vertices = new Dictionary<string,Vertex>();
        private int verticesCount = 0;
        private int edgesCount = 0;
        public UndirectedGraph() {}

        public int GetNumberOfVertices() {
            return verticesCount;
        }

        public int GetNumberOfEdges() {
            return edgesCount;
        }

        public bool AddVertex(string tag) {
            try {
                vertices.Add(tag, new Vertex(tag));
            }
            catch (System.ArgumentException) {
                return false;
            }
            verticesCount++;
            return true;
        }

        public bool ContainsVertex(string tag) {
            return vertices.ContainsKey(tag);
        }

        public bool GetAdjacentVertices(string tag, out List<string> adjacentVertices) {
            Vertex v;
            bool containsTag = vertices.TryGetValue(tag, out v);
            if (!containsTag) {
                adjacentVertices = null;
                return false;
            }
            adjacentVertices = v.GetAdjacentVertices().Keys.ToList();
            return true;
        }

        public List<string> GetVertices() {
            return vertices.Keys.ToList();
        }

        public bool AreConnected(string tagV, string tagU) {
            Vertex v, u;
            return TryGetEdge(tagV, tagU, out v, out u) && v.GetAdjacentVertices().ContainsKey(tagU);
        }

        public bool Connect(string tagV, string tagU) {
            Vertex v, u;
            if (!TryGetEdge(tagV, tagU, out v, out u) || v.GetAdjacentVertices().ContainsKey(tagU)) {
                return false;
            }
            v.GetAdjacentVertices().Add(tagU, u);
            u.GetAdjacentVertices().Add(tagV, v);
            edgesCount++;
            return true;
        }

        public bool Disconnect(string tagV, string tagU) {
            Vertex v, u;
            if (!TryGetEdge(tagV, tagU, out v, out u) || !v.GetAdjacentVertices().ContainsKey(tagU)) {
                return false;
            }
            v.GetAdjacentVertices().Remove(tagU);
            u.GetAdjacentVertices().Remove(tagV);
            edgesCount--;
            return true;
        }

        private bool TryGetEdge(string tagV, string tagU, out Vertex v, out Vertex u) {
            u = new Vertex("");
            return vertices.TryGetValue(tagV, out v) && vertices.TryGetValue(tagU, out u);
        }

        private class Vertex
        {
            private string tag;
            private Dictionary<string,Vertex> adjacentVertices = new Dictionary<string,Vertex>();

            public Vertex(string tag) {
                this.tag = tag;
            }

            public string GetTag() {
                return tag;
            }

            public Dictionary<string,Vertex> GetAdjacentVertices() {
                return adjacentVertices;
            }

            public bool IsAdjacentTo(string tag) {
                return adjacentVertices.ContainsKey(tag);
            }
        }
    }
}
