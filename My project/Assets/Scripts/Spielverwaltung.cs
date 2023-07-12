using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using TMPro;

public class Spielverwaltung : MonoBehaviour
{
    
    public List<GameObject> objectsToSpawn;
    public GameObject bomb;
    public GameObject countdown;
    public GameObject score;
    public GameObject hS;
    private TMP_Text countdownText;
    private TMP_Text scoreText;
    private TMP_Text highScoreText;
    private int time;
    private int currentScore;
    private int highScore;
    public GameObject startFruit;
    public GameObject quitFruit;
    public GameObject clappers;


    void Start () {
        countdownText = countdown.GetComponent<TMP_Text>();
        scoreText = score.GetComponent<TMP_Text>();
        highScoreText = hS.GetComponent<TMP_Text>();
        highScore = PlayerPrefs.GetInt("highscore");
        highScoreText.text = "Highscore:\n" + highScore.ToString();
    }

    public void StartRound(){
        clappers.SetActive(false);
        time = 120;
        currentScore = 0;
        countdownText.text = time.ToString();
        scoreText.text = "Punktestand:\n" + currentScore.ToString();
        InvokeRepeating("SpawnFruit", 3.0f, 3.0f);
        InvokeRepeating("cd", 1.0f, 1.0f);
    }

    public void ChangeScore(int change){
        if(time > 0){
            currentScore += change;
            scoreText.text = "Punktestand:\n" + currentScore.ToString();
        }
    }

    void cd(){
        time--;
        countdownText.text = time.ToString();
        if(time <= 3){
            CancelInvoke();
            InvokeRepeating("cd", 1.0f, 1.0f);
        }
        if(time == 0){
            CancelInvoke();
            if(currentScore > highScore){
                highScore = currentScore;
                PlayerPrefs.SetInt("highscore", highScore);
                highScoreText.text = "Highscore:\n" + highScore.ToString();
                clappers.SetActive(true);
                Animator[] anims = clappers.GetComponentsInChildren<Animator>();
                for(int i = 0; i < anims.Length; i++){
                    anims[i].SetTrigger("clap");
                }
            }
            startFruit.SetActive(true);
            quitFruit.SetActive(true);
        }
    }

    void SpawnFruit(){
        for(int x = 0; x < 5; x++){
            GameObject randomFruit;
            if(Random.Range(0.0f, 100.0f) < 8.0f){
                randomFruit = bomb;
            }else{
                randomFruit = objectsToSpawn[Random.Range(0 , objectsToSpawn.Count)];
            }
            Vector3 pos = transform.position;
            pos.x += x;
            pos.z = pos.z - Mathf.Abs(2-x)*0.7f;
            GameObject newFruit = Instantiate(randomFruit, pos, randomFruit.transform.rotation);
            
            Vector3 randomVector = new Vector3(Random.Range(-0.1f, 0.1f), 1.0f, -0.06f);
            newFruit.GetComponent<Rigidbody>().AddForce(randomVector * Random.Range(7.5f, 9.5f), ForceMode.Impulse);
            newFruit.GetComponent<Rigidbody>().AddTorque(Random.Range(-0.1f, 0.1f), Random.Range(-0.1f, 0.1f), Random.Range(-0.1f, 0.1f), ForceMode.Impulse);
            Destroy(newFruit, 3.0f);
        }
    }
}