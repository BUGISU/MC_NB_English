using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Mission1_DataManager : MonoBehaviour
{
    public int CorrectCount = 0;
    public int QuizCount = 0;
    public string[] word_list_en = new string[] 
    { "Cat", "Ball", "Dog", "Sun", "Tree", "Book", "Fish","Star","Hat"};
    public string[] word_list_kr = new string[]
    { "고양이", "공", "개", "해", "나무", "책", "물고기","별","모자"};
    public List<int> randomIndexList = new List<int>();

    //중복 없는 인덱스 리스트 생성
    public void GenerateRandomIndexList()
    {
        randomIndexList.Clear();
        for (int i = 0; i < word_list_en.Length; i++)
        {
            randomIndexList.Add(i);
        }

        // Fisher-Yates Shuffle
        for (int i = 0; i < randomIndexList.Count-1; i++)
        {
            int randomIndex = Random.Range(i, randomIndexList.Count);
            int temp = randomIndexList[i];
            randomIndexList[i] = randomIndexList[randomIndex];
            randomIndexList[randomIndex] = temp;
        }
        //Debug.Log("랜덤 인덱스 리스트 생성 완료!");
    }
    //다음 문제
    public void NextQuize_SetData()
    {
        QuizCount++;
        Mission1_GameManager.instance.answer_en = word_list_en[QuizCount]; //문제 en
        Mission1_GameManager.instance.answer_kr = word_list_kr[QuizCount]; //문제 kr
    }
   
}
