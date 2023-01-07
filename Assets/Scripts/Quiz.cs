using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Quiz : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI questionText;
    [SerializeField] QuestionSO question;
    [SerializeField] GameObject[] answerButtons;
    int correctAnswerIndex;
    [SerializeField] Sprite defaultAnswerSprite;
    [SerializeField] Sprite correctAnswerSprite;
    [SerializeField] Sprite wrongAnswerSprite;

    void Start()
    {
        GetNextQuestion();
    }

    public void OnAnswerSelected(int index)
    {
        Image buttonImage = answerButtons[correctAnswerIndex].GetComponent<Image>();
        buttonImage.sprite = correctAnswerSprite;
        if(index == correctAnswerIndex) {
            questionText.text = "Correct";
        } else {
            string correctAnswer = question.getAnswer(correctAnswerIndex);
            questionText.text = "Wrong!\nCorrect asnwer is " + correctAnswer;
            Image buttonImageClicked = answerButtons[index].GetComponent<Image>();
            buttonImageClicked.sprite = wrongAnswerSprite;
        }

        SetButtonState(false);
    }

    private void DisplayQuestion()
    {
        questionText.text = question.GetQuestion();
        correctAnswerIndex = question.GetCorrectAnswerIndex();
        for(int i = 0; i < 4; i++) {
            TextMeshProUGUI buttonText = answerButtons[i].GetComponentInChildren<TextMeshProUGUI>();
            buttonText.text = question.getAnswer(i);
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
        SetButtonState(true);
        SetDefaultButtonSprites();
        DisplayQuestion();
    }

    private void SetButtonState(bool state)
    {
        for(int i = 0; i < 4; i++) {
            Button button = answerButtons[i].GetComponent<Button>();
            button.interactable = state;
        }
    }
}
