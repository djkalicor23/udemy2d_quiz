using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class Quiz : MonoBehaviour
{
    [Header("Questions")]
    [SerializeField] TextMeshProUGUI questionText;
    [SerializeField] List<QuestionSO> questions = new List<QuestionSO>();
    QuestionSO currentQuestion;

    [Header("Answers")]
    [SerializeField] GameObject[] answerButtons;
    int correctAnswerIndex;
    bool hasAnsweredEarly = true;

    [Header("Button Colors")]
    [SerializeField] Sprite defaultAnswerSprite;
    [SerializeField] Sprite correctAnswerSprite;
    [SerializeField] Sprite wrongAnswerSprite;

    [Header("Timer")]
    [SerializeField] Image timerImage;
    Timer timer;

    [Header("Scoring")]
    [SerializeField] TextMeshProUGUI scoreText;
    ScoreKeeper scoreKeeper;

    [Header("Progress bar")]
    [SerializeField] Slider progressBar;

    public bool isComplete;

    void Awake()
    {
        timer = FindObjectOfType<Timer>();
        scoreKeeper = FindObjectOfType<ScoreKeeper>();
        progressBar.maxValue = questions.Count;
        progressBar.value = 0;
        isComplete = false;
    }

    void Start()
    {
        UpdateScore();
    }

    void Update()
    {
        timerImage.fillAmount = timer.fillFraction;
        if(timer.loadNextQuestion)
        {
            if(progressBar.value == progressBar.maxValue) {
                isComplete = true;
                return;
            }
            hasAnsweredEarly = false;
            GetNextQuestion();
            timer.loadNextQuestion = false;
        }
        else if(!hasAnsweredEarly && !timer.isAnsweringQuestion)
        {
            DisplayAnswer(correctAnswerIndex == 0 ? 1 : 0);
            SetButtonState(false);
        }
    }

    public void OnAnswerSelected(int index)
    {
        hasAnsweredEarly = true;
        DisplayAnswer(index);
        SetButtonState(false);
        timer.CancelTimer();
        UpdateScore();
    }

    void UpdateScore()
    {
        scoreText.text = "Score: " + scoreKeeper.GetScore() + "%";
    }

    void DisplayAnswer(int index)
    {
        Image buttonImage = answerButtons[correctAnswerIndex].GetComponent<Image>();
        buttonImage.sprite = correctAnswerSprite;
        if(index == correctAnswerIndex) {
            questionText.text = "Correct";
            scoreKeeper.IncrementCorrectAnswers();
        } else {
            string correctAnswer = currentQuestion.getAnswer(correctAnswerIndex);
            questionText.text = "Wrong!\nCorrect asnwer is " + correctAnswer;
            Image buttonImageClicked = answerButtons[index].GetComponent<Image>();
            buttonImageClicked.sprite = wrongAnswerSprite;
        }
    }

    private void DisplayQuestion()
    {
        questionText.text = currentQuestion.GetQuestion();
        correctAnswerIndex = currentQuestion.GetCorrectAnswerIndex();
        for(int i = 0; i < 4; i++) {
            TextMeshProUGUI buttonText = answerButtons[i].GetComponentInChildren<TextMeshProUGUI>();
            buttonText.text = currentQuestion.getAnswer(i);
        }
    }

    private void SetDefaultButtonSprites()
    {
        for(int i = 0; i < 4; i++) {
            Image buttonImage = answerButtons[i].GetComponent<Image>();
            buttonImage.sprite = defaultAnswerSprite;
        }
    }

    void GetNextQuestion()
    {
        if(questions.Count == 0) return;
        SetButtonState(true);
        SetDefaultButtonSprites();
        GetRandomQuestion();
        DisplayQuestion();
        progressBar.value++;
        scoreKeeper.IncrementQuestionsSeen();
    }

    private void GetRandomQuestion()
    {
        int index = UnityEngine.Random.Range(0, questions.Count);
        currentQuestion = questions[index];

        if(questions.Contains(currentQuestion)) {
            questions.Remove(currentQuestion);
        }
    }

    private void SetButtonState(bool state)
    {
        for(int i = 0; i < 4; i++) {
            Button button = answerButtons[i].GetComponent<Button>();
            button.interactable = state;
        }
    }
}
