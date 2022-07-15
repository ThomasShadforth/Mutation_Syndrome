using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIFade : MonoBehaviour
{
    public static UIFade instance;

    public Image fadeImage;
    public float fadeSpeed;

    public bool shouldFadeToBlack;
    public bool shouldFadeFromBlack;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (shouldFadeToBlack)
        {
            fadeImage.color = new Color(fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, Mathf.MoveTowards(fadeImage.color.a, 1f, fadeSpeed * Time.deltaTime));

            if(fadeImage.color.a == 1f)
            {
                shouldFadeToBlack = false;
                
            }
        }

        if (shouldFadeFromBlack)
        {
            fadeImage.color = new Color(fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, Mathf.MoveTowards(fadeImage.color.a, 0f, fadeSpeed * Time.deltaTime));

            if (fadeImage.color.a == 0f)
            {
                shouldFadeFromBlack = false;
                fadeImage.gameObject.SetActive(false);
            }
        }
    }

    public void FadeToBlack()
    {
        fadeImage.gameObject.SetActive(true);
        shouldFadeToBlack = true;
        shouldFadeFromBlack = false;
    }

    public void FadeFromBlack()
    {
        
        shouldFadeToBlack = false;
        shouldFadeFromBlack = true;
    }
}
