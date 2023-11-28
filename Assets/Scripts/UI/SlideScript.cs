using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlideScript : MonoBehaviour
{
    public List<GameObject> slides = new List<GameObject>();
    public int index;
    void Start() {
        index = 0;
        slides[index].SetActive(true);
    }

    // Update is called once per frame
    void Update() {

    }

    public void NextSlide() {
        if (index == slides.Count - 1) {
            slides[index].SetActive(false);
            index = 0;
            slides[index].SetActive(true);
        }
        else {
            slides[index].SetActive(false);
            index++;
            slides[index].SetActive(true);
        }

    }

    public void PreviousSlide() {
        if (index <= 0) {
            slides[index].SetActive(false);
            index = slides.Count - 1;
            slides[index].SetActive(true);
        }
        else {
            slides[index].SetActive(false);
            index--;
            slides[index].SetActive(true);
        }

    }
}
