using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GameManager : SingleTon<GameManager>
{
    [SerializeField] List<GameObject> ghosts;
    [SerializeField] GameObject eventSystem; 
    public Transform endLineCollider;
    public ArcadeRacerController controller;

    [SerializeField] Vector2[] sizes;

    public int day;
    public int coin;

    public int countDown;
    public bool racing = false;

    public float latestTime;
    float currentTime = 0;

    [SerializeField] Image countDownSprite;
    [SerializeField] Sprite[] countDownImages;
    [SerializeField] TextMeshProUGUI timer;

    [SerializeField] CinemachineVirtualCamera cmvc;

    public int upgradeDmg;
    public int upgradeHp;

    AniSoundPlayer sp;


    public void Start()
    {
        sp = GetComponent<AniSoundPlayer>();
        gameStart();
        if(controller != null) {
            cmvc.Follow = controller.transform;
            cmvc.LookAt = controller.transform;
        }
        
    }
    private void Update()
    {
        if (racing)
        {
            currentTime += Time.deltaTime;
        }
    }
    public void gameStart()
    {
        Debug.Log("컹");
        racing = false;

        endLineCollider.gameObject.GetComponent<BoxCollider>().enabled = false;

        countDown = 3;


        countDownSprite.gameObject.GetComponent<RectTransform>().sizeDelta = sizes[countDown];
        countDownSprite.gameObject.SetActive(true);
        countDownSprite.sprite = countDownImages[countDown];

        Invoke(nameof(countingDown), 1);
    }
    public void gameStart2()
    {
        GameManager fake = (GameManager)FindObjectOfType(typeof(GameManager));
        if (fake != null && fake != this)
        {
            Destroy(fake.gameObject);
        }
        racing = false;

        endLineCollider.gameObject.GetComponent<BoxCollider>().enabled = false;

        countDown = 3;

        SoundPlayer.instance.startSFX("Ready");
        countDownSprite.gameObject.GetComponent<RectTransform>().sizeDelta = sizes[countDown];
        countDownSprite.gameObject.SetActive(true);
        countDownSprite.sprite = countDownImages[countDown];

        Invoke(nameof(countingDown), 1);
    }
    void countingDown()
    {
        countDown--;

        if (countDown == -1)
        {
            Debug.Log("시작");
            countDownSprite.gameObject.SetActive(false);
            racing = true;
            if (controller != null) controller.gameStart();


            foreach (var i in ghosts)
            {
                i.SetActive(true);
                i.GetComponent<Ghost>().Initialize(i.GetComponent<PlayerReplay>().replayData);
                i.GetComponent<Ghost>().StartReplay();
            }

            SoundPlayer.instance.startBGM("BGM");
            currentTime = 0;

            return;
        }

        countDownSprite.sprite = countDownImages[countDown];
        countDownSprite.gameObject.GetComponent<RectTransform>().sizeDelta = sizes[countDown];

        Invoke(nameof(countingDown), 1);
    }
    public void gameEnd()
    {
        ghosts.Add(GameManager.Instance.controller.gameObject);

        day++;
        controller.gameEnd();
        racing = false;

        if(currentTime > latestTime)
        {
            //게임 오버
            SoundPlayer.instance.startSFX("GameOver");
            SceneManager.LoadScene("gameOver");
        }
        else
        {
            //다음 스테이지

            SceneManager.LoadScene("UpgradeScene");
        }

        latestTime = currentTime;

        controller.gameObject.SetActive(false);
    }
    public void ReloadGameManager()
    {
        gameObject.SetActive(false);
        gameObject.SetActive(true); // OnEnable() or Start() 호출
    }
    public void gameStartReady()
    {
        Invoke(nameof(gameStart2), 0.1f);
    }
}
