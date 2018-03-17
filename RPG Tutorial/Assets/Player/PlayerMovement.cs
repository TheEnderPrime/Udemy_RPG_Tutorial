using System;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

[RequireComponent(typeof (ThirdPersonCharacter))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float walkMoveStopRadius = 0.2f;
    ThirdPersonCharacter ThirdPersonCharacter;   // A reference to the ThirdPersonCharacter on the object
    CameraRaycaster cameraRaycaster;
    Vector3 currentClickTarget;

    bool isInDirectMode = false;
    private bool Jump;                      // the world-relative desired move direction, calculated from the camForward and user input.


    private void Start()
    {
        cameraRaycaster = Camera.main.GetComponent<CameraRaycaster>();
        ThirdPersonCharacter = GetComponent<ThirdPersonCharacter>();
        currentClickTarget = transform.position;
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
            currentClickTarget = transform.position; //clear the click target
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
            switch(cameraRaycaster.currentLayerHit) 
            {
                case Layer.Walkable:
                    currentClickTarget = cameraRaycaster.hit.point;  // So not set in default case
                    
                    break;
                case Layer.Enemy:
                    print("Not moving to enemy");
                    //currentClickTarget = cameraRaycaster.hit.point;
                    break;
                default:
                    print("Unexpected Layer Found");
                    return;
            }
        }
        var playerToClickPoint = currentClickTarget - transform.position;
        if(playerToClickPoint.magnitude >= walkMoveStopRadius) 
        {
            ThirdPersonCharacter.Move(playerToClickPoint, false, false);    
        } 
        else 
        {
            ThirdPersonCharacter.Move(Vector3.zero, false, false);
        }
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
}

