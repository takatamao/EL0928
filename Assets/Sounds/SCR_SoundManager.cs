/*
 *  Contents   : サウンドマネージャ
 *  Author     : Abe Kaoru
 */

using UnityEngine;

namespace Sound
{
    /// <summary>
    /// BGM enmu
    /// </summary>
    public enum BGM_Type
    {
        TITLE = 0,
        GAME,
    }
    /// <summary>
    /// 効果音 enum
    /// </summary>
    public enum SE_EatType
    {
        KUUU,
        KITAKITA,
        UNME,
        KIMOCHII,
    }
    public enum SE_Type
    {
        GameStart,
        TimeUp,
        KUWANAITO,
        MIKAKU,
    }

    public class SCR_SoundManager : MonoBehaviour
    {
        ///<summary> シングルトン </summary>
        public static SCR_SoundManager instance;

        ///<summary> BGM </summary>
        private AudioSource bgmSource;

        ///<summary> 同時に流せるSE 今回は10</summary>
        private AudioSource[] seSources = new AudioSource[10];

        ///<summary> BGM </summary>
        [SerializeField] [Header("BGM")] private AudioClip[] bgmClips;

        ///<summary> SE </summary>
        [SerializeField] [Header("SE")] private AudioClip[] seClips;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;

                //Scene移動で消されないようにする
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                //既に生成されているのであれば消す
                Destroy(this);
            }

            // AudioSource追加
            bgmSource = gameObject.AddComponent<AudioSource>();

            for (int i = 0; i < seSources.Length; i++)
            {
                seSources[i] = gameObject.AddComponent<AudioSource>();
            }
        }

        /// <summary>
        /// BGMプレイ関数
        /// </summary>
        /// <param BGMの種類 ="bgmType"></param>
        /// <param ループするか ="loopFlg"></param>
        public void PlayBGM(BGM_Type bgmType, bool loopFlg = true)
        {
            int index = (int)bgmType;
            if (index < 0 || bgmClips.Length <= index)//登録範囲外の場合は処理しない
            {
                return;
            }

            bgmSource.loop = loopFlg;
            bgmSource.clip = bgmClips[index];
            bgmSource.Play();
        }

        /// <summary>
        /// SEプレイ関数
        /// </summary>
        /// <param SEの種類 ="seType"></param>
        /// <param ボリューム ="vol"></param>
        public void PlaySE(SE_EatType seType, float vol = 1.0f)
        {
            int index = (int)seType;
            if (index < 0 || seClips.Length <= index)//登録範囲外の場合は処理しない
            { 
                return; 
            }

            // 再生中ではないAudioSourceをつかってSEを鳴らす
            foreach (AudioSource s in seSources)
            {
                if (s.isPlaying) { continue; }

                s.clip = seClips[index];
                s.loop = false;
                s.volume = vol;
                s.Play();
                break;
            }

        }

        /// <summary>
        /// BGM停止関数
        /// </summary>
        public void StopBGM()
        {
            bgmSource.Stop();
            bgmSource.clip = null;
        }


        /// <summary>
        /// BGMボリューム変更関数
        /// </summary>
        /// <param volume ="vol"></param>
        public void SetVolumeBGM(float vol)
        {
            vol = Mathf.Clamp(vol, 0.0f, 1.0f);
            bgmSource.volume = vol;
        }
    }


}

