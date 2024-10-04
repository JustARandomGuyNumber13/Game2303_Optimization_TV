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