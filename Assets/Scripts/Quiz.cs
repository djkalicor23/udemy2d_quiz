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
        questionText.text = question.GetQuestion();

        for(int i = 0; i < 4; i++) {
            TextMeshProUGUI buttonText = answerButtons[i].GetComponentInChildren<TextMeshProUGUI>();
            buttonText.text = question.getAnswer(i);
        }
    }

    public void OnAnswerSelected(int index) {
        int questionCorrectAnswerIndex = question.GetCorrectAnswerIndex();
        Image buttonImage = answerButtons[questionCorrectAnswerIndex].GetComponent<Image>();
        buttonImage.sprite = correctAnswerSprite;
        if(index == questionCorrectAnswerIndex) {
            questionText.text = "Correct";
        } else {
            string correctAnswer = question.getAnswer(questionCorrectAnswerIndex);
            questionText.text = "Wrong!\nCorrect asnwer is " + correctAnswer;
            Image buttonImageClicked = answerButtons[index].GetComponent<Image>();
            buttonImageClicked.sprite = wrongAnswerSprite;
        }
    }
}
