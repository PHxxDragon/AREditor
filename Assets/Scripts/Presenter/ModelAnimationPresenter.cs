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

        private ModelEntity firstModelEntity;
        void Awake()
        {
            animationBar.DropdownValueChangeEvent += DropdownValueChangeEventSubscriber;
            animationBar.PlayToggleClickEvent += PlayToggleClickEventSubscriber;
            animationBar.SliderValueChangeEvent += SliderValueChangeEventSubscriber;
            BaseEntity.OnEntityCreated += (entity) =>
            {
                if (!firstModelEntity && entity is ModelEntity modelEntity)
                {
                    firstModelEntity = modelEntity;
                    ApplyMode(GlobalStates.GetMode());
                }
            };

            GlobalStates.OnModeChange += (mode) =>
            {
                ApplyMode(mode);
            };
        }

        private void ApplyMode(GlobalStates.Mode mode)
        {
            switch(mode)
            {
                case GlobalStates.Mode.ViewModel:
                case GlobalStates.Mode.EditModel:
                    DetachListeners();
                    if (firstModelEntity)
                        ModelSelect(firstModelEntity.GetComponent<Selectable>());
                    break;
                case GlobalStates.Mode.EditARModule:
                case GlobalStates.Mode.ViewARModule:
                    if (firstModelEntity)
                        ModelDeselect(firstModelEntity.GetComponent<Selectable>());
                    AttachListeners();
                    break;
            }
        }

        private void AttachListeners()
        {
            selectionManager.OnObjectSelected += ModelSelect;
            selectionManager.OnObjectDeselected += ModelDeselect;
        }

        private void DetachListeners()
        {
            selectionManager.OnObjectSelected -= ModelSelect;
            selectionManager.OnObjectDeselected -= ModelDeselect;
        }

        private void ModelDeselect(Selectable selectable)
        {
            ModelEntity modelEntity = selectable.GetComponent<ModelEntity>();
            if (modelEntity == model)
            {
                model = null;       
                if (animationPlayer)
                {
                    animationPlayer.AnimationProgressChangeEvent -= AnimationProgressChangeEventSubscriber;
                    animationPlayer.AnimationStartEvent -= AnimationStartEventSubscriber;
                    if (animationBar)
                        animationBar.gameObject.SetActive(false);
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

