using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using TMPro;
using UnityEngine.UI;

public class TaskManager : MonoBehaviour
{
    public XRSocketInteractor socketInteractor1;
    public XRSocketInteractor socketInteractor2;
    public XRSocketInteractor socketInteractor3;
    public XRSocketInteractor socketInteractor4;
    public GameObject[] imageBoxPrefabs;
    private GameObject currentImageBox1;
    private GameObject currentImageBox2;
    private GameObject currentImageBox3;
    private GameObject currentImageBox4;
    public GameObject[] audioListenerPrefabs;
    private GameObject currentAudioListener;
    private List<Vector3> positionList;
    private List<int> indexList; // for Prefabs
    private List<int> numberList; // for Boxes
    public TextMeshProUGUI scoreText;
    private bool scoreUpdated = false;
    private int score = 0;

    void Start()
    {
        positionList = new List<Vector3>();
        positionList.Add(new Vector3(-39.1f, 2.0f, -19.9f));
        positionList.Add(new Vector3(-40.0f, 2.0f, -7.1f));
        positionList.Add(new Vector3(-33.1f, 2.0f, 3.7f));
        positionList.Add(new Vector3(-38.0f, 2.0f, 20.6f));

        indexList = new List<int>();
        for (int i = 0; i < imageBoxPrefabs.Length / 4; i++)
        {
            indexList.Add(i);
        }

        numberList = new List<int>{0, 1, 2, 3};

        socketInteractor1 = GameObject.Find("SocketLock1").GetComponent<XRSocketInteractor>();
        socketInteractor2 = GameObject.Find("SocketLock2").GetComponent<XRSocketInteractor>();
        socketInteractor3 = GameObject.Find("SocketLock3").GetComponent<XRSocketInteractor>();
        socketInteractor4 = GameObject.Find("SocketLock4").GetComponent<XRSocketInteractor>();

        int index = PickAndRemoveRandomNumber();
        SpawnImageBoxes(index);
        SpawnAudioListener(index);
    }

    void Update()
    {
        if (socketInteractor1.hasSelection && socketInteractor2.hasSelection &&
            socketInteractor3.hasSelection && socketInteractor4.hasSelection)
        {
            if (!scoreUpdated) 
            {
                UpdateScore(socketInteractor1.selectTarget.gameObject.name, currentImageBox1.name,
                    socketInteractor2.selectTarget.gameObject.name, currentImageBox2.name,
                    socketInteractor3.selectTarget.gameObject.name, currentImageBox3.name,
                    socketInteractor4.selectTarget.gameObject.name, currentImageBox4.name);
                scoreUpdated = true;
            }
            
            if (indexList.Count > 0)
            {
                int index = PickAndRemoveRandomNumber();
                SpawnImageBoxes(index);
                SpawnAudioListener(index);
                scoreUpdated = false;
            }
        }
    }

    private int PickAndRemoveRandomNumber()
    {
        int randomIndex = Random.Range(0, indexList.Count);
        int pickedNumber = indexList[randomIndex];
        indexList.RemoveAt(randomIndex);
        return pickedNumber;
    }

    private void SpawnImageBoxes(int index)
    {
        Shuffle(numberList);

        if (currentImageBox1 && currentImageBox2 && currentImageBox3 && currentImageBox4)
        {
            Destroy(currentImageBox1);
            Destroy(currentImageBox2);
            Destroy(currentImageBox3);
            Destroy(currentImageBox4);
        }
        
        currentImageBox1 = Instantiate(imageBoxPrefabs[index * 4 + 0], positionList[numberList[0]], imageBoxPrefabs[index * 4 + 0].transform.rotation);
        currentImageBox2 = Instantiate(imageBoxPrefabs[index * 4 + 1], positionList[numberList[1]], imageBoxPrefabs[index * 4 + 1].transform.rotation);
        currentImageBox3 = Instantiate(imageBoxPrefabs[index * 4 + 2], positionList[numberList[2]], imageBoxPrefabs[index * 4 + 2].transform.rotation);
        currentImageBox4 = Instantiate(imageBoxPrefabs[index * 4 + 3], positionList[numberList[3]], imageBoxPrefabs[index * 4 + 3].transform.rotation);
    }

    private void SpawnAudioListener(int index)
    {
        if (currentAudioListener)
        {
            Destroy(currentAudioListener);
        }

        currentAudioListener = Instantiate(audioListenerPrefabs[index], new Vector3(0, 0, 0), audioListenerPrefabs[index].transform.rotation);
        currentAudioListener.GetComponent<AudioSource>().Play();
    }

    private void UpdateScore(string response1, string answer1, string response2, string answer2,
        string response3, string answer3, string response4, string answer4)
    {
        if (response1 == answer1) score += 10;
        if (response2 == answer2) score += 10;
        if (response3 == answer3) score += 10;
        if (response4 == answer4) score += 10;
        scoreText.text = "Score : " + score.ToString();
    }

    private void Shuffle(List<int> list)
    {
        int n = list.Count;
        for (int i = 0; i < n; i++)
        {
            int k = Random.Range(0, n);
            int temp = list[k];
            list[k] = list[i];
            list[i] = temp;
        }
    }
}
