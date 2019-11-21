using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using System;
using System.Reflection;

public class TestHierarchyIcons : MonoBehaviour
{
    [Required][SerializeField] GameObject testValue;
    [Balance] [SerializeField] float testValueTwo;
    [Balance] [SerializeField] int three;
}