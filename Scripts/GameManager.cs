using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour{

    public int frameRate;

    void Start(){
        Application.targetFrameRate = frameRate;
    }

    void Update(){
        
    }
}
