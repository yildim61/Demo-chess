using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Main : MonoBehaviour
{
    [SerializeField] Transform startPosition;
    [SerializeField] GameObject prefabWhite;
    [SerializeField] GameObject prefabBlack;
    [SerializeField] GameObject knight;
    [SerializeField] GameObject target;
    [SerializeField] GameObject iz;
    public int knightPosX ; // At ın konumunu tutacak değişkenler
     public int knightPosY;
    public int targetPosX;   // hedefin konumunu tutacak değikenler
    public int targetPosY; 
    public Camera cam;
    public int t;
    public Text text;
    public float speed;
    public InputField input;
    [SerializeField] GameObject panel;
    [SerializeField] GameObject panel2;
    public List<Cell> list = new List<Cell>();
    public List<Cell> list2 = new List<Cell>();
    Queue<Cell> kuyruk = new Queue<Cell>();
    Queue<Cell> kuyruk2 = new Queue<Cell>();

    void Start()
    { 
       panel.SetActive(true); 
    }
    void Update()
    {
        text.text =(t.ToString()+ ". Adım" ) ;//Adım sayısını ekrana yazdır
        if(GameObject.FindGameObjectWithTag("Knight") == null) // at yerleştirilmediyse 
        {
            AddKnight(); //yerleştir
        }
        else if (GameObject.FindGameObjectWithTag("Target") == null) // hedef yerleştirilmediyse
        {
            AddTarget(); //hedef belirle
        }   
           
    }

    public void CreatePlatform(float x) // Santranç Tahtasını oluştur
    {
        float y = x;
        bool b = true;

        if (x%2 == 0) // N çift ise
        {
            for (int i = 0; i < x; i++) // x ekseni
            {       
                for (int j = 0; j < y; j++) // y ekseni
                {
                    if (b) // Beyaz
                    {
                        Instantiate(prefabWhite,new Vector3(startPosition.position.x + i,startPosition.position.y,startPosition.position.z+j)
                        ,startPosition.rotation);
                        b = false;
                    if (y-1 == j)
                    {
                        b = true; 
                    }
                    }
                    else // Siyah
                    {
                        Instantiate(prefabBlack,new Vector3(startPosition.position.x + i,startPosition.position.y,startPosition.position.z+j)
                        ,startPosition.rotation);
                        b = true;
                        if (y-1 == j)
                        {
                            b = false; 
                        }
                    }                                 
                }               
            }
        }
        else // N tek ise
        {
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
        
    }
    public bool AddKnight() // At ı yerleştirme 
    {
        if(Input.GetMouseButtonDown(0)) 
        {
            RaycastHit hit;
            Ray ray =Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, 200.0f))
        {
            if (hit.transform != null)
            {
                Instantiate(knight,hit.transform.position,hit.transform.rotation);
                knightPosX = (int)hit.transform.position.x; 
                knightPosY =  (int)hit.transform.position.z;
            }
        } 
        }   
        return true;
    }
    public bool AddTarget() // hedefi yerleştirme 
    {
        if(Input.GetMouseButtonDown(0)) 
        {
            RaycastHit hit;
            Ray ray =Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, 200.0f))
        {
            if (hit.transform != null)
            {
                Instantiate(target,hit.transform.position,hit.transform.rotation);
                targetPosX = ((int)hit.transform.position.x); 
                targetPosY = ((int)hit.transform.position.z);
                int[] knightPos = {knightPosX,knightPosY};
                int[] targetPos = {targetPosX,targetPosY};
                Cell y = Hesapla(knightPos,targetPos,int.Parse(input.GetComponent<InputField>().text));
                FindPoints(list,targetPosX,targetPosY);
                StartCoroutine(Move());
                return true;
                
            }
        } 
        } 
        return false;  
    }

public Cell Hesapla(int[] knightPos, int[] targetPos,int N)
{
    int[] dx = { -2, -1, 1, 2, -2, -1, 1, 2 };  // gidilebilecek yerleri hesaplamak için x değerleri
    int[] dy = { -1, -2, -2, -1, 1, 2, 2, 1 };   // gidilebilecek yerleri hesaplamak için y değerleri

    kuyruk.Enqueue(new Cell(knightPos[0],knightPos[1],knightPos[0],knightPos[1], 0));   // kuyruğa ilk pozisyonu yerleştir
    
    Cell c;
    
    int x, y;       // Anlık x ve y koordinatları
    int bx,by;      // Bir önceki konumun x,y kooordinatları

    bool[ , ] visit = new bool[N + 1, N + 1];   // gidilen noktaları işaretlemek için 2 boyutlu dizi

    for (int i = 1; i <= N; i++){       // Santranç tahtası üzerinde bütün hücreleri gidilmemiş durumuna getir
            for (int j = 1; j <= N; j++){
                visit[i, j] = false;
            }
    } 

    visit[knightPos[0], knightPos[1]] = true;   // ilk noktayı daha önce gidilmiş olarak işaretle

     while (kuyruk.Count != 0) {    // kuyrukta eleman kalmayana kadar tekrarla

            c = kuyruk.Peek();      // Kuyruğun ilk elemanını al
            kuyruk.Dequeue();       // ilk elemanı kaldır
    
            if (c.x == targetPos[0] && c.y == targetPos[1]){        // Hücre, hedef hücre ise  
                return c;                                   // adım sayısını geri döndür
            }

            for (int i = 0; i < 8; i++) {       // tüm gidilebilir hücreleri hesapla
                bx = c.x;                   // bx i önceki adımın x koordinatı olarak ekle
                by = c.y;                   // by yi  önceki adımın y koordinatı olarak ekle
                x = c.x + dx[i];             // bir sonraki x kooordinatını hesapla
                y = c.y + dy[i];          // bir sonraki y koordinatını hesapla
                
                if (isInside(x, y, N) && !visit[x, y]) {        // bu nokta tahta dışına çıkmıyorsa ve daha önce gidilmemiş ise
                    visit[x, y] = true;                             // Bu noktayı gidilmiş olarak işaretle
                    kuyruk.Enqueue(new Cell(x, y,bx,by,c.dis + 1));            // kuyruğa hücreyi ekle
                    list.Add(new Cell(x,y,bx,by,c.dis+1));                   // listeye hücreyi ekle
                   
                }
            }                       
        }   
       return null;
}


public void FindPoints(List<Cell> list,int posX, int posY) // At ın hedefe varırken geçtiği noktaları bul
{   
        for (int i = 0; i < list.Count; i++)
        {   
            if(list[i].x == posX && list[i].y == posY){
               list2.Add(list[i]);
                FindPoints(list,list[i].bx,list[i].by);
            }
        }
}
IEnumerator Move() // At ın ilerlemesini sağla
{   
    for (int i = list2.Count-1; i >= 0; i--)
    {
        knight = GameObject.FindGameObjectWithTag("Knight");
        Vector3 a = knight.transform.position;   // At ın o anki konumunu bul
        Vector3 b = new Vector3(list2[i].x,0,list2[i].y); // At ın gideceği vektörü bul
        Instantiate(iz,a,startPosition.rotation);  // Hücrenin uğramış olunduğunu göster
        yield return new WaitForSeconds(1);       // At ın ilerlemesi için zaman tanı
        
        knight.transform.position = b;          // At ı ilerlet
        t++;
        yield return new WaitForSeconds(1);     // At ın ilerlemesi için zaman tanı
        
   }
    
}

public void ButtonPress()// Başlama buton fonksiyonu
{ 
    int n = int.Parse(input.GetComponent<InputField>().text) ; // kullanıcıdan alınan N bilgisi
    if (n>3 && n<17)
    {
        panel.SetActive(false); // !. Paneli kapat
        CreatePlatform(n); // Santranç Tahtasını oluştur
        cam.transform.position = new Vector3( float.Parse(input.GetComponent<InputField>().text)/2,12.968f,-4.88f);   
        panel2.SetActive(true); // 2. Paneli aç
    }
    
}
    
public void PrintList(List<Cell> li){    // liste içeriğini yazdır
    for (int i = 0; i < li.Count; i++)
    {
        Debug.Log(string.Format("Listede {0},{1} var",li[i].x,li[i].y));
    }
}

static bool isInside(int x, int y, int N) // Hücrenin oyun alanı içerisinde olup olmadığı kontrolü
    {
        if (x >= 1 && x <= N && y >= 1 && y <= N)
            return true;
        return false;
    }

void PrintName(GameObject go )
    {
        Debug.Log(go.transform.position); 
    }

public void LoadScene()  // Sahneyi yükle
{
    panel2.SetActive(false);
    SceneManager.LoadScene(0);
}
public void Quit()      // Uygulamayı kapat
{
    Application.Quit();
}
}