using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Rigidbody2D rb;
    public CapsuleCollider2D col;
    public LayerMask groundLayers;
    public Animator anim;
    public Transform playerSprite;

    public float jumpTriggerThreshhold;
    public int jumpForce, moveSpeed;
    private bool _jumping;

    public bool Grounded;
    public float LookDirection { get; private set; }

    private float _jumpHeightMultiplier, _jumpDistanceMultiplier;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!Grounded) { return; }
        float dir = Input.GetAxisRaw("Horizontal");
        if(dir != 0) { playerSprite.localScale = new Vector3(LookDirection <= 0 ? -1 : 1, 1, 1); LookDirection = dir; }
    }

    private void FixedUpdate()
    {
        anim.SetFloat("YVelocity", rb.velocity.y);

        if (Physics2D.BoxCast(transform.position, new Vector2(col.size.x * .95f, Mathf.Abs(0.1f - rb.velocity.y / 10f)), 0f, Vector2.down, 0.1f, groundLayers))
        {
            Grounded = true;
            rb.sharedMaterial.bounciness = 0;
            anim.SetBool("Grounded", true); 
        }
        else if(Grounded) { Grounded = false; anim.SetBool("Grounded", false); rb.sharedMaterial.bounciness = 0.5f; }

        if (_jumping) { return; }
        float micVolume = GameManager.Instance.GetMicrophoneVolume();
        if (micVolume > jumpTriggerThreshhold) { ChargeJump(micVolume); }
        else if (_jumpDistanceMultiplier > 0f || _jumpHeightMultiplier > 0f) { Jump(); }
    }

    void Jump()
    {
        if (!Grounded || _jumping) { return; }
        Debug.Log("Height: " + _jumpHeightMultiplier + "    Distance: " + _jumpDistanceMultiplier);
        float heightMult = Mathf.Min(1f, _jumpHeightMultiplier);
        rb.AddForce(Vector2.up * jumpForce * (0.8f + heightMult), ForceMode2D.Impulse);
        rb.AddForce(Vector2.right * moveSpeed * (0.8f + _jumpDistanceMultiplier) * LookDirection, ForceMode2D.Impulse);
        StartCoroutine(StaticJumpCooldown());
        anim.Play("Jump");
        IEnumerator StaticJumpCooldown()
        {
            _jumping = true;
            yield return new WaitForSeconds(0.3f);
            _jumping = false;
        }

        _jumpHeightMultiplier = 0;
        _jumpDistanceMultiplier = 0;
    }
    void ChargeJump(float micVol)
    {
        _jumpHeightMultiplier += Time.fixedDeltaTime;
        if(_jumpDistanceMultiplier < micVol) _jumpDistanceMultiplier = micVol;
    }
}