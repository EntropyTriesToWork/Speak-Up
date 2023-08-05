using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HuatiPlayerController : MonoBehaviour
{
    public Rigidbody2D rb;
    public BoxCollider2D col;
    public LayerMask groundLayers;
    public SkeletonAnimation anim;
    public AudioSource jumpSFX;

    public HuatiSettings settings;
    private bool _jumping;

    public bool Grounded;
    public float timeSinceChargeStart = -1;

    public float jumpHeightMultiplier;
    private float _timeSinceGrounded;

    private void FixedUpdate()
    {
        if (Physics2D.BoxCast(transform.position, new Vector2(col.size.x * .95f, Mathf.Abs(0.1f - rb.velocity.y / 10f)), 0f, Vector2.down, 0.1f, groundLayers))
        {
            Grounded = true;
            _timeSinceGrounded = Time.realtimeSinceStartup;

            if (anim.AnimationName == "jump") { anim.AnimationState.SetAnimation(0, "idle", true); }
        }
        else if (Grounded) { 
            Grounded = false;
            //anim.SetBool("Grounded", false); 
        }
        float micVolume = HuatiGameManager.Instance.CurrentMicVolume;

        if(!Grounded && rb.velocity.y > 0 && _timeSinceGrounded - Time.realtimeSinceStartup < 1.5f) { rb.AddForce(settings.jumpForce * Vector2.up * micVolume); }

        if (timeSinceChargeStart > 0f) 
        {
            if (jumpHeightMultiplier < micVolume) { jumpHeightMultiplier = micVolume; }
            HuatiGameManager.Instance.SetMicVolVisual(jumpHeightMultiplier, true, false);
            timeSinceChargeStart -= Time.fixedDeltaTime;
            return; 
        }
        HuatiGameManager.Instance.lockMicVolVisual = false;
        
        if (micVolume > settings.jumpThreshhold) { Jump(); }
    }

    void Jump()
    {
        if (!Grounded || _jumping) { return; }

        StartCoroutine(StaticJumpCooldown());
        
        IEnumerator StaticJumpCooldown()
        {
            _jumping = true;
            StartChargingJump();
            HuatiGameManager.Instance.SetMicVolVisual(jumpHeightMultiplier, true);
            yield return new WaitForSeconds(settings.minJumpTime);
            rb.AddForce(Vector2.up * settings.jumpForce * (0.5f + jumpHeightMultiplier), ForceMode2D.Impulse);
            anim.AnimationState.SetAnimation(0, "jump", false);
            jumpSFX.volume = 0.5f + jumpHeightMultiplier;
            jumpSFX.Play();
            yield return new WaitForSeconds(1f);
            _jumping = false;
            jumpHeightMultiplier = 0;
            HuatiGameManager.Instance.SetMicVolVisual(jumpHeightMultiplier);
        }
    }
    void StartChargingJump()
    {
        timeSinceChargeStart = settings.minJumpTime;
        anim.ClearState();
        anim.AnimationState.SetAnimation(0, "prep", false);
    }
}