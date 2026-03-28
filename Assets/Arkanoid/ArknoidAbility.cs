using UnityEngine;

public class ArknoidAbility : MonoBehaviour
{
    public enum Type { BlockPaddle, PlayerSizeup, PlayerSizedown, Lifeup, Ballspeedup, Ballspeeddown, Nuke }
    public Type abilityType;
    [SerializeField]
    private float fallSpeed = 2f;
    [SerializeField]
    private Transform Player;
    private int abiscore = 100;
    private Arknoidscore playerscore;
    private ArknoidBall ball;
    private ArknoidLife life;
    private float maxscalex;
    private float minscalex;

    void Start()
    {
        // scoreclass
        playerscore = FindFirstObjectByType<Arknoidscore>();
        // ballclass
        ball = FindFirstObjectByType<ArknoidBall>();
        // lifeclass
        life = FindFirstObjectByType<ArknoidLife>();
        // player Transform
        Player = FindFirstObjectByType<Arknoidplayer>().getPlayer();
        // maxscale for player
        maxscalex = Player.localScale.x + 4f;
        // minscale for player
        minscalex = 1f;
    }
    void Update()
    {
        // Move the ability downwards
        //transform.Translate(Vector3.back * fallSpeed * Time.deltaTime);
        transform.Translate(Vector3.back * fallSpeed * Time.deltaTime, Space.World);
    }
    
    // change block reaction to paddle reaction
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Paddle"))
        {
            Debug.Log($"Ability {abilityType} collected by player!");
            Apply(other.gameObject);
            Destroy(gameObject);
        }

        if (other.CompareTag("Arenaout"))
        {
            Destroy(gameObject);
        }
    }

    // Apply the effect of the ability to the player
    private void Apply(GameObject player)
    {
        Player = player.transform;
        switch (abilityType)
        {
            // change blockball direction to paddleball direction
            case Type.BlockPaddle:
                IsBlockPaddle();
                playerscore.addScore(abiscore);
                break;
            // change paddle size up
            case Type.PlayerSizeup:
                Playersizeup();
                playerscore.subtractScore(abiscore);
                break;
            // change paddle size down
            case Type.PlayerSizedown:
                Playersizedown();
                playerscore.addScore(abiscore);
                break;
            // life up
            case Type.Lifeup:
                Lifeup();
                playerscore.subtractScore(abiscore);
                break;
            // change ball speed up
            case Type.Ballspeedup:
                Ballspeedup();
                playerscore.subtractScore(abiscore);
                break;
            // change ball speed down
            case Type.Ballspeeddown:
                Ballspeeddown();
                playerscore.addScore(abiscore);
                break;
            // nuke - all blocks life -1
            case Type.Nuke:
                Nuke();
                break;
        }
    }

    // change block reaction to paddle reaction
    public void IsBlockPaddle()
    {
        if (ball != null)
        {
            ball.IsBlockPaddle();
        }
    }

    // Playersize up
    private void Playersizeup()
    {
        float scalex = Player.localScale.x;
        if ((scalex + 1f) <= maxscalex)
        {
            scalex = scalex + 1f;
            Player.localScale = new Vector3(scalex, Player.localScale.y, Player.localScale.z);
        }
    }

    // Playersize down
    private void Playersizedown()
    {
        float scalex = Player.localScale.x;
        if ((scalex - 1f) >= minscalex)
        {
            scalex = scalex - 1f;
            Player.localScale = new Vector3(scalex, Player.localScale.y, Player.localScale.z);
        }
    }

    // life up
    public void Lifeup()
    {
        if (life != null)
        {
            life.Lifeup();
        }
    }

    // change ballspeed up
    public void Ballspeedup()
    {
        if (ball != null)
        {
            ball.ballspeedup();
        }
    }

    // change ballspeed down
    public void Ballspeeddown()   {
        if (ball != null)
        {
            ball.ballspeeddown();
        }
    }

    // nuke - all blocks destroy
     public void Nuke()
    {
        if (ball != null)
        {
            ball.Nuke();
        }
    }
}
