using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace EAR.View 
{
    public class ShowTutorial : MonoBehaviour
    {
        [SerializeField]
        private Button openButton;
        [SerializeField]
        private Button closeButton;
        [SerializeField]
        private GameObject tutorialTable;

        void Start()
        {
            openButton.onClick.AddListener(OpenTutorialTable);
            closeButton.onClick.AddListener(CloseTutorialTable);
            tutorialTable.transform.localScale = Vector3.zero;
        }

        private void OpenTutorialTable()
        {
            tutorialTable.SetActive(true);
            tutorialTable.transform.DOScale(Vector3.one, 0.2f);
            
        }

        private void CloseTutorialTable()
        {
            tutorialTable.transform.DOScale(Vector3.zero, 0.2f).OnComplete(() => {
                tutorialTable.SetActive(false);
            });
        }
    }
}

