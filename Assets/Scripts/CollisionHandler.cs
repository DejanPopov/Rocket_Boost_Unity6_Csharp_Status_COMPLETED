using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionHandler : MonoBehaviour
{
    [SerializeField] float          loadLevelDelay = 2f;
    [SerializeField] AudioClip      rocketCrashSFX;
    [SerializeField] AudioClip      rocketSuccessSFX;
    [SerializeField] ParticleSystem successParticles;
    [SerializeField] ParticleSystem explosionParticles;

    AudioSource myAudioSource;

    bool isControllable = true;

    void Start()
    {
        myAudioSource = GetComponent<AudioSource>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!isControllable) {return;}
        
        switch (collision.gameObject.tag)
            {
                case "Friendly":
                    break;

                case "Finish":
                    StartSuccessSequence();
                    break;

                default:
                    StartCrashSequence();
                    break;
            }
    }

    private void StartSuccessSequence()
    {
        isControllable = false;
        myAudioSource.Stop();
        myAudioSource.PlayOneShot(rocketSuccessSFX);
        successParticles.Play();
        GetComponent<Movement>().enabled = false;
        Invoke("LoadNextLevel", loadLevelDelay);
    }

    private void StartCrashSequence()
    {
        isControllable = false;
        myAudioSource.Stop();
        myAudioSource.PlayOneShot(rocketCrashSFX);
        explosionParticles.Play();
        GetComponent<Movement>().enabled = false;
        Invoke("ReloadLevel",loadLevelDelay);
    }

    private void LoadNextLevel()
    {
        int currentScene = SceneManager.GetActiveScene().buildIndex;
        int nextScene    = currentScene + 1;

        if (nextScene == SceneManager.sceneCountInBuildSettings)
        {
            nextScene = 0;
        }

        SceneManager.LoadScene(nextScene);
    }

    private void ReloadLevel()
    {
        int currentScene = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentScene);
    }
}
