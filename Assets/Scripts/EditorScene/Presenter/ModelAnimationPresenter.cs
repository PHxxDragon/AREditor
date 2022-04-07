using EAR.View;
using EAR.AR;
using UnityEngine;
using EAR.AnimationPlayer;
using EAR.Selection;
using EAR.Entity;

namespace EAR.Editor.Presenter
{
    public class ModelAnimationPresenter : MonoBehaviour
    {
        [SerializeField]
        private ModelLoader modelLoader;
        [SerializeField]
        private AnimationBar animationBar;
        [SerializeField]
        private SelectionManager selectionManager;

        private ModelEntity model;
        private AnimPlayer animationPlayer;

        void Awake()
        {
            if (!GlobalStates.IsEnableEditor())
            {
                AttachListeners();
            }

            GlobalStates.OnEnableEditorChange += (value) =>
            {
                if (value)
                {
                    AttachListeners();
                } else
                {
                    DetachListeners();
                }
            };
            
        }

        private void AttachListeners()
        {
            animationBar.DropdownValueChangeEvent += DropdownValueChangeEventSubscriber;
            animationBar.PlayToggleClickEvent += PlayToggleClickEventSubscriber;
            animationBar.SliderValueChangeEvent += SliderValueChangeEventSubscriber;
            selectionManager.OnObjectSelected += ModelSelect;
            selectionManager.OnObjectDeselected += ModelDeselect;
        }

        private void DetachListeners()
        {
            animationBar.DropdownValueChangeEvent -= DropdownValueChangeEventSubscriber;
            animationBar.PlayToggleClickEvent -= PlayToggleClickEventSubscriber;
            animationBar.SliderValueChangeEvent -= SliderValueChangeEventSubscriber;
            selectionManager.OnObjectSelected -= ModelSelect;
            selectionManager.OnObjectDeselected -= ModelDeselect;
        }

        private void ModelDeselect(Selectable selectable)
        {
            ModelEntity modelEntity = selectable.GetComponent<ModelEntity>();
            if (modelEntity == model)
            {
                model = null;
                if (animationBar)
                    animationBar.gameObject.SetActive(false);
                if (animationPlayer)
                {
                    animationPlayer.AnimationProgressChangeEvent -= AnimationProgressChangeEventSubscriber;
                    animationPlayer.AnimationStartEvent -= AnimationStartEventSubscriber;
                }
            }
        }

        private void ModelSelect(Selectable selectable)
        {
            ModelEntity modelEntity = selectable.GetComponent<ModelEntity>();
            if (modelEntity)
            {
                model = modelEntity;
                animationPlayer = modelEntity.GetComponentInChildren<AnimPlayer>();
                if (animationPlayer)
                {
                    animationPlayer.AnimationProgressChangeEvent += AnimationProgressChangeEventSubscriber;
                    animationPlayer.AnimationStartEvent += AnimationStartEventSubscriber;
                    animationBar.gameObject.SetActive(true);
                    animationBar.SetDropdownOption(animationPlayer.GetAnimationList(), animationPlayer.GetCurrentIndex());
                }
            }
                
        }

        private void SliderValueChangeEventSubscriber(float obj)
        {
            if (animationPlayer)
                animationPlayer.SetAnimationState(obj);
        }

        private void AnimationStartEventSubscriber(bool obj)
        {
            animationBar.SetPlayToggle(obj);
        }

        private void PlayToggleClickEventSubscriber(bool obj)
        {
            if (animationPlayer)
                animationPlayer.ToggleAnimationPlay(obj);
        }

        private void AnimationProgressChangeEventSubscriber(float obj)
        {
            animationBar.SetSliderValue(obj - Mathf.FloorToInt(obj));
        }

        private void DropdownValueChangeEventSubscriber(int obj)
        {
            if (animationPlayer)
                animationPlayer.PlayAnimation(obj);
        }
    }
}

