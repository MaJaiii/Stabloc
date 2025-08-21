using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class ScoreSystem : MonoBehaviour
{
    [SerializeField] BlockAction blockAction;
    [SerializeField] TextMeshProUGUI nowScore;
    int prevScore;
    public int score;
    bool isCountUp;
    Sequence sequence;

    ActionTimer actionTimer;

    private void Start()
    {
        actionTimer = GetComponent<ActionTimer>();
    }
    private void Update()
    {
        if (isCountUp) nowScore.SetText("{0:000000}", prevScore);
    }

    public void ModifyScore (int gain, int product = 0)
    {
        prevScore = score;
        if (product >= 1) gain = Mathf.CeilToInt(score * (product - 1) * .1f);
        score += gain;
        if (isCountUp) sequence.Kill(true);
        CountUpAnim();
    }

    void CountUpAnim()
    {
        isCountUp = true;
        sequence = DOTween.Sequence().Append(DOTween.To(() => prevScore, num => prevScore = num, score, .5f)).AppendInterval(.1f).AppendCallback(() => isCountUp = false);
    }
}
