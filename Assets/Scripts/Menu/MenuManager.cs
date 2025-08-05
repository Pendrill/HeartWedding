using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public Febucci.UI.Core.TAnimCore textAnimator;
    public Febucci.UI.Core.TypewriterCore typewriter;
    public Image panel;
    string title = "<bounce>Q-πd_001</bounce>";
    bool startGame = true;
    public AudioSource music;
    public AudioSource buttonAudio;
    public AudioClip hover, click;
    public RectTransform credits;
    // Start is called before the first frame update
    void Start()
    {
        SetPanel(true, new Color(0, 0, 0, 1));
        SetPanel(false, new Color(0, 0, 0, 0));
        music.DOFade(1, 2f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SetPanel(bool automatic, Color color)
    {
        if (automatic)
        {
            panel.color = color;
        }
        else
        {
            if (!startGame)
            {
                music.DOFade(0, 0.6f).SetEase(Ease.InCubic);
            }
            panel.DOColor(color, 0.6f).OnComplete(PanelChanged);
        }
    }

    void SetTitle()
    {
        typewriter.ShowText(title);
    }

    public void NewGameClicked()
    {
        //ButtonClick();
        SetPanel(false, new Color(0, 0, 0, 1));
    }

    void PanelChanged()
    {
        if(startGame)
        {
            startGame = false;
            SetTitle();
        } 
        else
        {
            LoadNextScene();
        }
    }

    void LoadNextScene()
    {
        SceneManager.LoadScene(1);
    }

    public void ButtonHover()
    {
        buttonAudio.clip = hover;
        buttonAudio.Play();
    }

    public void ButtonClick()
    {
        buttonAudio.clip = click;
        buttonAudio.Play();
    }

    public void ActivateCredits()
    {
        credits.DOAnchorPosY(2342, 12f).SetEase(Ease.Linear);
    }

    public void DeActivateCredits()
    {
        credits.DOAnchorPosY(-862, 0f, true);
    }

    public void QuitOut()
    {
        Application.Quit();
    }
}
