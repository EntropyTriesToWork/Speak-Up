using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RewardController : MonoBehaviour
{
    public Image rewardSprite;
    public List<Sprite> rewardSprites;

    public void SwitchReward(Sprite sprite)
    {
        rewardSprite.sprite = sprite;
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) { SwitchReward(rewardSprites[0]); }
        if (Input.GetKeyDown(KeyCode.Alpha2)) { SwitchReward(rewardSprites[1]); }
        if (Input.GetKeyDown(KeyCode.Alpha3)) { SwitchReward(rewardSprites[2]); }
        if (Input.GetKeyDown(KeyCode.Alpha4)) { SwitchReward(rewardSprites[3]); }
        if (Input.GetKeyDown(KeyCode.Alpha5)) { SwitchReward(rewardSprites[4]); }
    }
}
