using UnityEngine;

public class ArknoidLife : MonoBehaviour
{
    [Header("Life Settings")]
    [SerializeField]
    private GameObject lifePrefab; // used Life model
    [SerializeField]
    private Transform Line; // midpoint position of life line
    private int lifes = 3; //  number of rows (Reihen)
    private float spacingZ = 2f; // distance between lifes vertical
    
    [Header("Position Settings")]
    [SerializeField]
    private Vector3 startPosition; // Startposition

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (lifePrefab == null)
        {
            Debug.LogError("Life Prefab nicht zugewiesen!");
            return;
        }

        if (Line == null)
        {
            Debug.LogError("Line Transform nicht zugewiesen!");
            return;
        }

        startPosition  = Line.position;

        // create lifes
        for (int i = 0; i < lifes; i++)
        {
            CreateSingleLife();
        }

        Debug.Log($"Leben erstellt: {lifes}");
    }

    // Update is called once per frame
    void Update()
    {
   
    }

    // one life create
    private void CreateSingleLife()
    {
        // Get sphere scales
        float sphereZ = lifePrefab.transform.localScale.z; // Z Sphere deepth (Tiefe)

        // calculat position of block
        Vector3 position = new Vector3(
            startPosition.x,
            startPosition.y,
            startPosition.z + (transform.childCount) * (sphereZ + (spacingZ/2))
        );

        GameObject newLife = Instantiate(lifePrefab, position, Quaternion.identity, transform);
        newLife.name = $"Life_{transform.childCount}";
    }

    // life down - destroy one life
    public void Lifedown()
    {
        if (transform.childCount > 0)
        {
            lifes--;
            Transform lastLife = transform.GetChild(transform.childCount - 1);
            Destroy(lastLife.gameObject);  
        }
        else
        {
            Debug.Log("Game Over!");
        }
    }

    // life up - create one life
    public void Lifeup()
    {
        Debug.Log("+ 1 Life");
        lifes++;
        CreateSingleLife();
    }

    // get life
    public int GetLifes()
    {
        return lifes;
    }
}
