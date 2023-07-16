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

    public HuatiSettings settings;
    private bool _jumping;

    public bool Grounded;
    public float timeSinceChargeStart = -1;

    private float _jumpHeightMultiplier, _timeSinceGrounded;

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
        float micVolume = HuatiGameManager.Instance.GetMicrophoneVolume();

        if(!Grounded && rb.velocity.y > 0 && _timeSinceGrounded - Time.realtimeSinceStartup < 1.5f) { rb.AddForce(settings.jumpForce * Vector2.up * micVolume); }
        if (_jumping || !Grounded) { return; }

        if (timeSinceChargeStart > 0f) { timeSinceChargeStart -= Time.fixedDeltaTime; return; }

        if (micVolume > settings.jumpThreshhold) { ChargeJump(micVolume); }
        else if (_jumpHeightMultiplier > 0f) { Jump(); }
    }

    void Jump()
    {
        if (!Grounded || _jumping) { return; }
        rb.AddForce(Vector2.up * settings.jumpForce * (1f+_jumpHeightMultiplier), ForceMode2D.Impulse);
        StartCoroutine(StaticJumpCooldown());
        anim.AnimationState.SetAnimation(0, "jump", false);

        IEnumerator StaticJumpCooldown()
        {
            _jumping = true;
            yield return new WaitForSeconds(1.5f);
            _jumping = false;
        }

        Debug.Log(_jumpHeightMultiplier);

        _jumpHeightMultiplier = 0;
    }
    void ChargeJump(float micVol)
    {
        if(timeSinceChargeStart <= 0) { StartChargingJump(); }
        if(_jumpHeightMultiplier < micVol) { _jumpHeightMultiplier = micVol; }
    }
    void StartChargingJump()
    {
        timeSinceChargeStart = settings.minJumpTime;
        anim.ClearState();
        anim.AnimationState.SetAnimation(0, "prep", false);
    }
}