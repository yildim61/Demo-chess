using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public  class Cell : MonoBehaviour
{
    public int x, y;
    public int bx,by;
    public int dis;
  
    public Cell(int x, int y,int bx, int by, int dis)
    {   
        this.x = x;
        this.y = y;
        this.bx = bx;
        this.by = by;
        this.dis = dis;
        
    }
    public void  writeCell()
    {
        
    }
}
