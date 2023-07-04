using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Rigidbody2D rb;
    public BoxCollider2D col;
    public LayerMask groundLayers;

    public int jumpForce, moveSpeed;
    public bool grounded;
    private bool _jumping;

    public Animator anim;
    public SpriteRenderer sprite;
    public float lookDir;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float dir = Input.GetAxisRaw("Horizontal");
        if(dir != 0) { sprite.flipX = lookDir <= 0; lookDir = dir; }
    }

    private void FixedUpdate()
    {
        anim.SetFloat("YVelocity", rb.velocity.y);
        float micVolume = GameManager.Instance.GetMicrophoneVolume();
        if (GameManager.Instance.GetMicrophoneVolume() > 0.1f) { Jump(micVolume); }

        if (Physics2D.BoxCast(transform.position, new Vector2(col.size.x * .95f, Mathf.Abs(0.1f - rb.velocity.y / 10f)), 0f, Vector2.down, 0.1f, groundLayers)) { grounded = true; anim.SetBool("Grounded", true); }
        else { grounded = false; anim.SetBool("Grounded", false); }
    }

    void Jump(float jumpMultipler = 0f)
    {
        if (!grounded || _jumping) { return; }
        Debug.Log("Jump");
        rb.AddForce(Vector2.up * jumpForce * (1 + jumpMultipler), ForceMode2D.Impulse);
        rb.AddForce(Vector2.right * moveSpeed * (1 + jumpMultipler) * lookDir, ForceMode2D.Impulse);
        StartCoroutine(StaticJumpCooldown());
        anim.Play("Jump");
        IEnumerator StaticJumpCooldown()
        {
            _jumping = true;
            yield return new WaitForSeconds(0.3f);
            _jumping = false;
        }
    }
}
