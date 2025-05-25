using System;
using TMPro;
using UnityEditor.Build.Content;
using UnityEngine;

public class Timer : MonoBehaviour
{
    public static Timer Instance;
    private float timeRemaining; // seconds
    [SerializeField] private float initialTime = 10f;
    [SerializeField] public float TimeRefund, DrawPenalty;

    public bool IsCounting = false;
    private bool ended = false;

    [SerializeField] TMP_Text timerText;
    public Action OnTimerEnd;

    [SerializeField] TMP_Text roundNumText;

    [SerializeField] TimerChangePopup posTimePopup, negTimePopup;
    [SerializeField] Transform popupSpawnLocation;

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
                timerText.enabled = false;
                return;
            }
        }
        timerText.enabled = true;
        timerText.text = FormatTime(timeRemaining);
        if (timeRemaining < 30f)
        {
            timerText.color = Color.red;
        }
        else
        {
            timerText.color = Color.white;
        }
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

    public static string FormatTime(float seconds)
    {
        int minutes = Mathf.FloorToInt(seconds / 60f);
        int secs = Mathf.FloorToInt(seconds % 60f);
        return $"{minutes}:{secs:00}";
    }

}