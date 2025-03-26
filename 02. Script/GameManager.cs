using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance { get; private set; }
    public bool CanTouch = false;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void OnLoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
public static class StringUtil
{
    private static Dictionary<string, KeyValuePair<string, string>> koreanParticles = new Dictionary<string, KeyValuePair<string, string>>
    {
        { "을/를", new KeyValuePair<string, string>("을", "를") },
        { "이/가", new KeyValuePair<string, string>("이", "가") },
        { "은/는", new KeyValuePair<string, string>("은", "는") },
    };

    public static string KoreanParticle(string text)
    {
        foreach (var particle in koreanParticles)
        {
            text = Regex.Replace(text, $@"([\uAC00-\uD7A3]+){particle.Key}", match =>
            {
                string word = match.Groups[1].Value;
                char lastChar = word[word.Length - 1];

                bool hasFinalConsonant = (lastChar - 0xAC00) % 28 > 0;

                return word + (hasFinalConsonant ? particle.Value.Key : particle.Value.Value);
            });
        }
        return text;
    }
}
