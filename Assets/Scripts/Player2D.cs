using UnityEngine;

public class Player2D : MonoBehaviour {
    public float speed = 10;

    private Rigidbody2D playerRigidbody2D;
    private Vector2 velocity;

    private void Start() {
        playerRigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void Update() {
        velocity = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized * speed;
    }

    private void FixedUpdate() {
        playerRigidbody2D.MovePosition(playerRigidbody2D.position + velocity * Time.fixedDeltaTime);
    }
}