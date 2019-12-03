using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public float turnLength;
    public float turn;
    public float timeLeft;
    public GameObject cubePrefab;
    public static GameObject[,] grid;
    public GameObject nextCube;
    GameObject cubeSelected;
    public static int activeCubeX, activeCubeY;
    GameObject activeCubeObject;
    Color[] colorList = { Color.blue, Color.green, Color.red, Color.yellow, Color.magenta };
    Color color;
    public int gridLength, gridHeight;
    public float gridSpacing;
    public Text timerText;
    public Text scoreText;
    Vector3 cubePos;
    Vector3 nextCubePos;
    int gridRow; // keeps track of what row to place the cube in
    GameObject[] row1, row2, row3, row4, row5;
    bool row1Full, row2Full, row3Full, row4Full, row5Full;
    public int score;
    public int pointsSameColorPlus, pointsDiffColorPlus;
    public int penalty;
    GameObject centerCube, rightCube, leftCube, topCube, bottomCube;
    Color centerColor, rightColor, leftColor, topColor, bottomColor;
    bool sameColorPlus, diffColorPlus;
    string gameOverStatus;
    bool keyPressFault = false; // will turn true if a player tries to place a cube in a full row, resulting in losing the game
    bool outOfRoom = false; // will turn true if a black cube should be made but there is no more room for black cubes, resulting in the game ending

    void GenNextCube()
    {
        nextCube = Instantiate(cubePrefab, nextCubePos, Quaternion.identity);
        nextCube.GetComponent<Renderer>().material.color = colorList[Random.Range(0,colorList.Length)];
        color = nextCube.GetComponent<Renderer>().material.color;
    }

    void PlaceColorCube() // issues: black cubes appear regardless of whether or not a key was pressed 
    {                     
        if (gridRow != -1 && nextCube != null)
        {
            cubeSelected = grid[Random.Range(0, gridLength), gridRow];
            if (cubeSelected.GetComponent<Renderer>().material.color == Color.white)
            {
                cubeSelected.GetComponent<Renderer>().material.color = color;
                Destroy(nextCube);
                nextCube = null;
            }
            else
            {
                PlaceColorCube();
            }
            gridRow = -1;
        }
    }

    void CheckForKeyPress()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1) && row1Full == false)
        {
            gridRow = 4;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha1) && row1Full == true)
        {
            keyPressFault = true;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2) && row2Full == false)
        {
            gridRow = 3;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2) && row2Full == true)
        {
            keyPressFault = true;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3) && row3Full == false)
        {
            gridRow = 2;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3) && row3Full == true)
        {
            keyPressFault = true;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4) && row4Full == false)
        {
            gridRow = 1;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4) && row4Full == true)
        {
            keyPressFault = true;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5) && row5Full == false)
        {
            gridRow = 0;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5) && row5Full == true)
        {
            keyPressFault = true;
        }
    }

    void CheckForFullRows()
    {
        // these bools are set to true every time, but each will continue to be set to false if even a single cube is white in the respective row
        row1Full = true;
        row2Full = true;
        row3Full = true;
        row4Full = true;
        row5Full = true;
        for (int i = 0; i < row1.Length; i++)
        {
            if (row1[i].GetComponent<Renderer>().material.color == Color.white)
            {
                row1Full = false;
            }
        }
        for (int i = 0; i < row2.Length; i++)
        {
            if (row2[i].GetComponent<Renderer>().material.color == Color.white)
            {
                row2Full = false;
            }
        }
        for (int i = 0; i < row3.Length; i++)
        {
            if (row3[i].GetComponent<Renderer>().material.color == Color.white)
            {
                row3Full = false;
            }
        }
        for (int i = 0; i < row4.Length; i++)
        {
            if (row4[i].GetComponent<Renderer>().material.color == Color.white)
            {
                row4Full = false;
            }
        }
        for (int i = 0; i < row5.Length; i++)
        {
            if (row5[i].GetComponent<Renderer>().material.color == Color.white)
            {
                row5Full = false;
            }
        }
    }

    void MakeCubeBlack()
    {
        try {
            cubeSelected = grid[Random.Range(0, gridLength), Random.Range(0, gridHeight)];
            if (cubeSelected.GetComponent<Renderer>().material.color == Color.white)
            {
                cubeSelected.GetComponent<Renderer>().material.color = Color.black;
            }
            else
            {
                MakeCubeBlack();
            }
        }
        catch
        {
            outOfRoom = true;
        }
        
    }

    void CheckSameColorPlus()
    {
        for (int y = 1; y < gridHeight-1; y++)
        {
            for (int x = 1; x < gridLength-1; x++)
            {
                centerCube = grid[x, y];
                centerColor = centerCube.GetComponent<Renderer>().material.color;
                rightCube = grid[x + 1, y];
                rightColor = rightCube.GetComponent<Renderer>().material.color;
                leftCube = grid[x - 1, y];
                leftColor = leftCube.GetComponent<Renderer>().material.color;
                topCube = grid[x, y + 1];
                topColor = topCube.GetComponent<Renderer>().material.color;
                bottomCube = grid[x, y - 1];
                bottomColor = bottomCube.GetComponent<Renderer>().material.color;
                if (centerColor != Color.white && centerColor != Color.black) // only check colored cubes
                {
                    // if all cubes in + match in color...
                    if (centerColor == rightColor && centerColor == leftColor && centerColor == topColor && centerColor == bottomColor)
                    {
                        // turn all cubes in the + shape black
                        centerCube.GetComponent<Renderer>().material.color = Color.black;
                        rightCube.GetComponent<Renderer>().material.color = Color.black;
                        leftCube.GetComponent<Renderer>().material.color = Color.black;
                        topCube.GetComponent<Renderer>().material.color = Color.black;
                        bottomCube.GetComponent<Renderer>().material.color = Color.black;
                        // also deactivate the active cube if it is part of the + shape
                        if (activeCubeObject == centerCube || activeCubeObject == rightCube || activeCubeObject == leftCube || activeCubeObject == topCube || activeCubeObject == bottomCube)
                        {
                            if (CubeController.activeCube == true)
                            {
                                CubeController.activeCube = false;
                                activeCubeObject.transform.localScale -= new Vector3(CubeController.scaleX, CubeController.scaleY, CubeController.scaleZ);
                            }
                        }
                        sameColorPlus = true;
                    }
                }
            }
        }
    }

    void CheckDiffColorPlus()
    {
        for (int y = 1; y < gridHeight - 1; y++)
        {
            for (int x = 1; x < gridLength - 1; x++)
            {
                centerCube = grid[x, y];
                centerColor = centerCube.GetComponent<Renderer>().material.color;
                rightCube = grid[x + 1, y];
                rightColor = rightCube.GetComponent<Renderer>().material.color;
                leftCube = grid[x - 1, y];
                leftColor = leftCube.GetComponent<Renderer>().material.color;
                topCube = grid[x, y + 1];
                topColor = topCube.GetComponent<Renderer>().material.color;
                bottomCube = grid[x, y - 1];
                bottomColor = bottomCube.GetComponent<Renderer>().material.color; // only check colored cubes
                if (centerColor != Color.white && centerColor != Color.black)
                {
                    // the next 4 if statements are all to check if every cube in the + is of a unique color (excluding white and black)
                    if (centerColor != rightColor && centerColor != leftColor && centerColor != topColor && centerColor != bottomColor)
                    {
                        if (rightColor != leftColor && rightColor != topColor && rightColor != bottomColor && rightColor != Color.white && rightColor != Color.black)
                        {
                            if (leftColor != topColor && leftColor != bottomColor && leftColor != Color.white && leftColor != Color.black)
                            {
                                if (topColor != bottomColor && topColor != Color.white && topColor != Color.black && bottomColor != Color.white && bottomColor != Color.black)
                                {
                                    // turn all cubes in the + shape black
                                    centerCube.GetComponent<Renderer>().material.color = Color.black;
                                    rightCube.GetComponent<Renderer>().material.color = Color.black;
                                    leftCube.GetComponent<Renderer>().material.color = Color.black;
                                    topCube.GetComponent<Renderer>().material.color = Color.black;
                                    bottomCube.GetComponent<Renderer>().material.color = Color.black;
                                    // also deactivate the active cube if it is part of the + shape
                                    if (activeCubeObject == centerCube || activeCubeObject == rightCube || activeCubeObject == leftCube || activeCubeObject == topCube || activeCubeObject == bottomCube)
                                    {
                                        if (CubeController.activeCube == true)
                                        {
                                            CubeController.activeCube = false;
                                            activeCubeObject.transform.localScale -= new Vector3(CubeController.scaleX, CubeController.scaleY, CubeController.scaleZ);
                                        }
                                    }
                                    diffColorPlus = true;
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    bool IsGameOver()
    {
        if (timeLeft <= 0)
        {
            if (score > 0)
            {
                gameOverStatus = "You Win!";
                timerText.color = Color.green;
            }
            return true;
        }
        else if (keyPressFault == true)
        {
            timerText.color = Color.red;
            return true;
        }
        else if (outOfRoom == true)
        {
            if (score > 0)
            {
                gameOverStatus = "You Win!";
                timerText.color = Color.green;
            }
            else
            {
                timerText.color = Color.red;
            }
            return true;
        }
        else
        {
            return false;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        turnLength = 2f;
        turn = turnLength;
        timeLeft = 60f;
        gridLength = 8;
        gridHeight = 5;
        gridSpacing = 1.5f;
        grid = new GameObject[gridLength, gridHeight];
        for (int y = 0; y < gridHeight; y++)
        {
            for (int x = 0; x < gridLength; x++)
            {
                cubePos = new Vector3(x * gridSpacing, y * gridSpacing, 0);
                grid[x, y] = Instantiate(cubePrefab, cubePos, Quaternion.identity);
                grid[x, y].GetComponent<CubeController>().myX = x;
                grid[x, y].GetComponent<CubeController>().myY = y;
            }
        }
        nextCubePos = new Vector3(gridLength * gridSpacing, gridHeight * gridSpacing, 0);
        GenNextCube();
        gridRow = -1; // no keys have been pressed yet
        row1 = new GameObject[] {grid[0,4], grid[1,4], grid[2,4], grid[3,4], grid[4,4], grid[5,4], grid[6,4], grid[7,4]};
        row2 = new GameObject[] {grid[0,3], grid[1,3], grid[2,3], grid[3,3], grid[4,3], grid[5,3], grid[6,3], grid[7,3]};
        row3 = new GameObject[] {grid[0,2], grid[1,2], grid[2,2], grid[3,2], grid[4,2], grid[5,2], grid[6,2], grid[7,2]};
        row4 = new GameObject[] {grid[0,1], grid[1,1], grid[2,1], grid[3,1], grid[4,1], grid[5,1], grid[6,1], grid[7,1]};
        row5 = new GameObject[] {grid[0,0], grid[1,0], grid[2,0], grid[3,0], grid[4,0], grid[5,0], grid[6,0], grid[7,0] };
        score = 0;
        pointsSameColorPlus = 10;
        pointsDiffColorPlus = 5;
        penalty = 1;
        gameOverStatus = "You Lose"; // this will change if the players meet the criteria for winning
    }

    // Update is called once per frame
    void Update()
    {
        if (sameColorPlus == true)
        {
            score += pointsSameColorPlus;
        }
        sameColorPlus = false;
        if (diffColorPlus == true)
        {
            score += pointsDiffColorPlus;
        }
        diffColorPlus = false;
        scoreText.text = "Score: " + score;
        if (timeLeft <= 10 && IsGameOver() == false)
        {
            timerText.color = Color.red;
        }
        if (IsGameOver() == true)
        {
            Destroy(nextCube);
            timerText.text = gameOverStatus;
        }
        else
        {
            activeCubeObject = grid[activeCubeX, activeCubeY];
            CheckForKeyPress();
            PlaceColorCube();
            CheckForFullRows();
            CheckSameColorPlus();
            CheckDiffColorPlus();
            IsGameOver();
            if (timeLeft > 0)
            {
                timeLeft -= Time.deltaTime;
                timerText.text = timeLeft.ToString("F2");
            }
            if (Time.time > turn)
            {
                turn += turnLength;
                if (nextCube != null) // if the nextCube is still there i.e. the player hasn't pressed one of the keys within the turn
                {
                    Destroy(nextCube);
                    MakeCubeBlack();
                    if (score > 0)
                    {
                        score -= penalty;
                    }
                }
                GenNextCube();
                print("Score: " + score);
            }
        }
    }
}
