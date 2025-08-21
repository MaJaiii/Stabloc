using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ActionTimer : MonoBehaviour
{
    [SerializeField] Image[] overallTimer;
    [SerializeField] Image[] bonusTimer;
    [SerializeField] TextMeshProUGUI blockCountText;
    [SerializeField] public float timer;
    [SerializeField] BlockAction blockAction;
    [SerializeField] int maxTime;

    ScoreSystem scoreSystem;
    public int blockCount = 0;
    bool isRecovery = false;
    public bool isGameOver = false;
    int chain;

    private void Start()
    {
        scoreSystem = GetComponent<ScoreSystem>();
    }

    private void Update()
    {
        if (GameStatus.gameState == GAME_STATE.TRANSITIONING) isGameOver = true;
        if (isGameOver) return;
        if (isRecovery)
        {
            timer += Time.deltaTime * 15;
            if (timer > maxTime) 
            { 
                timer = maxTime; 
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
            overallTimer[i].fillAmount = Mathf.Max(0, Mathf.Min(6, timer)) / 6;
            bonusTimer[i].fillAmount = Mathf.Max(0, Mathf.Min(maxTime, timer)) / maxTime;
            bonusTimer[i].color = blockAction.colorHistory[blockAction.colorHistory.Length - 1];
        }
        blockCountText.text = $"{blockCount} blocks placed";
    }

    public void RecoveryTimer()
    {
        isRecovery = true;
    }

    public void AddPoint(int score = 0)
    {
        if (scoreSystem == null)
        {
            Debug.LogError("scoreSystem is not assigned!");
            return;
        }


    }
}
