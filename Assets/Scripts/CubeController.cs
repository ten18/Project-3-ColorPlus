using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeController : MonoBehaviour
{
    public int myX, myY;
    public static float scaleX, scaleY, scaleZ;
    public static bool activeCube;
    GameObject clickedCube;
    public static GameObject hoveredCube, exitedCube; // the cube that is being hovered over and the cube that the mouse recently stopped hovering over, respectively
    Color colorSaved; // saves the color of the cube that is being moved
    GameObject oldCube; // the cube that is moving to a different space
    public static float highlightMultiplier; // the factor by which a cube highlights when moused over
    // Start is called before the first frame update
    void Start()
    {
        scaleX = 0.25f;
        scaleY = 0.25f;
        scaleZ = 0.25f;
        highlightMultiplier = 5f; // the factor by which a cube highlights (and de-highlights) during mouse-over interaction
    }

    // Update is called once per frame
    void Update()
    {
    }

    void OnMouseDown()
    {
        if (GameController.disableClicking == false)
        {
            clickedCube = GameController.grid[myX, myY];
            if (clickedCube.GetComponent<Renderer>().material.color != Color.white && clickedCube.GetComponent<Renderer>().material.color != Color.black) // If you click on a colored cube...
            {
                if (activeCube && clickedCube == GameController.activeCubeObject) // deactivate if active
                {
                    activeCube = false;
                    GameController.grid[GameController.activeCubeX, GameController.activeCubeY].transform.localScale -= new Vector3(scaleX, scaleY, scaleZ);
                }
                else if (activeCube && clickedCube != GameController.activeCubeObject)
                {
                    GameController.grid[GameController.activeCubeX, GameController.activeCubeY].transform.localScale -= new Vector3(scaleX, scaleY, scaleZ);
                    GameController.activeCubeX = myX;
                    GameController.activeCubeY = myY;
                    GameController.grid[GameController.activeCubeX, GameController.activeCubeY].transform.localScale += new Vector3(scaleX, scaleY, scaleZ);
                }
                else // activate if not active
                {
                    activeCube = true;
                    GameController.activeCubeX = myX;
                    GameController.activeCubeY = myY;
                    GameController.grid[GameController.activeCubeX, GameController.activeCubeY].transform.localScale += new Vector3(scaleX, scaleY, scaleZ);
                }
            }
            else if (clickedCube.GetComponent<Renderer>().material.color == Color.white) // If you click on a white cube...
            {
                if (activeCube) // and there is an active colored cube...
                {
                    // and the white cube is adjacent to said active cube (including diagonals)...
                    if (myX < (GameController.activeCubeX + 2) && myX > (GameController.activeCubeX - 2) && myY < (GameController.activeCubeY + 2) && myY > (GameController.activeCubeY - 2))
                    {
                        // move colored cube to new space
                        oldCube = GameController.grid[GameController.activeCubeX, GameController.activeCubeY];
                        oldCube.transform.localScale -= new Vector3(scaleX, scaleY, scaleZ);
                        colorSaved = (oldCube.GetComponent<Renderer>().material.color);
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
    // NOTE: I took out mouse-over interaction because it involved changing the color of the cubes. This caused bugs with the pre-existing code.
    /*
    void OnMouseEnter()
    {
        if (GameController.disableClicking == false)
        {
            // highlights cube when moused over
            hoveredCube = GameController.grid[myX, myY];
            if (hoveredCube.GetComponent<Renderer>().material.color != Color.white && hoveredCube.GetComponent<Renderer>().material.color != Color.black)
            {
                hoveredCube.GetComponent<Renderer>().material.color *= highlightMultiplier;
            }
        } 
    }

    void OnMouseExit()
    {
        if (GameController.disableClicking == false)
        {
            // de-highlights cube when mouse stops hovering over it
            exitedCube = GameController.grid[myX, myY];
            if (exitedCube.GetComponent<Renderer>().material.color != Color.white && hoveredCube.GetComponent<Renderer>().material.color != Color.black)
            {
                exitedCube.GetComponent<Renderer>().material.color /= highlightMultiplier;
            }
        }
    }
    */
}
