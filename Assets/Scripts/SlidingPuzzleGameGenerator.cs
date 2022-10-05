using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
public class SlidingPuzzleGameGenerator : MonoBehaviour
{

    private ArraySlidingPuzzle puzzle = new ArraySlidingPuzzle();

    private List<SpriteIndex> spritesIndex = new List<SpriteIndex>();

    [SerializeField]
    private TextMeshProUGUI timer;

    [SerializeField]
    private Camera camera;

    private TimerUtils chrono;

    [SerializeField]
    private Sprite[] androidArray;

    [SerializeField]
    private Sprite[] appleArray;

    [SerializeField]
    private List<GameObject> gameObjects;

    private Transform posEmptyCase;

    private float time = 0;

    private int[] indexInGame = { 0, 1, 2, 3, 4, 5, 6, 7, 8 };

    //verifie l'appareille jouer
    private bool IsAndroid()
    {
        //RuntimePlatform.Android
        if (Application.platform == RuntimePlatform.WindowsEditor)
        {
            return true;
        }

        return false;
    }


    private List<SpriteIndex> InitListSpritesIndex(Sprite[] spriteArray)
    {

        List<SpriteIndex> spritesList = new List<SpriteIndex>();

        int length = spriteArray.Length;

        //on melange un index 
        indexInGame = puzzle.ShuffleSlidingPuzzle(indexInGame);

        //on transforme l'ordre en fonction de l'index
        for (int i = 0; i <= length - 1; i++)
        {
            SpriteIndex spriteIndexTemp = new SpriteIndex
            {
                index = indexInGame[i],
                sprite = spriteArray[indexInGame[i]]
            };


            spritesList.Add(spriteIndexTemp);

        }

        return spritesList;
    }

    //permet de transferer les données d'un liste de spriteIndex a une liste de gameobject
    private List<GameObject> InitListGameObject(List<SpriteIndex> spritesIndexList, List<GameObject> listeGameObjects)
    {
        List<GameObject> listeGameTest = listeGameObjects;

        int count = 0;
        foreach (var spriteIndex in spritesIndexList)
        {

            //transferer sprites index 
            int indexTemp = spriteIndex.index;

            Sprite spriteTemp = spriteIndex.sprite;

            listeGameTest[count].GetComponent<SpriteRenderer>().sprite = spriteTemp;
            listeGameTest[count].tag = indexTemp + "";

            if (indexTemp == 0)
            {
                posEmptyCase = listeGameTest[count].transform;
            }


            count++;
        }

        return listeGameTest;
    }

    private void InitPuzzle()
    {
        Sprite[] spritesIndex = { };

        if (IsAndroid())
        {
            //on stoque les sprites android
            spritesIndex = androidArray;
        }
        else
        {
            //on stoque les sprites apple
            spritesIndex = appleArray;
        }

        //Creation d'une liste de spriteIndex aleatoire et soluble
        //on modifie la liste de gameobjects renseignier pour que les information concorde
        gameObjects = InitListGameObject(InitListSpritesIndex(spritesIndex), gameObjects);

    }

    private void Sliding()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //change la position de deux elements
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);
            if (hit)
            {

                //distance A et B
                if (Vector2.Distance(posEmptyCase.transform.position, hit.transform.position) < 4)
                {
                    Vector2 lastPos = hit.transform.position;
                    hit.transform.position = posEmptyCase.transform.position;
                    posEmptyCase.transform.position = lastPos;

                    int tagInt = int.Parse(hit.transform.tag);
                    int newTag = 0;
                    int newZero = 0;

                    for (int i = 0; i < indexInGame.Length; i++)
                    {
                        if (indexInGame[i] == 0)
                        {
                            newZero = i;
                        }
                        if (indexInGame[i] == tagInt)
                        {
                            newTag = i;
                        }
                    }
                    int temp = indexInGame[newTag];
                    indexInGame[newTag] = indexInGame[newZero];
                    indexInGame[newZero] = temp;

                    string text = "";
                    foreach (var value in indexInGame)
                    {
                        text += "" + value;
                    }

                    if (text.CompareTo("123405678") == 0)
                    {
                        SceneManager.LoadScene("Home");
                    }

                }
            }

        }
    }

    //met fin au jeu a la fin du timer + gere le temps
    private void EndTime(float maxTime, float time, TextMeshProUGUI timer)
    {
        float minutes = Mathf.FloorToInt(time / 60);
        float secondes = Mathf.FloorToInt(time % 60);
        timer.text = minutes + "" + secondes;

        float max = Mathf.FloorToInt(maxTime * 60);

        //verifie le timer
        if (time > max)
        {
            SceneManager.LoadScene("Home");

        }

    }

    // Start is called before the first frame update
    void Start()
    {
        InitPuzzle();

    }

    // Update is called once per frame
    void Update()
    {
        Sliding();

        //timer
        time += Time.deltaTime;
        EndTime(3, time, timer);


    }
}
