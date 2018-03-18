using System;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

[RequireComponent(typeof (ThirdPersonCharacter))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float walkMoveStopRadius = 0.2f;
    [SerializeField] float attackMoveStopRadius = 5f;
    ThirdPersonCharacter ThirdPersonCharacter;   // A reference to the ThirdPersonCharacter on the object
    CameraRaycaster cameraRaycaster;
    Vector3 currentDestination, clickPoint;

    bool isInDirectMode = false;
    private bool Jump;                      // the world-relative desired move direction, calculated from the camForward and user input.


    private void Start()
    {
        cameraRaycaster = Camera.main.GetComponent<CameraRaycaster>();
        ThirdPersonCharacter = GetComponent<ThirdPersonCharacter>();
        currentDestination = transform.position;
    }

    private void Update()
        {
            if (!Jump)
            {
                Jump = Input.GetButtonDown("Jump");
            }
        }

    // Fixed update is called in sync with physics
    private void FixedUpdate()
    {
        if(Input.GetKeyDown(KeyCode.G)) // G for gamepad
        {
            isInDirectMode = !isInDirectMode; //toggle mode
            currentDestination = transform.position; //clear the click target
        }

        if(isInDirectMode) {
            
            ProcessDirectMovement();
        
        } else {
            ProcessMouseMovement(); //mouse movement
        }

        
    }

    private void ProcessMouseMovement()
    {
        if (Input.GetMouseButton(0))
        {
            clickPoint = cameraRaycaster.hit.point;
            switch (cameraRaycaster.currentLayerHit)
            {
                case Layer.Walkable:
                    currentDestination = clickPoint;  // So not set in default case
                    currentDestination = ShortDestination(clickPoint, walkMoveStopRadius);

                    break;
                case Layer.Enemy:
                    currentDestination = ShortDestination(clickPoint, attackMoveStopRadius);
                    //currentClickTarget = cameraRaycaster.hit.point;
                    break;
                default:
                    print("Unexpected Layer Found");
                    return;
            }
        }

        WalkToDestination();
    }

    

    private void ProcessDirectMovement() 
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        bool crouch = Input.GetKey(KeyCode.C);

        // calculate camera relative direction to move:
        
        Vector3 cameraForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;
        Vector3 Move = v * cameraForward + h * Camera.main.transform.right;

        ThirdPersonCharacter.Move(Move, crouch, Jump);
        Jump = false;
    }

    private void WalkToDestination()
    {
        var playerToClickPoint = currentDestination - transform.position;
        if (playerToClickPoint.magnitude >= walkMoveStopRadius)
        {
            ThirdPersonCharacter.Move(playerToClickPoint, false, false);
        }
        else
        {
            ThirdPersonCharacter.Move(Vector3.zero, false, false);
        }
    }

    Vector3 ShortDestination(Vector3 destination, float shortening)
    {
        Vector3 reductionVector = (destination - transform.position).normalized * shortening;
        return destination - reductionVector;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawLine(transform.position, currentDestination);
        Gizmos.DrawSphere(currentDestination, 0.1f); // shows shortened destination
        Gizmos.DrawSphere(clickPoint, 0.15f); // shows click point

        //Draw attack sphere
        Gizmos.color = new Color(255f, 0f, 0, .5f);
        Gizmos.DrawWireSphere(transform.position, attackMoveStopRadius);
    }
}

