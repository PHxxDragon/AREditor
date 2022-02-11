using EAR.View;
using EAR.AR;
using UnityEngine;
using EAR.AnimationPlayer;
using System;

namespace EAR.Editor.Presenter
{
    public class ModelAnimationPresenter : MonoBehaviour
    {
        [SerializeField]
        private ModelLoader modelLoader;
        [SerializeField]
        private AnimationBar animationBar;
        [SerializeField]
        private AnimPlayer animationPlayer;


        void Start()
        {
            if (modelLoader != null && animationBar != null && animationPlayer != null)
            {
                modelLoader.OnLoadEnded += ModelLoadDone;
            } else
            {
                Debug.Log("Unassigned references");
            }
        }

        private void ModelLoadDone()
        {
            if (animationPlayer.SetModel(modelLoader.GetModel()))
            {
                animationBar.gameObject.SetActive(true);
                animationBar.AddDropdownOption(animationPlayer.GetAnimationList());
                animationBar.DropdownValueChangeEvent += DropdownValueChangeEventSubscriber;
                animationBar.PlayToggleClickEvent += PlayToggleClickEventSubscriber;
                animationBar.SliderValueChangeEvent += SliderValueChangeEventSubscriber;
                animationPlayer.AnimationProgressChangeEvent += AnimationProgressChangeEventSubscriber;
                animationPlayer.AnimationStartEvent += AnimationStartEventSubscriber;
            }
        }

        private void SliderValueChangeEventSubscriber(float obj)
        {
            animationPlayer.SetAnimationState(obj);
        }

        private void AnimationStartEventSubscriber(bool obj)
        {
            animationBar.SetPlayToggle(obj);
        }

        private void PlayToggleClickEventSubscriber(bool obj)
        {
            animationPlayer.ToggleAnimationPlay(obj);
        }

        private void AnimationProgressChangeEventSubscriber(float obj)
        {
            animationBar.SetSliderValue(obj - Mathf.FloorToInt(obj));
        }

        private void DropdownValueChangeEventSubscriber(int obj)
        {
            animationPlayer.PlayAnimation(obj);
        }
    }
}

