using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private float rotateSpeed = 720f;
    [SerializeField] private GameInput gameInput;
    [SerializeField] private Transform visual;

    private Rigidbody rb;
    private bool isWalking;

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

        HandleInteractions();
    }

    public bool IsWalking() => isWalking;

    private void HandleInteractions()
    {
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();
        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);

        if (moveDir.sqrMagnitude < 0.0001f)
            return;

        float interactDistance = 2f;
        Vector3 rayOrigin = transform.position + Vector3.up * 0.5f;

        if (Physics.Raycast(rayOrigin, moveDir, out RaycastHit raycastHit, interactDistance));
    }
}