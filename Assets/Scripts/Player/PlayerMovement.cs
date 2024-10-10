using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
	public float speed = 6f;

    private Vector2 _moveInput;
	private readonly int _walkAnimHash = Animator.StringToHash("IsWalking");
    private Camera _cam;

    private Vector3 movement;
	private Animator anim;
	private Rigidbody playerRigidbody;
	private int floorMask;
	private float camRayLength = 100f;

	void Awake()
	{
		floorMask = LayerMask.GetMask("Floor");
		anim = GetComponent<Animator>();
		playerRigidbody = GetComponent<Rigidbody>();
        _cam =Camera.main;
    }
    void FixedUpdate()
	{
        if (_moveInput != Vector2.zero)
            Move();
        Turning();
    }


    private void Move()
    {
        movement.Set(_moveInput.x, 0f, _moveInput.y);
        movement = movement.normalized * speed * Time.deltaTime;
        playerRigidbody.MovePosition(transform.position + movement);
    }

    /* _TV_ What I think this funciton does:
    Cast a ray from camera position to the mouse position on the screen
    Check if the ray hit the ground collider
    Calculate the direction to look from player toward the hit point of the ray
    Rotate player to look to that point

    => The closer the player to the ground, the easier to rotate ( rotate wider )

    Possible solution:
    + Create an object with trigger, use this object as a ray to lower the cost
    + On this trigger object, you can return a "replacement of RaycastHit floorHit", which is Vector3 floorHit, exp:

    void OnTriggerStay(Collider other) {
        if(other == floorCollider)
            Vector3 floorHit = other.ClosestPoint(transform.position);
    }

    Finally, get a reference of this new floorHit   => More complicate but cost less
     */
    void Turning()  // This method cost too heavy to run because of raycast _TV_
    {
        Ray camRay = _cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit floorHit;

        //Debug.DrawLine(camRay.origin, camRay.origin + camRay.direction * camRayLength, Color.red); // Display the ray _TV_

        if (Physics.Raycast(camRay, out floorHit, camRayLength, floorMask))
        {
            // print(floorHit.collider.name);

            Vector3 playerToMouse = floorHit.point - transform.position;    // Direction toward hit point _TV_
            playerToMouse.y = 0f;

            Quaternion newRotation = Quaternion.LookRotation(playerToMouse);
            playerRigidbody.MoveRotation(newRotation);
        }
    }

    private void OnMove(InputValue value)		// New input system _TV_
	{
        _moveInput = value.Get<Vector2>();
        anim.SetBool(_walkAnimHash, _moveInput != Vector2.zero);    // Change animation call to event/trigger base
    }
}