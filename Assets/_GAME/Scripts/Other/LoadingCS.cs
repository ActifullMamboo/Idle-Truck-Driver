using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingCS : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        loadText = GetComponent<TextMeshProUGUI>();
        StartCoroutine(TextRoutine());
    }
    TextMeshProUGUI loadText;
   IEnumerator TextRoutine()
    {
        string mainText = "LOADING";

            loadText.text = mainText + ".";
            yield return new WaitForSeconds(0.2f);
            loadText.text = mainText + "..";
            yield return new WaitForSeconds(0.2f);
            SceneManager.LoadSceneAsync(1);

            loadText.text = mainText + "...";
            yield return new WaitForSeconds(0.2f);

        
    }
}
