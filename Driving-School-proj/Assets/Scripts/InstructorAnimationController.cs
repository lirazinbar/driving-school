using System;
using System.Collections;
using Audio;
using Enums;
using Managers;
using UnityEngine;
using Random = UnityEngine.Random;

public class InstructorAnimationController : MonoBehaviour
{
    private Animator _animator;
    private bool _isTalking;
    private static readonly int IsTalking = Animator.StringToHash("isTalking");

    private const string AudioInstructorPath = "Audio/Instructor";
    private AudioClip[] _audioClipsMisc;
    private AudioClip[] _audioClipsStartDrive;

    void Start()
    {
        LoadAudioClips();
        _animator = GetComponent<Animator>();
        
        EventsManager.Instance.carEnteredCrossSectionEvent.AddListener(OnCarEnteredCrossSectionEvent);
        
        StartCoroutine(PlaySoundAfterDelay(3f, _audioClipsStartDrive[Random.Range(0, _audioClipsStartDrive.Length)].name));
    }
    
    void OnDestroy()
    {
        EventsManager.Instance.carEnteredCrossSectionEvent.RemoveListener(OnCarEnteredCrossSectionEvent);
    }
    
    private void LoadAudioClips()
    {
        _audioClipsMisc = Resources.LoadAll<AudioClip>(AudioInstructorPath + "/Misc");
        _audioClipsStartDrive = Resources.LoadAll<AudioClip>(AudioInstructorPath + "/StartDrive");
        
        if (_audioClipsMisc.Length == 0 || _audioClipsStartDrive.Length == 0)
        {
            if (_audioClipsMisc.Length == 0)
            {
                throw new Exception("No audio clips found in " + AudioInstructorPath + "/Misc");
            }
            throw new Exception("No audio clips found in " + AudioInstructorPath + "/StartDrive");
        }
    }
    
    private void OnCarEnteredCrossSectionEvent(CrossSectionDirections direction)
    {
        float audioClipLength = Talk(direction.ToString());
        
        // Say some misc sentence after the direction
        StartCoroutine(PlaySoundAfterDelay(audioClipLength + 5f, _audioClipsMisc[Random.Range(0, _audioClipsMisc.Length)].name));
    }
    
    private float Talk(string audioClipName)
    {
        _isTalking = true;
        _animator.SetBool(IsTalking, _isTalking);
        AudioManager.Instance.Play(audioClipName);
        
        float audioClipLength = AudioManager.Instance.GetAudioClipLength(audioClipName);
        StartCoroutine(StopTalkingAnimationAfterDelay(audioClipLength));
        
        return audioClipLength;
    }
    
    IEnumerator StopTalkingAnimationAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        _isTalking = false;
        _animator.SetBool(IsTalking, _isTalking);
    }
    
    IEnumerator PlaySoundAfterDelay(float delay, string audioClipName)
    {
        yield return new WaitForSeconds(delay);
        if (!_isTalking)
        {
            Talk(audioClipName);
        }
    }
}
