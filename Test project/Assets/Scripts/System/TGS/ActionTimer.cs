using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ActionTimer : MonoBehaviour
{
    [SerializeField] Image[] overallTimer;
    [SerializeField] Image[] bonusTimer;
    [SerializeField] TextMeshProUGUI durationText;
    [SerializeField] TextMeshProUGUI blockCountText;
    [SerializeField] float timer;
    [SerializeField] BlockAction blockAction;

    float duration = 0;
    public int blockCount = 0;
    bool isRecovery = false;
    public bool isGameOver = false;


    private void Update()
    {
        if (GameStatus.gameState == GAME_STATE.TRANSITIONING) isGameOver = true;
        if (isGameOver) return;
        if ((blockAction.flagStatus & FlagsStatus.FirstDrop) != FlagsStatus.FirstDrop)
        {
            duration += Time.deltaTime;
            float sec = duration % 60;
            float min = (sec - sec % 60) / 60;
            durationText.text = $"{min.ToString("00")}:{sec.ToString("00.000")}";
        }
        if (isRecovery)
        {
            timer += Time.deltaTime * 15;
            if (timer > 15) 
            { 
                timer = 15; 
                isRecovery = false;
            }
        }
        else if (timer > 0 && (blockAction.flagStatus & FlagsStatus.PressDownButton) != FlagsStatus.PressDownButton)
        {
            timer -= Time.deltaTime;
            if (timer < 0)
            {
                timer = 0;
                GameObject[] obj = GameObject.FindGameObjectsWithTag("Ground");
                foreach (GameObject obj2 in obj)
                {
                    Destroy(obj2);
                }
                
            }
        }
        for (int i = 0; i < 2; i++)
        {
            overallTimer[i].fillAmount = Mathf.Max(0, Mathf.Min(9, timer)) / 9;
            bonusTimer[i].fillAmount = Mathf.Max(0, Mathf.Min(15, timer)) / 15;
            bonusTimer[i].color = blockAction.colorHistory[blockAction.colorHistory.Length - 1];
        }
        blockCountText.text = $"{blockCount} blocks placed";
    }

    public void RecoveryTimer()
    {
        isRecovery = true;
    }
}
