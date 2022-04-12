using UnityEngine;
using EAR.Container;

namespace EAR.Entity
{
    public class SoundEntity : InvisibleEntity
{
        private static int count = 1;
        private string assetId = "";
        private bool playAtStart;

        protected override string GetDefaultName()
        {
            return "New sound " + count++;
        }

        public override void StartDefaultState()
        {
            base.StartDefaultState();
            if (playAtStart)
            {
                PlaySound();
            }
        }

        public override void ResetEntityState()
        {
            base.ResetEntityState();
            StopSound();
        }

        public void StopSound()
        {
            AudioSource audioSource = GetComponentInChildren<AudioSource>();
            audioSource.Stop();
        }

        public void PlaySound()
        {
            AudioSource audioSource = GetComponentInChildren<AudioSource>();
            audioSource.Play();
        }

        public void SetPlayAtStart(bool playAtStart)
        {
            this.playAtStart = playAtStart;
        }

        public void SetLoop(bool loop)
        {
            AudioSource audioSource = GetComponentInChildren<AudioSource>();
            audioSource.loop = loop;
        }

        public void SetAudioClip(string assetId)
        {
            if (this.assetId == assetId)
                return;

            AudioSource audioSource = GetComponentInChildren<AudioSource>();
            this.assetId = assetId;
            audioSource.clip = AssetContainer.Instance.GetSound(assetId);
        }

        public SoundData GetSoundData()
        {
            AudioSource audioSource = GetComponentInChildren<AudioSource>();
            SoundData soundData = new SoundData();
            soundData.assetId = assetId;
            soundData.id = GetId();
            soundData.loop = audioSource.loop;
            soundData.transform = TransformData.TransformToTransformData(transform);
            soundData.playAtStart = playAtStart;
            soundData.name = GetEntityName();
            return soundData;
        }

        public void PopulateData(SoundData soundData)
        {
            if (!string.IsNullOrEmpty(soundData.id))
            {
                SetId(soundData.id);
            }

            if (!string.IsNullOrEmpty(soundData.name))
            {
                SetEntityName(soundData.name);
            }

            if (soundData.playAtStart.HasValue)
            {
                playAtStart = soundData.playAtStart.Value;
            }

            AudioSource audioSource = GetComponentInChildren<AudioSource>();

            if (soundData.loop.HasValue)
            {
                audioSource.loop = soundData.loop.Value;
            }

            if (soundData.assetId != null)
            {
                AudioClip audioClip = AssetContainer.Instance.GetSound(soundData.assetId);
                audioSource.clip = audioClip;
                assetId = soundData.assetId;
            }

            if (soundData.transform != null)
            {
                TransformData.TransformDataToTransfrom(soundData.transform, transform);
            }
        }

        public static SoundEntity InstantNewEntity(SoundData soundData)
        {
            SoundEntity soundPrefab = AssetContainer.Instance.GetSoundPrefab();
            SoundEntity soundEntity = Instantiate(soundPrefab);
            soundEntity.PopulateData(soundData);
            OnEntityCreated?.Invoke(soundEntity);
            return soundEntity;
        }
    }
}

