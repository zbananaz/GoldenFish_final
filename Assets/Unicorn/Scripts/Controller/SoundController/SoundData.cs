using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SoundData : MonoBehaviour
{
    public AudioClip AudioClickBtn;

    public AudioClip AudioFootStep;
    public AudioClip AudioRevive;
    public AudioClip AudioReward;
    public AudioClip AudioSpinWheel;
    public AudioClip AudioStartCrewmate;
    public AudioClip AudioStartImpostor;
    public AudioClip AudioWin;
    public AudioClip AudioLose;
    public AudioClip AudioPuddleFall;
    public AudioClip AudioClockTick;
    public AudioClip AudioRewardClick;
    public AudioClip AudioClockTickFast;
    public AudioClip AudioClockTickFaster;
    public AudioClip AudioTimeOver;
    public AudioClip AudioMoreTime;

    public AudioClip[] AudiosLobby;
    public AudioClip[] AudioBgs;
    public AudioClip AudioOverTime;

    public AudioClip AudioMineExplode;
    public AudioClip PushFall;
    public AudioClip Push;
    public AudioClip SawHit;
    public AudioClip KnifePickedUp;
    public AudioClip KnifeAttack;

    public AudioClip SpikeDown;
    public AudioClip SpikeUp;

    public AudioClip GlassBreak;
    public AudioClip GlassLanding;
    public AudioClip Jump;
 
    public AudioClip AudioTugging;
    public AudioClip AudioTugWin;
    public AudioClip AudioTapTug;
    public AudioClip AudioSlap;
    public AudioClip AudioPowerClick;
    public AudioClip AudioMarbleThrow;
    public AudioClip AudioMarbleLanding;

    public AudioClip[] AudioPlayerFalls;
    public AudioClip[] AudioPlayerDie;

    [Title("Collectibles")]
    public List<AudioClip> ListAudioCollects;

    [Title("Special Objects")]
    public AudioClip[] BreakableObjectSounds;
    public AudioClip HitBedSound;



}
