using UnityEngine;

public class Player : MonoBehaviour {
    public float speed = 10;

    private Rigidbody playerRigidbody;
    private Vector3 velocity;

    private void Start() {
        playerRigidbody = GetComponent<Rigidbody>();
    }

    private void Update() {
        velocity = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized * speed;
    }

    private void FixedUpdate() {
        playerRigidbody.MovePosition(playerRigidbody.position + velocity * Time.fixedDeltaTime);
    }
}