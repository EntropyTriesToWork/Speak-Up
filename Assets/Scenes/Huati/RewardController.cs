using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class RewardController : MonoBehaviour
{
    public Image rewardSprite;
    public SpriteRenderer mysteryReward;
    public SpriteRenderer book;
    public Animator anim;
    public List<Sprite> rewardSprites;
    public CanvasGroup rewardPopupCanvas;

    public int triggerCountsToActivate = 3;
    private int _triggersLeft;

    public AudioSource audioSource;
    public AudioClip bubbleSFX, revealRewardSFX;

    public void SwitchReward(Sprite sprite)
    {
        rewardSprite.sprite = sprite;
        mysteryReward.sprite = sprite;
        rewardSprite.SetNativeSize();
    }
    private void Start()
    {
        ResetRewardProgress();
    }

    public void Trigger()
    {
        _triggersLeft--;
        audioSource.PlayOneShot(bubbleSFX);
        if(_triggersLeft <= 0)
        {
            ShowReward();
        }
        switch (_triggersLeft)
        {
            case 0: anim.Play("Book_3"); break;
            case 1: anim.Play("Book_2"); break;
            case 2: anim.Play("Book_1");  break;
            default: anim.Play("Book_1"); break;
        }
    }
    public void ShowReward()
    {
        audioSource.PlayOneShot(revealRewardSFX);

        StartCoroutine(RewardSequence());

        IEnumerator RewardSequence()
        {
            FindObjectOfType<HuatiPlayerController>().enabled = false;
            Time.timeScale = 0.2f;
            yield return new WaitForSecondsRealtime(1.5f);
            rewardPopupCanvas.DOFade(1f, 2f).SetUpdate(true);
            rewardSprite.DOFade(255f, 0.5f).SetDelay(1.5f).SetUpdate(true);
            rewardSprite.transform.DOScale(Vector3.one * 1.5f, 1f).SetDelay(1.5f).SetUpdate(true);
            yield return new WaitForSecondsRealtime(3f);
        }
    }
    private void ResetRewardProgress()
    {
        _triggersLeft = triggerCountsToActivate;
        rewardPopupCanvas.alpha = 0;
        rewardSprite.DOFade(0, 0);
        rewardSprite.transform.localScale = Vector3.zero;
        anim.Play("Book_Idle");
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            Trigger();
        }
    }
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) { SwitchReward(rewardSprites[0]); }
        if (Input.GetKeyDown(KeyCode.Alpha2)) { SwitchReward(rewardSprites[1]); }
        if (Input.GetKeyDown(KeyCode.Alpha3)) { SwitchReward(rewardSprites[2]); }
        if (Input.GetKeyDown(KeyCode.Alpha4)) { SwitchReward(rewardSprites[3]); }
        if (Input.GetKeyDown(KeyCode.Alpha5)) { SwitchReward(rewardSprites[4]); }

        if (Input.GetKeyDown(KeyCode.Q)) { ResetRewardProgress(); }
        if (Input.GetKeyDown(KeyCode.Z)) { book.transform.position = new Vector3(0, 6, 0); }
        if (Input.GetKeyDown(KeyCode.X)) { book.transform.position = new Vector3(0, 7, 0); }
        if (Input.GetKeyDown(KeyCode.C)) { book.transform.position = new Vector3(0, 8, 0); }
    }
}
