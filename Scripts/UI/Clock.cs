using UnityEngine;
using UnityEngine.UI;

public class Clock : MonoBehaviour
{
    public Text clockTimeText; 
    public Image clockFillImage; 
    public Text dayText;
    public DailyCycle dailyCycle; 

    void Start()
    {
        // DailyCycle의 OnDayChanged 이벤트에 핸들러 추가
        dailyCycle.OnDayChanged += UpdateDayText;
        UpdateDayText(dailyCycle.day);
    }

    void Update()
    {
        UpdateClock();
    }

    void UpdateClock()
    {
        // DailyCycle의 time 값을 24시간 형식으로 변환
        float hours = dailyCycle.time * 24;
        int hourInt = Mathf.FloorToInt(hours);
        int minutesInt = Mathf.FloorToInt((hours - hourInt) * 60);

        string hourString = hourInt.ToString("00");
        string minuteString = minutesInt.ToString("00");

        clockTimeText.text = hourString + ":" + minuteString;

        // time 값을 clockFillImage의 fillAmount로 설정
        clockFillImage.fillAmount = dailyCycle.time;
    }

    // DayText 업데이트 메서드
    void UpdateDayText(int day)
    {
        dayText.text = "Day: " + day;
    }
}
