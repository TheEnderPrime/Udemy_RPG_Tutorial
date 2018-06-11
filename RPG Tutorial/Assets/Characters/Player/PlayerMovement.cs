using System;
using UnityEngine;
using UnityEngine.AI;
using UnityStandardAssets.Characters.ThirdPerson;

[RequireComponent(typeof (ThirdPersonCharacter))]
[RequireComponent(typeof (NavMeshAgent))]
[RequireComponent(typeof (AICharacterControl))]
public class PlayerMovement : MonoBehaviour
{
    ThirdPersonCharacter ThirdPersonCharacter = null;   // A reference to the ThirdPersonCharacter on the object
    CameraRaycaster cameraRaycaster = null;
    Vector3 currentDestination, clickPoint;
    AICharacterControl aiCharacterControl = null;
    GameObject walkTarget = null;

    // TODO solve fight between serialize and const
    [SerializeField] const int walkableLayerNumber = 8;
    [SerializeField] const int enemyLayerNumber = 9;

    bool isInDirectMode = false;
    private bool Jump;                      // the world-relative desired move direction, calculated from the camForward and user input.


    void Start()
    {
        cameraRaycaster = Camera.main.GetComponent<CameraRaycaster>();
        ThirdPersonCharacter = GetComponent<ThirdPersonCharacter>();
        currentDestination = transform.position;
        aiCharacterControl = GetComponent<AICharacterControl>();
        walkTarget = new GameObject("walkTarget");

        cameraRaycaster.notifyMouseClickObservers += ProcessMouseClick;
    }

    void ProcessMouseClick(RaycastHit raycastHit, int layerHit) {
        switch(layerHit) {
            case enemyLayerNumber:
                GameObject enemy = raycastHit.collider.gameObject;
                aiCharacterControl.SetTarget(enemy.transform);
                break;
            case walkableLayerNumber:
                walkTarget.transform.position = raycastHit.point;
                aiCharacterControl.SetTarget(walkTarget.transform);
                break;
            default:
                Debug.LogWarning("Mouse Click Handling Error in Player Movement");
                return;
        }
    }

    // private void Update()
    // {
    //         if (!Jump)
    //         {
    //             Jump = Input.GetButtonDown("Jump");
    //         }
    // }

    // Fixed update is called in sync with physics
    // private void FixedUpdate()
    // {
    //     if(Input.GetKeyDown(KeyCode.G)) // G for gamepad
    //     {
    //         isInDirectMode = !isInDirectMode; //toggle mode
    //         currentDestination = transform.position; //clear the click target
    //     }

    //     if(isInDirectMode) {
            
    //         ProcessDirectMovement();
        
    //     } else {
    //         ProcessMouseMovement(); //mouse movement
    //     }

        
    // }


    
// TODO make this get called once again
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

    Vector3 ShortDestination(Vector3 destination, float shortening)
    {
        Vector3 reductionVector = (destination - transform.position).normalized * shortening;
        return destination - reductionVector;
    }
}

