using System.Collections.Generic;
using UnityEngine;

public class SoundManager
{
    //오디오 소스를 BGM용, 이펙트용도로 하나씩 두개를 만듬
    AudioSource[] _audioSources = new AudioSource[(int)Define.Sound.MaxCount];
    Dictionary<string, AudioClip> _audioClip = new Dictionary<string, AudioClip>();

    //사운드를 재생시켜줄 오브젝트를 생성시켜줌
    public void Init()
    {
        GameObject root = GameObject.Find("@Sound");

        if (root == null)
        {
            root = new GameObject { name = "@Sound" };
            Object.DontDestroyOnLoad(root);

            string[] soundNames = System.Enum.GetNames(typeof(Define.Sound));

            for (int i = 0; i < soundNames.Length - 1; i++)
            {
                GameObject go = new GameObject { name = soundNames[i] };
                _audioSources[i] = go.AddComponent<AudioSource>();
                go.transform.parent = root.transform;
            }
            _audioSources[(int)Define.Sound.Bgm].loop = true;
        }
    }

    public void Clear()
    {
        foreach (AudioSource audioSource in _audioSources)
        {
            audioSource.clip = null;
            audioSource.Stop();
        }

        _audioClip.Clear();
    }

    //타입, 경로, 재생속도를 일단 랩핑해줌
    public void Play(string path, Define.Sound type = Define.Sound.Effect, float pitch = 1.0f)
    {
        AudioClip audioClip = GetOrAddAudioClip(path);
        if (audioClip == null)
        {
            Debug.Log($"AudioClip Missing{path}");
            return;
        }
    }

    //public void Play(string path, Define.Sound type = Define.Sound.Effect, float pitch = 1.0f)
    //{
    //    //함수를 여러가지 버전으로 분리 할 때는 반복된 코드는 하나의 버전에서 다른 버전을 호출 하도록
    //    if (path.Contains("Sounds/") == false)
    //    {
    //        path = $"Sounds/{path}";
    //    }
    //    if (type == Define.Sound.Bgm)
    //    {
    //        AudioClip audioClip = Managers.Resources.Load<AudioClip>(path);
    //        if (audioClip == null)
    //        {
    //            Debug.Log($"AudioClip Missing{path}");
    //            return;
    //        }

    //        //AudioSource audioSource = _audioSources[(int)Define.Sound.Bgm];
    //        ////이미 실행중인 Bgm이 있다면 정지
    //        //if (audioSource.isPlaying) { audioSource.Stop(); }

    //        //audioSource.pitch = pitch;
    //        //audioSource.clip = audioClip;
    //        //audioSource.Play();
    //    }
    //    else
    //    {
    //        AudioClip audioClip = GetOrAddAudioClip(path);
    //        if (audioClip == null)
    //        {
    //            Debug.Log($"AudioClip Missing{path}");
    //            return;
    //        }
    //        //AudioSource audioSource = _audioSources[(int)Define.Sound.Effect];
    //        //audioSource.pitch = pitch;
    //        //audioSource.PlayOneShot(audioClip);
    //    }
    //}

    public void Play(AudioClip audioClip, Define.Sound type = Define.Sound.Effect, float pitch = 1.0f)
    {
        if (audioClip == null)
        {
            return;
        }
        if (type == Define.Sound.Bgm)
        {
            AudioSource audioSource = _audioSources[(int)Define.Sound.Bgm];
            if (audioSource.isPlaying) { audioSource.Stop(); }
            audioSource.pitch = pitch;
            audioSource.clip = audioClip;
            audioSource.Play();
        }
        else
        {
            AudioSource audioSource = _audioSources[(int)Define.Sound.Effect];
            audioSource.pitch = pitch;
            audioSource.PlayOneShot(audioClip);
        }
    }


    AudioClip GetOrAddAudioClip(string path, Define.Sound type = Define.Sound.Effect)
    {
        //함수를 여러가지 버전으로 분리 할 때는 반복된 코드는 하나의 버전에서 다른 버전을 호출 하도록
        if (path.Contains("Sounds/") == false)
        {
            path = $"Sounds/{path}";
        }
        AudioClip audioClip = null;

        if (type == Define.Sound.Bgm)
        {
            audioClip = Managers.Resources.Load<AudioClip>(path);
        }
        else
        {
            if (_audioClip.TryGetValue(path, out audioClip) == false)
            {
                audioClip = Managers.Resources.Load<AudioClip>(path);
                _audioClip.Add(path, audioClip);
            }
        }
        if (audioClip == null)
        {
            Debug.Log($"AudioClip Missing {path}");
        }
        //없었으면 가져와서 있는 것은 리턴 있었으면 있던것을 리턴
        return audioClip;
    }
}
