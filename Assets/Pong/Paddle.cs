using UnityEngine;
using UnityEngine.InputSystem;

public class Padlle : MonoBehaviour
{
    [Header("Paddle Einstellungen")]
    public float speed;
    public Transform playArea;

    public Vector2 inputDir;

    public void OnMove(InputAction.CallbackContext context)
    {
        inputDir = context.ReadValue<Vector2>();
    }

    void Update()
    {
        Move();
    }

    void Move()
    {
        float horizontalInput = inputDir.y;
        float newX = transform.position.x + horizontalInput * speed * Time.deltaTime;
        // Limit the paddle's movement to the play area
        float halfPlayArea = playArea.localScale.x * 10 * 0.5f; // Assuming the play area is scaled by 10
        float limitX = halfPlayArea - (transform.localScale.x * 0.5f); // Limit based on paddle width
        newX = Mathf.Clamp(newX, -limitX, limitX);
        transform.position = new Vector3(newX, transform.position.y, transform.position.z);
    }
}
