using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;
using TMPro;
using System.Linq;

public class Mission2_DataManager : MonoBehaviour
{
    [Header("UI 및 외부 참조")]
    [SerializeField] private Mission2_UIManager mission2_UIManager;

    [Header("게임 데이터")]
    public List<string> wordList = new List<string>();           // 최종 선택지 단어 리스트
    private List<string> currentWords = new List<string>();      // 이미 선택된 단어 리스트
    private List<string> quizSentenceList = new List<string>();  // 퀴즈 문장 데이터
    public List<GameObject> randomQuizObjects = new List<GameObject>(); // 랜덤으로 선택된 문제 오브젝트 리스트

    [Header("문제 진행 관련")]
    public int currentQuizIndex = 0;  // 현재 문제 인덱스
    public int currentBlankIndex = 0; // 현재 문제의 빈칸 인덱스
    private string[] correctWords;    // 단어 분리용 임시 배열

    #region Public Methods

    // 미션 퀴즈 세팅 진입점
    public void SetupQuiz()
    {
        RandomizeBlankIndex();          // 랜덤 블랭크 선택
        SetupQuestionBoard();           // 문제 텍스트 셋팅 및 정답 관리
    }

    // 랜덤으로 문제 5개 뽑기
    public void GenerateRandomQuizList()
    {
        quizSentenceList.Clear();
        randomQuizObjects.Clear();

        List<int> indices = Enumerable.Range(0, mission2_UIManager.WordGameObejcts.Length).ToList();
        indices = ShuffleList(indices);

        int maxCount = Mathf.Min(5, indices.Count);

        for (int i = 0; i < maxCount; i++)
        {
            int index = indices[i];
            GameObject selectedObj = mission2_UIManager.WordGameObejcts[index];

            randomQuizObjects.Add(selectedObj);
            quizSentenceList.Add(selectedObj.name);
        }
    }

    #endregion

    #region Private Methods

    // 선택지 단어를 3개 뽑고 정답 포함해 섞기
    private void GenerateChoiceWords()
    {
        wordList.Clear();

        // 모든 문장을 단어로 나누고 중복 제거
        HashSet<string> uniqueWords = ExtractUniqueWords(quizSentenceList);

        // currentWords, currentAnswer 제거 후 후보 단어 생성
        string currentAnswer = Mission2_GameManager.instance.currentAnswer;

        List<string> candidateWords = uniqueWords
            .Except(currentWords)
            .Where(word => !word.Equals(currentAnswer))
            .ToList();

        // 3개 랜덤 선택 + 정답 추가 + 셔플
        List<string> randomChoices = GetRandomElements(candidateWords, 3);
        randomChoices.Add(currentAnswer);
        wordList = ShuffleList(randomChoices);
    }

    // 빈칸 인덱스 랜덤 선택
    private void RandomizeBlankIndex()
    {
        GameObject parentObj = mission2_UIManager.WordGameObejcts[currentQuizIndex];
        int childCount = parentObj.transform.childCount;
        currentBlankIndex = Random.Range(0, childCount-1); //마지막 빈칸은 한글
    }

    // 현재 문제 보드 텍스트 셋업 및 정답 처리
    private void SetupQuestionBoard()
    {
        GameObject parentObj = randomQuizObjects[currentQuizIndex];
        Transform blankTransform = parentObj.transform.GetChild(currentBlankIndex);

        // 텍스트 및 라인 표시 처리
        //mission2_UIManager.BlankAnswer = blankTransform.gameObject;
        var textComponent = blankTransform.GetComponent<TextMeshProUGUI>();
        textComponent.enabled = false;
        blankTransform.GetChild(0).gameObject.SetActive(true);
        parentObj.SetActive(true);

        // 정답 저장
        Mission2_GameManager.instance.currentAnswer = textComponent.text;

        // 선택지 단어 처리
        GenerateChoiceWords();

        // 선택지 UI에 단어 삽입
        for (int i = 0; i < mission2_UIManager.ChoiceText.Length; i++)
        {
            mission2_UIManager.ChoiceText[i].text = wordList[i];
        }
    }

    private void ResetQuiz()
    {

    }
    #endregion

    #region Utility Methods

    // 모든 문장에서 중복 없는 단어 추출
    private HashSet<string> ExtractUniqueWords(List<string> sentences)
    {
        HashSet<string> uniqueWords = new HashSet<string>();

        foreach (string sentence in sentences)
        {
            string cleanSentence = Regex.Replace(sentence, @"[^\w\s]", "");
            string[] words = cleanSentence.Split(new char[] { ' ' }, System.StringSplitOptions.RemoveEmptyEntries);

            foreach (string word in words)
            {
                uniqueWords.Add(word);
            }
        }

        return uniqueWords;
    }

    // 원하는 개수만큼 랜덤 단어 뽑기
    private List<string> GetRandomElements(List<string> sourceList, int count)
    {
        if (sourceList == null || sourceList.Count == 0)
            return new List<string>();

        if (sourceList.Count <= count)
            return new List<string>(sourceList);

        List<string> shuffledList = ShuffleList(sourceList);
        return shuffledList.Take(count).ToList();
    }

    // 리스트 셔플링 (Fisher-Yates 알고리즘)
    private List<T> ShuffleList<T>(List<T> list)
    {
        List<T> shuffledList = new List<T>(list);
        int n = shuffledList.Count;

        for (int i = 0; i < n; i++)
        {
            int randomIndex = Random.Range(i, n);
            (shuffledList[i], shuffledList[randomIndex]) = (shuffledList[randomIndex], shuffledList[i]);
        }

        return shuffledList;
    }

    #endregion
}
