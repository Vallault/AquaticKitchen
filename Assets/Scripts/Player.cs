using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private float rotateSpeed = 720f;
    [SerializeField] private GameInput gameInput;
    [SerializeField] private Transform visual;
    [SerializeField] private float interactDistance = 2f;
    [SerializeField] private float interactRange = 09f;

    private Rigidbody rb;
    private bool isWalking;
    private Vector3 lastInteractionDir = Vector3.forward;
    private ClearCounter selectedCounter;

    private void Start()
    {
        gameInput.OnInteractAction += GameInput_OnInteractAction;
    }

    private void OnDestory()
    {
        gameInput.OnInteractAction += GameInput_OnInteractAction;
    }

    private void GameInput_OnInteractAction(object sender, System.EventArgs e)
    {
        if (selectedCounter == null)
        {
            Debug.Log("E pressed: no counter selected");
                return;
        }

        selectedCounter.Interact();
     }

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();


        rb.constraints = RigidbodyConstraints.FreezeRotation;
        rb.useGravity = false;
    }

    private void FixedUpdate()
    {
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();
        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);


        rb.MovePosition(rb.position + moveDir * moveSpeed * Time.fixedDeltaTime);


        if (moveDir.sqrMagnitude > 0.0001f)
        {
            Quaternion target = Quaternion.LookRotation(moveDir, Vector3.up);
            visual.rotation = Quaternion.RotateTowards(visual.rotation, target, rotateSpeed * Time.fixedDeltaTime);
        }

        isWalking = moveDir.sqrMagnitude > 0.0001f;
    }

    private void Update()
    {
        HandleInteractions();
    }

    public bool IsWalking() => isWalking;

    private void HandleInteractions()
    {
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();
        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);

        if (moveDir.sqrMagnitude > 0.0001f)
        {
            lastInteractionDir = moveDir;
        }

        float interactDistance = 2f;
        Vector3 rayOrigin = transform.position + Vector3.up * 0.5f;
        Debug.DrawRay(rayOrigin, lastInteractionDir * interactDistance);

        ClearCounter newSelected = null;

        if (Physics.Raycast(rayOrigin, lastInteractionDir, out RaycastHit raycastHit, interactDistance))
        {
            ClearCounter clearCounter = raycastHit.transform.GetComponentInParent<ClearCounter>();

            if (clearCounter != null)
            {
                float flatDistance = Vector3.Distance(new Vector3(transform.position.x, 0f, transform.position.z),
                    new Vector3(clearCounter.transform.position.x, 0f, clearCounter.transform.position.z));

                if (flatDistance <= interactRange)
                {
                    newSelected = clearCounter;
                }
            }

        }

        if (newSelected != selectedCounter)
        {
            selectedCounter = newSelected;

            if (selectedCounter != null)
                Debug.Log("LOCKED ON: " + selectedCounter.name);
            else
                Debug.Log("UNSELECTED");
        }
    }
}