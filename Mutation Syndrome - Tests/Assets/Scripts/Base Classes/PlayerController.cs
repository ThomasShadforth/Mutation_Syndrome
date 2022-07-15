using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;
    //Note: Stats are being handled by the characterstatus script
    //The only stats that should be read to the main player controller are speed, etc.
    [SerializeField] float moveSpeed;
    Rigidbody2D rb;
    Vector2 moveInput;

    [SerializeField] bool limping;
    public bool canMove;

    // Start is called before the first frame update
    void Start()
    {
        if(instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }

        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");

        moveInput.Normalize();
    }

    private void LateUpdate()
    {
        rb.velocity = new Vector2(moveInput.x * (!limping ? moveSpeed : moveSpeed / 2), moveInput.y * (!limping ? moveSpeed : moveSpeed / 2));
    }

    public void updateMoveSpeed(float speedToSet)
    {

    }
}
