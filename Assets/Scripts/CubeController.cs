using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeController : MonoBehaviour
{
    public int myX, myY;
    public static float scaleX, scaleY, scaleZ;
    public static bool activeCube;
    GameObject clickedCube;
    Color colorSaved; // saves the color of the cube that is being moved
    GameObject oldCube; // the cube that is moving to a different space
    // Start is called before the first frame update
    void Start()
    {
        scaleX = 0.25f;
        scaleY = 0.25f;
        scaleZ = 0.25f;
    }

    // Update is called once per frame
    void Update()
    {
    }

    void OnMouseDown()
    {
        clickedCube = GameController.grid[myX, myY];
        if (clickedCube.GetComponent<Renderer>().material.color != Color.white && clickedCube.GetComponent<Renderer>().material.color != Color.black) // If you click on a colored cube...
        {
            if (activeCube) // deactivate if active
            {
                activeCube = false;
                GameController.grid[GameController.activeCubeX, GameController.activeCubeY].transform.localScale -= new Vector3(scaleX, scaleY, scaleZ);
            }
            else // activate if not active
            {
                activeCube = true;
                GameController.activeCubeX = myX;
                GameController.activeCubeY = myY;
                GameController.grid[GameController.activeCubeX, GameController.activeCubeY].transform.localScale += new Vector3(scaleX, scaleY, scaleZ);
            }
        }
        else if(clickedCube.GetComponent<Renderer>().material.color == Color.white) // If you click on a white cube...
        {
            if (activeCube) // and there is an active colored cube...
            {
                // and the white cube is adjacent to said active cube (including diagonals)...
                if (myX < (GameController.activeCubeX + 2) && myX > (GameController.activeCubeX - 2) && myY < (GameController.activeCubeY + 2) && myY > (GameController.activeCubeY - 2))
                {
                    // move colored cube to new space
                    oldCube = GameController.grid[GameController.activeCubeX, GameController.activeCubeY];
                    oldCube.transform.localScale -= new Vector3(scaleX, scaleY, scaleZ);
                    colorSaved = oldCube.GetComponent<Renderer>().material.color;
                    oldCube.GetComponent<Renderer>().material.color = Color.white;
                    GameController.activeCubeX = myX;
                    GameController.activeCubeY = myY;
                    GameController.grid[GameController.activeCubeX, GameController.activeCubeY] = clickedCube;
                    clickedCube.GetComponent<Renderer>().material.color = colorSaved;
                    clickedCube.transform.localScale += new Vector3(scaleX, scaleY, scaleZ);
                }
            }
        }
    }
}
