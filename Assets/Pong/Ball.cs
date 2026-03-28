using TMPro;
using UnityEngine; 

public class Ball : MonoBehaviour
{
    public TMP_Text TextMesh;
    private Vector3 velocity;
    public Transform player;
    public float speedwall;
    public float speedpaddle;
    private int score1 = 0;
    private int score2 = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //gameObject.GetComponent<AudioSource>().Play();
        velocity = new Vector3(0, 0, speedpaddle);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += velocity * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Paddle"))
        {
            float maxDist = other.transform.localScale.x * 0.5f + transform.localScale.x * 0.5f;
            float dist = transform.position.x - other.transform.position.x;
            float nDist = dist / maxDist;
            velocity = new Vector3(nDist * speedwall, velocity.y, -velocity.z);
        }
        else if(other.CompareTag("Wall"))
        {
            velocity = new Vector3(-velocity.x, velocity.y, velocity.z);
        } 
        // Check for goal1
        else if(other.CompareTag("Goal1"))
        {
            score1++;
            velocity = new Vector3(0, 0, speedpaddle);
            transform.position = player.position;
            UpdateScoreText();
        }
        // Check for goal2
        else if(other.CompareTag("Goal2"))
        {
            score2++;
            velocity = new Vector3(0, 0, -speedpaddle);
            transform.position = player.position;
            UpdateScoreText();

        }

        gameObject.GetComponent<AudioSource>().Play();
    }
    void UpdateScoreText()
    {
        TextMesh.text = score1.ToString() + " : " + score2.ToString();
    }
}
