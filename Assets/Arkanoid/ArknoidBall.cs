using System;
using UnityEngine; 

public class ArknoidBall : MonoBehaviour
{
    //public TMP_Text TextMesh;
    [SerializeField]
    private Transform Ballstart; // startingposition of ball
    [SerializeField]
    private Transform Lifes; // lifes text
    [SerializeField]
    private float speed = 6f; // speed direction paddle
    private float maxspeed;
    private float minspeed;
    [SerializeField]
    private float ballRadius = 0.5f; // ball radius needed for collision detection
    private Vector3 velocity;
    private Vector3 previousPosition;
    private bool blockpaddle = false;
    private int lifescore = 100;
    private Arknoidscore scoreSystem;
    private ArknoidLife playerlife;
    private ArknoidBlockPlacer placer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {   
        // scoreclass
        scoreSystem = FindFirstObjectByType<Arknoidscore>();
        // lifeclass
        playerlife = FindFirstObjectByType<ArknoidLife>();
        // blockplacer class
        placer = FindFirstObjectByType<ArknoidBlockPlacer>();

        //velocity = new Vector3(0, 0, -speed);
        velocity = new Vector3(0, 0, -1f);
        velocityset();

        maxspeed = speed + 2f;
        minspeed = Mathf.Max(speed - 2f, 1f);
    }

    // Update called fixed time per second
    void FixedUpdate()
    {
        previousPosition = transform.position;
        transform.position += velocity * Time.fixedDeltaTime;
    }
    
    private void OnTriggerEnter(Collider other)
    {
        switch (other.tag)
        {
            
            case "Paddle":
            {
                // spawn new blockline
                if (scoreSystem.newBlockline())
                {
                    placer.newBlockline();
                    scoreSystem.setzerodestroyed();
                }

                float maxDist = other.transform.localScale.x * 0.5f + transform.localScale.x * 0.5f;
                float dist = transform.position.x - other.transform.position.x;
                float nDist = dist / maxDist;
                transform.position = previousPosition; // back to calculationposition
                //velocity = new Vector3(nDist * speed, velocity.y, -velocity.z);
                
                // new direction
                velocity = new Vector3(nDist, 0, 1f);
                // normalize speed
                velocityset();
                gameObject.GetComponent<AudioSource>().Play();
            }
            break;

            case "Block":
            {
                Bounds blockBounds = other.bounds;
                transform.position = previousPosition; // back to calculationposition

                // Overlap calculation with ballradius
                float overlapLeft = Mathf.Abs((previousPosition.x + ballRadius) - blockBounds.min.x);
                float overlapRight = Mathf.Abs(blockBounds.max.x - (previousPosition.x - ballRadius));
                float overlapX = Mathf.Min(overlapLeft, overlapRight);

                float overlapBottom = Mathf.Abs((previousPosition.z + ballRadius) - blockBounds.min.z);
                float overlapTop = Mathf.Abs(blockBounds.max.z - (previousPosition.z - ballRadius));
                float overlapZ = Mathf.Min(overlapBottom, overlapTop);

                if (blockpaddle)
                {
                    float maxDist = other.transform.localScale.x * 0.5f + transform.localScale.x * 0.5f;
                    float dist = transform.position.x - other.transform.position.x;
                    float nDist = dist / maxDist;
                    //velocity = new Vector3(nDist * speed, velocity.y, -Mathf.Abs(velocity.z));
                    // new direction
                    velocity = new Vector3(nDist, 0, -1f);
                }
                else
                {
                    // decision which side is hit
                    if (overlapX < overlapZ)
                    {
                        velocity.x = -velocity.x;
                    }
                    else
                    {
                        velocity.z = -velocity.z;
                    }
                }
                velocityset();
                gameObject.GetComponent<AudioSource>().Play();
            }
            break;

            case "Wall":
                gameObject.GetComponent<AudioSource>().Play();
                transform.position = previousPosition;
                velocity.x = -velocity.x;
                velocityset();
                break;

            case "Backwall":
                gameObject.GetComponent<AudioSource>().Play();
                transform.position = previousPosition;
                velocity.z = -velocity.z;
                velocityset();
                break;

            case "Arenaout":
                int lifes = playerlife.GetLifes();

                // score minus 100
                scoreSystem.subtractScore(lifescore);

                if (lifes > 0)
                {
                    // -1 life
                    playerlife.Lifedown();
                    placer.newBlockline();
                    scoreSystem.setzerodestroyed();

                    //velocity = new Vector3(0, 0, -speed);
                    velocity = new Vector3(0, 0, -1f);
                    velocityset();
                    transform.position = Ballstart.position;
                }
                else
                {
                    Gameover();
                }
                break;
        }
             
    }

    // no lifes left
    private void Gameover()
    {
        velocity = Vector3.zero;
        Destroy(gameObject);
        scoreSystem.Gameover();
    }

    // velocity set
    private void velocityset()
    {
        velocity = velocity.normalized * speed;
        if (Mathf.Abs(velocity.z) < 0.2f)
        {
            velocity.z = 0.2f * Mathf.Sign(velocity.z);
            velocity = velocity.normalized * speed;
        }
    }

    // change block reaction to paddle reaction
    public void IsBlockPaddle()
    {
        if (blockpaddle)
        {
            blockpaddle = false;
            Debug.Log("BlockPaddle out");
        }
        else
        {
            blockpaddle = true;
            Debug.Log("BlockPaddle on");
        }
        
    }

    // Ball size up
    public void ballspeedup()
    {   
        if ((speed + 0.2f) <= maxspeed)
        {
            speed = speed + 0.2f;
            velocityset();
        }
    }

    // Ball size down
    public void ballspeeddown()
    {
        if ((speed - 0.2f) >= minspeed)
        {
            speed = speed - 0.2f;
            velocityset();
        }
    }

    // nuke - all blocks life -1
     public void Nuke()
    {
        var blocks = FindObjectsByType<ArknoidBlock>(FindObjectsSortMode.None);
        foreach (var block in blocks)
        {
            block.lifedown();
        }
    }
}

