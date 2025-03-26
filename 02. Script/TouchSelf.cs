using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Properties;
using UnityEngine;
using UnityEngine.Events;

public class TouchSelf : MonoBehaviour
{
    [SerializeField] private UnityEvent onClick_correct;
    [SerializeField] private UnityEvent onClick_wrong;
    public void OnClick_Correct()
    {
        onClick_correct.Invoke();
        //Debug.Log($"{name} onClick_correct ½ÇÇàµÊ");
    }
    public void OnClick_Wrong()
    {
        onClick_wrong.Invoke();
        //Debug.Log($"{name} onClick_wrong ½ÇÇàµÊ");
    }

    private void OnTriggerEnter(Collider other)
    {
        GameObject self = this.gameObject;
        self.GetComponent<BoxCollider>().enabled = false;
        //ºÎµ÷Èù ¿ÀºêÁ§Æ®ÀÇ ºÎ¸ð ¿ÀºêÁ§Æ®¸¦ °¡Á®¿È
        Transform parentTransform = other.transform.parent;
        string Item = parentTransform.gameObject.GetComponent<TextMeshProUGUI>().text;
        string selfTxt = self.GetComponent<TextMeshProUGUI>().text;
        if (other.CompareTag("Quiz"))
        {
            if(Item == selfTxt)
            {
                parentTransform.GetComponent<TextMeshProUGUI>().enabled = true;
                other.gameObject.SetActive(false);
                OnClick_Correct();
            }
            else
            {               
                OnClick_Wrong();
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        GameObject self = this.gameObject;
        self.GetComponent<BoxCollider>().enabled = true;
    }
}
