using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class SwipeController : MonoBehaviour, IEndDragHandler
{
    [SerializeField] private int maxPage;
    private int currentPage;
    private Vector3 pageWidth;
    private Vector3 targetPos;
    [SerializeField] private RectTransform mapPages;
    [SerializeField] private float tweenTime;
    [SerializeField] LeanTweenType tweenType;
    [SerializeField] private GameObject map1;
    private float dragThreshold;
    [SerializeField] private int thresholdValue; //8 parece ser ideal
    [SerializeField] Button previousButton, nextButton;
    [SerializeField] private TMP_Text mapText;

    [SerializeField] private TMP_Text starsCountText;
    [Tooltip("Quantos niveis existem em cada mapa")]
    [SerializeField] private int[] howManyLevelsInEachMap;

    private void Awake()
    {
        currentPage = 1;
        targetPos = mapPages.localPosition;
        dragThreshold = Screen.width / thresholdValue;
        UpdateArrowButtons();
        mapText.text = "Mapa " + currentPage.ToString();
        TotalStarsCount();
    }

    void Start()
    {
        var rectTransform = map1.GetComponent<RectTransform>(); //para saber o quanto a pagina tem que virar
        float width = rectTransform.rect.width;
        pageWidth = new Vector3(-width, 0, 0);
    } 

    public void NextPage()
    {
        if(currentPage < maxPage)
        {
            currentPage++;
            targetPos += pageWidth;
            MovePage();
        }
    }

    public void PreviousPage()
    {
        if(currentPage > 1)
        {
            currentPage--;
            targetPos -= pageWidth;
            MovePage();
        }
    }

    private void MovePage()
    {
        mapPages.LeanMoveLocal(targetPos, tweenTime).setEase(tweenType);
        UpdateArrowButtons();
        mapText.text = "Mapa " + currentPage.ToString();
        TotalStarsCount();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if(Mathf.Abs(eventData.position.x-eventData.pressPosition.x) > dragThreshold)
        {
            if (eventData.position.x > eventData.pressPosition.x)
            {
                PreviousPage();
            }
            else
            {
                NextPage();
            }
        }
        else
        {
            MovePage();
        }
    }

    private void UpdateArrowButtons()
    {
        previousButton.interactable = true;
        nextButton.interactable = true;

        if (currentPage == 1)
        {
            previousButton.interactable = false;
        }
        else if (currentPage == maxPage)
        {
            nextButton.interactable = false;
        }
    }

    private void TotalStarsCount()
    {
        int previousTotalLevels = 0; //todos os niveis anteriores ao ultimo mapa
        for (int i = currentPage-1; i > 0; i--)
        {
            previousTotalLevels += howManyLevelsInEachMap[i];
        }

        int currentStars = 0;
        for (int i = 1; i <= howManyLevelsInEachMap[currentPage]; i++) //vai ser menor do que a quantidade de niveis no mapa
        {
            currentStars += PlayerPrefs.GetInt("Level_" + (i + previousTotalLevels).ToString("000"));
        }
        starsCountText.text = currentStars + "/" + howManyLevelsInEachMap[currentPage] * 3;
    }




    public void BackToMainMenu()
    {
        SceneManager.LoadScene("1 - Main Menu");
    }
}

//se quiser deixar um botao ativo ou inativo, coloque o nome do botao .interactable = true; (ou false) --> pode tentar com .enabled = true;