using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; //DONT FORGET THIS!!!!!!!

public class Health : MonoBehaviour {

  //  static public int health;
  //  public int numOfHearts; //heart containers

   // public Image[] hearts;
  //  public Sprite fullHeart;

    private GameObject[] heartsobj; //usually public
  //  private Animator[] anim;

    public GameObject heart5;
    public GameObject heart4;
    public GameObject heart3;
    public GameObject heart2;
    public GameObject heart1;

   // private Animator animator3;
 //   private Animator animator2;
   // private Animator animator1;

    void Start () {
        Tester.health = 5;
        //starting health
      //  health = 5;
        //get Animator components of each heart obj
        //  animator3 = heart3.GetComponent<Animator>();
        //  animator2 = heart2.GetComponent<Animator>();
        //  animator1 = heart1.GetComponent<Animator>();
        //put heart obj into an array
        //  heartsobj[0] = heart1;
        //  heartsobj[1] = heart2;
        //  heartsobj[2] = heart3;
        //put each animator component into an array
        //   anim[0] = animator1;
        //  anim[1] = animator2;
        //  anim[2] = animator3;

      //  health = PlayerPrefs.GetInt("health", 5);
    }
    void OnDestroy()
    {
       // Debug.Log("health has been reset");
       // PlayerPrefs.SetInt("health", health);
        
    }
	
	void Update () {

     //   DontDestroyOnLoad(heart5);
     //   DontDestroyOnLoad(heart4);
      //  DontDestroyOnLoad(heart3);
      //  DontDestroyOnLoad(heart2);
     //   DontDestroyOnLoad(heart1);

        if (Tester.health == 4)
        {
            Destroy(heart5);
            // animator3.SetBool("Play", true);
        }
        if (Tester.health == 3)
        {
            Destroy(heart4);
            // animator3.SetBool("Play", true);
        }
        if (Tester.health == 2)
        {
            Destroy(heart3);
           // animator3.SetBool("Play", true);
        } else if (Tester.health == 1)
        {
            Destroy(heart2);
            //animator2.SetBool("Play", true);
        } else if(Tester.health == 0)
        {
            Destroy(heart1);
            //animator1.SetBool("Play", true);
        }

        //if by accident we enter more hearts than containers available
     //   if(health > numOfHearts){
           // health = numOfHearts;
      //  }

     //   for (int i = 0; i < heartsobj.Length; i++){

            //for each new heart of the array we want to create a new heart sprite
           // hearts[i].sprite = fullHeart;

          //  if(i == health)
        //    {
              //  animator = heartsobj[i].GetComponent<Animator>();
            //    anim[i].SetBool("Play", true);
          //  }


          //  if (i < health){ //make the heart white if we havent reached full health
              //  hearts[i].color = Color.red;

          //  } else { //make the heart black if we have reached full health
                     // hearts[i].color = Color.black; //or Destroy(hearts[i]);
             //   animator = heartsobj[i].GetComponent<Animator>();
              //  animator.SetBool("Play", true);
         //   }

            //if(health == 0) destroy player, GameOver.gameover = true;

            //create a new heart if i is less than the number of hearts we want
         //   if (i < numOfHearts){
        //        hearts[i].enabled = true;
        //    } else {
         //       hearts[i].enabled = false;
         //       Destroy(heartsobj[i]);
         //   }
        }
	}

