using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject prefabWhite;
    [SerializeField] GameObject prefabBlack;
    [SerializeField] Transform startPosition;
     public InputField input;
     [SerializeField] GameObject panel;
     [SerializeField] GameObject hedef;
     [SerializeField] Move _move;
     Cell c;
     

    
    // Start is called before the first frame update
    void Start()
    {
        panel.SetActive(true);
        _move = FindObjectOfType<Move>();
        c=GetComponent<Cell>();
       

        //CreatePlatform(7);
        
        
    }

    public void CreatePlatform(float x)
    {
        //float x = 3 ;
        float y = x;

        bool b = true;
        
        for (int i = 0; i < x; i++)
        {   
            for (int j = 0; j < y; j++)
            {
                if (b)
                {
                    Instantiate(prefabWhite,new Vector3(startPosition.position.x + i,startPosition.position.y,startPosition.position.z+j)
                ,startPosition.rotation);
                b = false;
                }
                else
                {
                    Instantiate(prefabBlack,new Vector3(startPosition.position.x + i,startPosition.position.y,startPosition.position.z+j)
                ,startPosition.rotation);
                b = true;
                }                                 
            }            
        }
    }

    // Update is called once per frame
    void Update()
    {   
        if(Input.GetMouseButtonDown(0)) 
        {
            RaycastHit hit;
            Ray ray =Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, 200.0f))
        {
            if (hit.transform != null)
            {
                PrintName(hit.transform.gameObject);
                Instantiate(hedef,hit.transform.position,hit.transform.rotation);
                
                int[] knightPos = { 1, 1 };
                int[] targetPos = { (int)hit.transform.position.x+1,(int)hit.transform.position.z+1 };

                //_move.target.position= hit.transform.position;

                
                
               // Debug.Log( minStepToReachTarget(knightPos,targetPos,7));
                //Cursor.visible = false;
                //Cursor.lockState = CursorLockMode.Locked;
            }
        }
        }        
        
    }
    void PrintName(GameObject go )
    {
        Debug.Log(go.transform.position);
       
    }
    public void ButtonPress()
    {
        panel.SetActive(false);
        CreatePlatform(int.Parse(input.GetComponent<InputField>().text));
        
    }
    
    static bool isInside(int x, int y, int N)
    {
        if (x >= 1 && x <= N && y >= 1 && y <= N)
            return true;
        return false;
    }



    public int minStepToReachTarget(int[] knightPos,int[] targetPos, int N)
    {
        // At'ın gidebileceği vektörler
        int[] dx = { -2, -1, 1, 2, -2, -1, 1, 2 };
        int[] dy = { -1, -2, -2, -1, 1, 2, 2, 1 };
 
        // At'ın gidebileceği yerleri tutacak olan kuyruk
        Queue<Cell> kuyruk = new Queue<Cell>();
 
        // push starting position of knight with 0 distance
        //kuyruk.Enqueue(new Cell(knightPos[0],knightPos[1], 0));
 
        
        int x, y;
        bool[, ] visit = new bool[N + 1, N + 1];
 
        // make all cell unvisited
        for (int i = 1; i <= N; i++)
            for (int j = 1; j <= N; j++)
                visit[i, j] = false;
 
        // visit starting state
        visit[knightPos[0], knightPos[1]] = true;
        
 
        // kuyruktaki elemanları kontrol et
        while (kuyruk.Count != 0) {
            c = kuyruk.Peek(); // Kuyruğun ilk elemanını al
            kuyruk.Dequeue();   // ilk elemanı kaldır
            
            

            //Vector3 cv = new Vector3(c.x,0,c.y);
            //_move.Goto(cv);
           
 
            // Hücre, hedef hücre ise
            if (c.x == targetPos[0] && c.y == targetPos[1])
                return c.dis;
 
            // tüm gidilebilir hücreleri hesapla
            for (int i = 0; i < 8; i++) {
                //x = c.x + dx[i];
                //y = c.y + dy[i];
 
                // If reachable state is not yet visited and
                // inside board, push that state into queue
               /* if (isInside(x, y, N) && !visit[x, y]) {
                    visit[x, y] = true;
                    kuyruk.Enqueue(new Cell(x, y, c.dis + 1));// kuyruğa hücreyi ekle
                    
                    
                }*/
            }
            
            
            
        }
        return 0;
    }

}
