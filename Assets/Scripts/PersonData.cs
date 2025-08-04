using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PersonData : MonoBehaviour
{
    public Image Neck, Head, ArmRight, ArmLeft;
    public Image[] Faces, Hair, Shirt, SleevesLeft, SleevesRight;

    public Image currentNeck, currentHead, currentArmRight, currentArmLeft, currentFace, currentHair, currentShirt, currentSleeveLeft, currentSleeveRight;

    private void Start()
    {
        currentNeck = Neck;
        currentHead = Head;
        currentArmLeft = ArmLeft;
        currentArmRight = ArmRight;
    }
}
