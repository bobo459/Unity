using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // UI를 사용할 때 필요

public class GameManager : MonoBehaviour
{
    public GameObject mainImage;    // 이미지를 담아두는 GameObject
    public Sprite gameOverSpr;      // GAME OVER 이미지
    public Sprite gameClearSpr;     // GAME CLEAR 이미지
    public GameObject panel;        // 패널
    public GameObject restartButton;    // RESTART 버튼
    public GameObject nextButton;       // NEXT 버튼

    Image titleImage;                   // 이미지를 표시하는 Image 컴포넌트

    // 시간제한 추가
    public GameObject timeBar;      // 시간 표시 이미지
    public GameObject timeText;     // 시간 텍스트
    TimeController timeCnt;         // TimeController

    // 점수 추가
    public GameObject scoreText;    // 점수 텍스트
    public static int totalScore;   // 점수 총합
    public int stageScore = 0;      // 스테이지 점수


    public AudioClip meGameOver;  //게임 오버
    public AudioClip meGameClear; //게임 클리어


    // Start is called before the first frame update
    void Start()
    {
        // 이미지 숨기기
        Invoke("InactiveImage", 1.0f);  // Invoke c#에서 사용하는 비동기 함수. ?초 뒤에 실행시킴
        // 버튼 (패널)을 숨기기
        panel.SetActive(false);

        // 시간제한 추가
        // TimeController 가져옴
        timeCnt = GetComponent<TimeController>();
        if ( timeCnt != null)
        {
            if(timeCnt.gameTime == 0.0f)
            {
                timeBar.SetActive(false); // 시간제한이 없으면 숨김
            }
        }

        // 점수 추가
        UpdateScore();
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerController.gameState == "gameclear")
        {
            // 게임 클리어
            mainImage.SetActive(true); // 이미지 표시
            panel.SetActive(true); // 버튼 (패널)을 표시
            // RESTART 버튼 무효화
            Button bt = restartButton.GetComponent<Button>();
            bt.interactable = false; // true 시 버튼 활성화

            mainImage.GetComponent<Image>().sprite = gameClearSpr; // Image의 스프라이트만 바꿈
            PlayerController.gameState = "gameend";

            // 시간제한 추가
            if(timeCnt != null)
            {
                timeCnt.isTimeOver = true; // 시간 카운트 중지

                // 점수 추가
                // 정수에 할당하여 소수점을 버린다
                int time = (int)timeCnt.displayTime;
                totalScore += time * 10;    // 남은 시간을 점수에 더한다
            }

            // 점수 추가
            totalScore += stageScore;
            stageScore = 0;
            UpdateScore(); // 점수 갱신


            // +++사운드 재생 추가+++
            //사운드 재생
            AudioSource soundPlayer = GetComponent<AudioSource>();
            if(soundPlayer != null)
            {
                //BGM정지
                soundPlayer.Stop();
                soundPlayer.PlayOneShot(meGameClear);
            }


        }
        else if (PlayerController.gameState == "gameover")
        {
            // 게임 오버
            mainImage.SetActive(true);  // 이미지 표시
            panel.SetActive(true);      // 버튼 (패널) 을 표시
            // NEXT 버튼을 비활성
            Button bt = nextButton.GetComponent<Button>();
            bt.interactable = false;
            mainImage.GetComponent<Image>().sprite = gameOverSpr;
            PlayerController.gameState = "gameend";

            // 시간제한 추가
            if(timeCnt != null)
            {
                timeCnt.isTimeOver = true; // 시간 카운트 중지
            }



            // +++사운드 재생 추가+++
            //사운드 재생
            AudioSource soundPlayer = GetComponent<AudioSource>();
            if(soundPlayer != null)
            {
                //BGM정지
                soundPlayer.Stop();
                soundPlayer.PlayOneShot(meGameOver);
            }




        }
        else if (PlayerController.gameState == "playing")
        {
            // 게임 중
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            // PlayerController 가져오기
            PlayerController playerCnt = player.GetComponent<PlayerController>();

            // 시간제한 추가
            // 시간 갱신
            if(timeCnt != null)
            {
                // 정수에 할당하여 소수점을 이하를 버림
                int time = (int)timeCnt.displayTime;
                // 시간 갱신
                timeText.GetComponent<Text>().text = time.ToString();
                // 타임 오버
                if(time == 0)
                {
                    playerCnt.GameOver(); // 게임 오버
                }
            }

            // 점수 추가
            if(playerCnt.score != 0)
            {
                stageScore += playerCnt.score;
                playerCnt.score = 0;
                UpdateScore();
            }
        }
    }
    // 이미지 숨기기
    void InactiveImage()
    {
        mainImage.SetActive(false);
    }

    // 점수 추가
    void UpdateScore()
    {
        int score = stageScore + totalScore;
        scoreText.GetComponent<Text>().text = score.ToString();
    }
}
