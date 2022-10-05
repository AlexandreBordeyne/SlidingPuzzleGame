using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SceneTransitionSystem;
//class pour creer et gerer le tableau du jeu
public class ArraySlidingPuzzle : MonoBehaviour
{

    private System.Random randomS = new System.Random();

    //melange le jeux de facon soluble
    public int[] ShuffleSlidingPuzzle(int[] index)
    {
        //melanger le tableau
        int[] randomArray = ShuffleIndex(index);

        //si soluble return tableau
        if (IsSoluble(randomArray))
        {
            return randomArray;
        }
        //sinon rendre le tableau soluble
        else
        {
            SwapIndexWhileNotSoluble(index);
        }

        return index;
    }


    //on creer une copy melanger random de notre index
    public int[] ShuffleIndex(int[] index)
    {
        int[] indexRandom = index;

        for (int i = index.Length - 1; i > 0; i--)
        {
            int random = randomS.Next(0, i + 1);

            int temp = indexRandom[i];
            indexRandom[i] = index[random];
            indexRandom[random] = temp;
        }
        return indexRandom;

    }


    //compte le nombre d'inversions dans un tableau melangé 
    public int CountInversion(int[] indexRandom)
    {
        int emptyTile = 0;
        int numberInverions = 0;

        // une inversion se produit lorsqu'une tuile précède une autre tuile avec un nombre inférieur
        for (int i = 0; i < 9; i++)
        {
            for (int j = i + 1; j < 9; j++)
            {

                // Value 0 is used for empty space
                if (indexRandom[i] > emptyTile && indexRandom[j] > emptyTile && indexRandom[i] > indexRandom[j])
                {
                    numberInverions++;
                }

            }
        }
        return numberInverions;
    }


    //renvoie true si la combinaison est soluble sinon false
    public bool IsSoluble(int[] indexRandom)
    {
        Debug.Log(CountInversion(indexRandom));
        //le puzzle est résoluble lorsque le nombre d'inversions est pair
        if (CountInversion(indexRandom) % 2 != 0)
        {
            return false;
        }
        return true;
    }

    //on rend soluble un tableau insoluble
    public int[] SwapIndex(int[] indexRandom)
    {
        int[] indexSoluble = indexRandom;

        //on change de place les 2 premier
        if (indexSoluble[0] != 0 & indexSoluble[0] != 1)
        {
            int temp = indexSoluble[0];
            indexSoluble[0] = indexSoluble[1];
            indexSoluble[1] = temp;
        }
        //sinon les 2 dernier
        else
        {
            int length = indexRandom.Length - 1;
            int temp = indexRandom[length];
            indexSoluble[length] = indexSoluble[length - 1];
            indexSoluble[length - 1] = temp;
        }

        return indexSoluble;
    }

    //tant que  non soluble
    public int[] SwapIndexWhileNotSoluble(int[] indexRandom)
    {
        int[] indexSoluble = indexRandom;

        while (IsSoluble(indexSoluble) == false)
        {
            SwapIndex(indexSoluble);
        }

        return indexSoluble;
    }


}
