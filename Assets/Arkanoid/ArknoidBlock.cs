using UnityEngine;

public class ArknoidBlock : MonoBehaviour
{
    [Header("Block Settings")]
    private int hitPoints; // life of block (1-3)

    [Header("Effects and Abilities")]
    [SerializeField]
    private float abilitySpawnChance = 0.2f; // norm (20% Chance to spawn an ability)
    [SerializeField]
    private AudioClip breakSound;     // optional
    [SerializeField]
    private GameObject hitEffect;     // optional
    [SerializeField]
    private Material[] hitPointMaterials; // Matetial (Colors) for HitPoints (Index 0 = 1 HP, Index 1 = 2 HP, etc.)
    [SerializeField]
    private GameObject[] abilityPrefabsPerType = new GameObject[7]; // Ein Prefab für jeden Ability-Typ (0=BlockPaddle, 1=PlayerSizeup, etc.)
    private bool destroyed = false;
    private Arknoidscore scoreSystem;
    private ArknoidBlockPlacer placer;
    private int hitscore = 50;
    private int destroyscore = 100; 
    private int row; // Reihe
    private int col; // Spalte
    private bool alreadyRemoved = false;

    // on start blocks placed
    void Start()
    {
        // scoreclass
        scoreSystem = FindFirstObjectByType<Arknoidscore>();
        
        // random HitPoint amount (1-3)
        hitPoints = Random.Range(1, 4); // Random.Range(1, 4) = 1, 2 or 3
        
        // change material based on hitPoints
        UpdateMaterial();

    }

    // set connection to Blockplacer
    public void setplacer(ArknoidBlockPlacer p)
    {
        placer = p;
    }
    // ball hits block
    private void OnTriggerEnter(Collider other)
    {
        if (destroyed) return;

        if (other.CompareTag("Ball"))
        {
            if (hitEffect != null)
            {
                Instantiate(hitEffect, transform.position, Quaternion.identity);
            }
            lifedown();
        }
    }

    // life -1
    public void lifedown()
    {
        hitPoints--;
        if (hitPoints <= 0)
        {
            destroyed = true;

            if (breakSound != null)
            {
                AudioSource.PlayClipAtPoint(breakSound, Camera.main.transform.position);
            }
            OnDestroy();
            scoreSystem.addScore(destroyscore);
            scoreSystem.adddestroyedBlock();
            Abilityspawn();
        }
        else
        {
            // actualize material based on current hitPoints
            UpdateMaterial();
            scoreSystem.addScore(hitscore);
        }
    }

    private void OnDestroy()
    {
        if (placer != null && !alreadyRemoved)
        {
            placer.RemoveFromGrid(row, col, this);
            alreadyRemoved = true;
            Destroy(gameObject);
        }
    }
    // destroy block with minus points
    public void blockboom()
    {
        OnDestroy();
        int minusscore = 100 + ((hitPoints - 1) * 50); 
        scoreSystem.subtractScore(minusscore);
    }

    // set row and col
    public void settRowCol(int row, int col)
    {
        this.row = row;
        this.col = col;
    }

    // get row
    public int getrow()
    {
        return row;
    }

    // get col
    public int getcol()
    {
        return col;
    }

    // when an ability should spawn
    private void Abilityspawn()
    {
        // randomly decide if an ability should be spawned
        if (Random.value > abilitySpawnChance)
        {
            return; // no ability spawned
        }

        // randomly select an ability type
        ArknoidAbility.Type[] allTypes = {
            ArknoidAbility.Type.BlockPaddle,
            ArknoidAbility.Type.PlayerSizeup,
            ArknoidAbility.Type.PlayerSizedown,
            ArknoidAbility.Type.Lifeup,
            ArknoidAbility.Type.Ballspeedup,
            ArknoidAbility.Type.Ballspeeddown,
            ArknoidAbility.Type.Nuke
        };

        int randomTypeIndex = Random.Range(0, allTypes.Length);
        ArknoidAbility.Type selectedType = allTypes[randomTypeIndex];

        // Get specific prefab for this ability type
        if (abilityPrefabsPerType == null || abilityPrefabsPerType.Length != 7)
        {
            Debug.LogWarning("Ability prefabs array not properly configured!");
            return;
        }

        GameObject selectedPrefab = abilityPrefabsPerType[randomTypeIndex];

        if (selectedPrefab == null)
        {
            Debug.LogWarning($"Ability prefab for type {selectedType} is not assigned!");
            return;
        }

        // spawn ability at block position
        GameObject spawnedAbility = Instantiate(selectedPrefab, transform.position, selectedPrefab.transform.rotation);

        // set ability type on the spawned ability
        ArknoidAbility abilityComponent = spawnedAbility.GetComponent<ArknoidAbility>();
        if (abilityComponent != null)
        {
            abilityComponent.abilityType = selectedType;
            Debug.Log($"Ability spawned: {selectedType} at position {transform.position}");
        }
        else
        {
            Debug.LogWarning("Spawned ability has no ArknoidAbility component!");
        }
    }

    // actualize material based on current hitPoints
    private void UpdateMaterial()
    {
        if (hitPointMaterials != null && hitPointMaterials.Length > 0)
        {
            // Index calculate (hitPoints - 1, Array start 0)
            int materialIndex = Mathf.Clamp(hitPoints - 1, 0, hitPointMaterials.Length - 1);
            
            Renderer renderer = GetComponent<Renderer>();
            if (renderer != null && hitPointMaterials[materialIndex] != null)
            {
                renderer.material = hitPointMaterials[materialIndex];
            }
        }
    }
}