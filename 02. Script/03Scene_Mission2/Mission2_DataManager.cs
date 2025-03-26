using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;
using TMPro;
using System.Linq;

public class Mission2_DataManager : MonoBehaviour
{
    [Header("UI �� �ܺ� ����")]
    [SerializeField] private Mission2_UIManager mission2_UIManager;

    [Header("���� ������")]
    public List<string> wordList = new List<string>();           // ���� ������ �ܾ� ����Ʈ
    private List<string> currentWords = new List<string>();      // �̹� ���õ� �ܾ� ����Ʈ
    private List<string> quizSentenceList = new List<string>();  // ���� ���� ������
    public List<GameObject> randomQuizObjects = new List<GameObject>(); // �������� ���õ� ���� ������Ʈ ����Ʈ

    [Header("���� ���� ����")]
    public int currentQuizIndex = 0;  // ���� ���� �ε���
    public int currentBlankIndex = 0; // ���� ������ ��ĭ �ε���
    private string[] correctWords;    // �ܾ� �и��� �ӽ� �迭

    #region Public Methods

    // �̼� ���� ���� ������
    public void SetupQuiz()
    {
        RandomizeBlankIndex();          // ���� ��ũ ����
        SetupQuestionBoard();           // ���� �ؽ�Ʈ ���� �� ���� ����
    }

    // �������� ���� 5�� �̱�
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

    // ������ �ܾ 3�� �̰� ���� ������ ����
    private void GenerateChoiceWords()
    {
        wordList.Clear();

        // ��� ������ �ܾ�� ������ �ߺ� ����
        HashSet<string> uniqueWords = ExtractUniqueWords(quizSentenceList);

        // currentWords, currentAnswer ���� �� �ĺ� �ܾ� ����
        string currentAnswer = Mission2_GameManager.instance.currentAnswer;

        List<string> candidateWords = uniqueWords
            .Except(currentWords)
            .Where(word => !word.Equals(currentAnswer))
            .ToList();

        // 3�� ���� ���� + ���� �߰� + ����
        List<string> randomChoices = GetRandomElements(candidateWords, 3);
        randomChoices.Add(currentAnswer);
        wordList = ShuffleList(randomChoices);
    }

    // ��ĭ �ε��� ���� ����
    private void RandomizeBlankIndex()
    {
        GameObject parentObj = mission2_UIManager.WordGameObejcts[currentQuizIndex];
        int childCount = parentObj.transform.childCount;
        currentBlankIndex = Random.Range(0, childCount-1); //������ ��ĭ�� �ѱ�
    }

    // ���� ���� ���� �ؽ�Ʈ �¾� �� ���� ó��
    private void SetupQuestionBoard()
    {
        GameObject parentObj = randomQuizObjects[currentQuizIndex];
        Transform blankTransform = parentObj.transform.GetChild(currentBlankIndex);

        // �ؽ�Ʈ �� ���� ǥ�� ó��
        //mission2_UIManager.BlankAnswer = blankTransform.gameObject;
        var textComponent = blankTransform.GetComponent<TextMeshProUGUI>();
        textComponent.enabled = false;
        blankTransform.GetChild(0).gameObject.SetActive(true);
        parentObj.SetActive(true);

        // ���� ����
        Mission2_GameManager.instance.currentAnswer = textComponent.text;

        // ������ �ܾ� ó��
        GenerateChoiceWords();

        // ������ UI�� �ܾ� ����
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

    // ��� ���忡�� �ߺ� ���� �ܾ� ����
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

    // ���ϴ� ������ŭ ���� �ܾ� �̱�
    private List<string> GetRandomElements(List<string> sourceList, int count)
    {
        if (sourceList == null || sourceList.Count == 0)
            return new List<string>();

        if (sourceList.Count <= count)
            return new List<string>(sourceList);

        List<string> shuffledList = ShuffleList(sourceList);
        return shuffledList.Take(count).ToList();
    }

    // ����Ʈ ���ø� (Fisher-Yates �˰���)
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
