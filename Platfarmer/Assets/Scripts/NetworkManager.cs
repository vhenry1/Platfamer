using Unity.Netcode;
using UnityEngine;

public class NetworkPaddleController : NetworkBehaviour
{
    [SerializeField] private float speed = 8f;
    [SerializeField] private float leftX = -8f;
    [SerializeField] private float rightX = 8f;
    [SerializeField] private float minY = -4f;
    [SerializeField] private float maxY = 4f;


    private Rigidbody2D rb;

    public override void OnNetworkSpawn()
    {
        rb = GetComponent<Rigidbody2D>();

    
        float x = (OwnerClientId == 0) ? leftX : rightX;
        transform.position = new Vector3(x, 0f, 0f);
    }


    void FixedUpdate()
    {
        if (IsOwner)
        {
            string axis = (OwnerClientId == 0) ? "LeftPaddle" : "RightPaddle";
            float input = Input.GetAxisRaw(axis);

            float newY = transform.position.y + (input * speed * Time.fixedDeltaTime);


            newY = Mathf.Clamp(newY, minY, maxY);


            rb.MovePosition(new Vector2(rb.position.x, newY));
            syncedYPosition.Value = newY;

        }
        else
        {
            rb.MovePosition(new Vector2(rb.position.x, syncedYPosition.Value));
        }
    }

    private NetworkVariable<float> syncedYPosition =
    new NetworkVariable<float>(
        0f,
        NetworkVariableReadPermission.Everyone,
        NetworkVariableWritePermission.Owner
    );

}