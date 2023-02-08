using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : Singleton<SoundManager>
{
    public ClipData[] clips_UI;

    public ClipData[] clips_Player;
    public ClipData[] clips_Bulletkin;
    public ClipData[] clips_ShotgunKin;
    public ClipData[] clips_Boss;
    public ClipData[] clips_Weapon;
    public ClipData[] clips_Item;

    private AudioSource source;
    private AudioSource itemSource;

    protected override void Awake()
    {
        base.Awake();
        source = GetComponent<AudioSource>();
        itemSource = transform.GetChild(0).GetComponent<AudioSource>();
    }

    // DIZZY CODES
    public void PlaySound_UI(Clips_UI clipNum, AudioSource _source = null)
    {
        if(_source == null)
            _source = source;

        _source.PlayOneShot(clips_UI[(int)clipNum].clip,
            clips_UI[(int)clipNum].clipVolume * GameManager.Inst.Volume_Master * GameManager.Inst.Volume_UI);
    }

    public void PlaySound_Player(Clips_Player clipNum)
    {
        source.PlayOneShot(clips_Player[(int)clipNum].clip,
            clips_Player[(int)clipNum].clipVolume * GameManager.Inst.Volume_Master * GameManager.Inst.Volume_VFX);
    }

    public void PlaySound_BulletKin(Clips_BulletKin clipNum, AudioSource _source = null)
    {
        if (_source == null)
            _source = source;   

        _source.PlayOneShot(clips_Bulletkin[(int)clipNum].clip,
            clips_Bulletkin[(int)clipNum].clipVolume * GameManager.Inst.Volume_Master * GameManager.Inst.Volume_VFX);
    }

    public void PlaySound_ShotgunKin(Clips_ShotgunKin clipNum, AudioSource _source = null)
    {
        if (_source == null)
            _source = source;

        _source.PlayOneShot(clips_ShotgunKin[(int)clipNum].clip,
            clips_ShotgunKin[(int)clipNum].clipVolume * GameManager.Inst.Volume_Master * GameManager.Inst.Volume_VFX);
    }

    public void PlaySound_Boss(Clips_Boss clipNum, AudioSource _source = null)
    {
        if (_source == null)
            _source = source;

        _source.PlayOneShot(clips_Boss[(int)clipNum].clip,
            clips_Boss[(int)clipNum].clipVolume * GameManager.Inst.Volume_Master * GameManager.Inst.Volume_VFX);
    }

    public void PlaySound_Weapon(Clips_Weapon clipNum, AudioSource _source = null)
    {
        if (_source == null)
            _source = source;

        _source.PlayOneShot(clips_Weapon[(int)clipNum].clip,
            clips_Weapon[(int)clipNum].clipVolume * GameManager.Inst.Volume_Master * GameManager.Inst.Volume_VFX);
    }

    public void PlaySound_Item(Clips_Item clipNum)
    {
        itemSource.PlayOneShot(clips_Item[(int)clipNum].clip,
            clips_Item[(int)clipNum].clipVolume * GameManager.Inst.Volume_Master * GameManager.Inst.Volume_VFX);
    }
}
