using UnityEngine;
using EAR.AssetManager;

namespace EAR.Entity
{
    public class SoundEntity : InvisibleEntity
{
        private string assetId;
        private bool playAtStart;

        public override bool IsValidEntity()
        {
            return !string.IsNullOrEmpty(assetId);
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
            Debug.Log(audioSource);
            Debug.Log(audioSource.clip);
            Debug.Log(audioSource.clip.name);
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
            if (!string.IsNullOrEmpty(assetId))
            {
                AudioClip audioClip = AssetContainer.Instance.GetSound(assetId);
                audioSource.clip = audioClip;
            } else
            {
                audioSource.clip = null;
            }
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

        public static SoundEntity InstantNewEntity(SoundData soundData)
        {
            SoundEntity soundPrefab = AssetContainer.Instance.GetSoundPrefab();
            SoundEntity soundEntity = Instantiate(soundPrefab);
            if (!string.IsNullOrEmpty(soundData.id))
            {
                soundEntity.SetId(soundData.id);
            }

            if (!string.IsNullOrEmpty(soundData.name))
            {
                soundEntity.SetEntityName(soundData.name);
            }

            soundEntity.playAtStart = soundData.playAtStart;

            AudioSource audioSource = soundEntity.GetComponentInChildren<AudioSource>();
            audioSource.loop = soundData.loop;

            if (!string.IsNullOrEmpty(soundData.assetId))
            {
                AudioClip audioClip = AssetContainer.Instance.GetSound(soundData.assetId);
                audioSource.clip = audioClip;
                soundEntity.assetId = soundData.assetId;
            }

            TransformData.TransformDataToTransfrom(soundData.transform, soundEntity.transform);
            OnEntityCreated?.Invoke(soundEntity);
            return soundEntity;
        }
    }
}

