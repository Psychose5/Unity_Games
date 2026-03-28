using UnityEngine;
using UnityEngine.InputSystem;

public class Arknoidplayer : MonoBehaviour
{
    public Vector2 inputDir;
    public Transform Field;
    public float speed;
    public void OnMove(InputAction.CallbackContext context) => inputDir = context.ReadValue<Vector2>();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    // Move the paddle based on player input
    void Move()
    {
        float horizontalInput = inputDir.x;
        float newX = transform.position.x + horizontalInput * speed * Time.deltaTime;
        // Limit the paddle's movement to the play area
        float halfPlayArea = Field.localScale.x * 10 * 0.5f; // Assuming the play area is scaled by 10
        float limitX = halfPlayArea - (transform.localScale.x * 0.5f); // Limit based on paddle width
        newX = Mathf.Clamp(newX, -limitX, limitX);
        transform.position = new Vector3(newX, transform.position.y, transform.position.z);
    }

// get player Transform
    public Transform getPlayer()
    {
        return transform;
    }
}