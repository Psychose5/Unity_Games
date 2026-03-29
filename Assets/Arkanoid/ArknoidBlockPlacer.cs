using UnityEngine;

public class ArknoidBlockPlacer : MonoBehaviour
{
    [Header("Block Settings")]
    [SerializeField]
    private GameObject blockPrefab; // used Block model 
    [SerializeField]
    private Transform Line; // midpoint position of block line
    private int rows = 4; //  number of rows (Reihen)
    private int maxrow = 8; // number of maxblock rows (maxBlockreihen)
    private int columns = 9; // number of columns (Spalten)
    [SerializeField]
    private float spacingX = 5f; // distance between blocks horizontal
    [SerializeField]
    private float spacingY = 5f; // distance between blocks vertical

    
    [Header("Position Settings")]
    public Vector3 startPosition; // Startposition
    // Grid
    private ArknoidBlock[,] grid;
    private Arknoidscore scoreSystem;

    void Start()
    {
        // Startposition based on Line position
        startPosition = Line.position;
        // Grid initialisieren
        grid = new ArknoidBlock[maxrow, columns];
        // scoreclass
        scoreSystem = FindFirstObjectByType<Arknoidscore>();

        PlaceBlocks();
    }

    void PlaceBlocks()
    {
        if (blockPrefab == null || Line == null)
        {
            Debug.LogError("Setup missing");
            return;
        }

        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < columns; col++)
            {
                SpawnBlock(row, col);
            }
        }
        Debug.Log($"Block-Placer: {rows * columns} Blocks placed in {rows} rows and {columns} columns.");
    }

    // calculate position of next block
    private Vector3 GetBlockPosition(int row, int col)
    {   
        // Get block scales
        float blockX = blockPrefab.transform.localScale.x; // X Block width (Breite)
        float blockZ = blockPrefab.transform.localScale.z; // Z Block deepth (Tiefe)

        // claculate position for block
        Vector3 position = new Vector3(
            startPosition.x + col * (blockX + (spacingX/2)),
            startPosition.y,
            startPosition.z - row * (blockZ + (spacingY/2))
        );
        return position;
    }

    // spawn a new block
    public void SpawnBlock(int row, int col)
    {
        // Spawn with Grid
        if (grid[row, col] != null)
            return;

        Vector3 position = GetBlockPosition(row, col);

        GameObject newBlock = Instantiate(blockPrefab, position, Quaternion.identity, transform);
        newBlock.name = $"Block_{row + 1}/{col + 1}";

        ArknoidBlock block = newBlock.GetComponent<ArknoidBlock>();
        block.settRowCol(row, col);

        // safe in grid
        grid[row, col] = block;
        block.setplacer(this);
    }

    // Blockdestroy - destroy all blocks 
    public void ClearBlocks()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
        grid = new ArknoidBlock[maxrow, columns];
    }

    // Blockreset - clear all blocks and place new blocks
    public void RePlaceBlocks()
    {
        ClearBlocks();
        PlaceBlocks();
    }

    // move exiting Block 
    private void moveBlock(int row, int col)
    {
        ArknoidBlock block = grid[row, col];
        if (block == null) return;

        int newRow = row + 1;

        // if reached lowes row
        if (newRow >= maxrow)
        {
            block.blockboom();
            grid[row, col] = null;
            return;
        }

        // if targed area is free
        if (grid[newRow, col] == null)
        {
            grid[newRow, col] = block;
            grid[row, col] = null;

            block.transform.position = GetBlockPosition(newRow, col);
            block.settRowCol(newRow, col);
            // rename block
            block.name = $"Block_{newRow + 1}/{col + 1}";
        }
        else
        {
            // destroy block
            block.blockboom();
        }
    }

    // new Block line created and existing block moved
    public void newBlockline()
    {
        // move from down to upwords
        for (int row = maxrow - 1; row >= 0; row--)
        {
            for (int col = 0; col < columns; col++)
            {
                moveBlock(row, col);
            }
        }

        // spawn new Line
        for (int col = 0; col < columns; col++)
        {
            SpawnBlock(0, col);
        }
    }

    // remove a block from grid
    public void RemoveFromGrid(int row, int col, ArknoidBlock block)
    {
        if (row < 0 || row >= maxrow || col < 0 || col >= columns)
        return;

        if (grid[row, col] == block)
        {
            grid[row, col] = null;

            // check if won
            if (IsGridEmpty())
            {
                Debug.Log("YOU WIN!");
                scoreSystem.winning();
            }
        }
    }

    // test if grid is empty
    public bool IsGridEmpty()
    {
        for (int row = 0; row < maxrow; row++)
        {
            for (int col = 0; col < columns; col++)
            {
                if (grid[row, col] != null)
                {
                    return false;
                }
            }
        }
        return true;
    }
}