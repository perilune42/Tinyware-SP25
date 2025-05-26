using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public static Timer Instance;
    private float timeRemaining; // seconds
    [SerializeField] private float initialTime = 10f;
    [SerializeField] public float TimeRefund = 5f;
    [SerializeField] public float BaseTimeReward = 15f;
    [SerializeField] public float TimeRewardPerRound = 5f;
    [SerializeField] public float BaseDrawPenalty = 5f;
    [SerializeField] public float MaxDrawPenalty = 15f;
    [SerializeField] public float DrawPenaltyPercentage = 0.1f; // % of time reduced

    public bool IsCounting = false;
    private bool ended = false;
    private bool lowTime;

    [SerializeField] TMP_Text timerText;
    public Action OnTimerEnd;

    [SerializeField] TMP_Text roundNumText;

    [SerializeField] TimerChangePopup posTimePopup, negTimePopup;
    [SerializeField] Transform popupSpawnLocation;

    public Action<float> OnTimerTick; // every 0.5s

    bool flashOn;

    [SerializeField] Image clockImage;
    [SerializeField] Sprite[] clockSprites;

    private void Awake()
    {
        
        Instance = this;
        GameManager.Instance.OnNewRound += (roundNum) => roundNumText.text = $"ROUND {roundNum}";
    }

    public void StartTimer()
    {
        timeRemaining = initialTime;
        IsCounting = true;
    }

    private void Update()
    {
        if (ended) return;
        if (IsCounting)
        {
            timeRemaining -= Time.deltaTime;
            if (timeRemaining <= 0)
            {
                EndTimer();
                return;
            }
            if (Mathf.Floor(timeRemaining * 2) % 2 == 0)
            {
                if (flashOn)
                {
                    OnTimerTick?.Invoke(timeRemaining);
                    flashOn = false;
                }
                timerText.enabled = false;
                return;
            }
        }
        if (!flashOn)
        {
            flashOn = true;
            OnTimerTick?.Invoke(timeRemaining);
        }
        timerText.enabled = true;
        timerText.text = FormatTime(timeRemaining);
        if (timeRemaining < 30f)
        {
            if (!lowTime)
            {
                lowTime = true;
                SoundPlayer.PlayTime();
            }
            timerText.color = Color.red;
        }
        else
        {
            lowTime = false;
            timerText.color = Color.white;
        }

        int clkIndex = (int)(Mathf.Clamp((timeRemaining / (initialTime * 2)), 0f, 0.99f) * clockSprites.Length);
        clockImage.sprite = clockSprites[clkIndex];
    }


    private void EndTimer()
    {
        timerText.enabled = true;
        timerText.text = FormatTime(0);
        OnTimerEnd?.Invoke();
        timerText.color = Color.red;
        IsCounting = false;
        ended = true;
    }

    public void StopCounting()
    {
        IsCounting = false;
    }
    
    public void AddTime(float time, string desc)
    {
        timeRemaining += time;
        var popup = Instantiate(posTimePopup);
        popup.transform.SetParent(popupSpawnLocation, false);
        popup.Display(time, desc);
    }

    public void RemoveTime(float time, string desc)
    {
        timeRemaining -= time;
        var popup = Instantiate(negTimePopup);
        popup.transform.SetParent(popupSpawnLocation, false);
        popup.Display(time, desc);
    }

    public void ApplyDrawPenalty()
    {

        RemoveTime(GetDrawPenalty(), "");
    }

    public float GetDrawPenalty()
    {
        float penalty = Mathf.Clamp(timeRemaining * DrawPenaltyPercentage, BaseDrawPenalty, MaxDrawPenalty);
        penalty = Mathf.Round(penalty);
        return penalty;
    }

    public static string FormatTime(float seconds)
    {
        int minutes = Mathf.FloorToInt(seconds / 60f);
        int secs = Mathf.FloorToInt(seconds % 60f);
        return $"{minutes}:{secs:00}";
    }

}