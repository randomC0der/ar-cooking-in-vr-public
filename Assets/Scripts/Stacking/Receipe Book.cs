using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
class ReceipeBook
{
    public Receipe[] recipes;
}

[Serializable]
class Receipe
{
    /// <see cref="StackableBehavior.ingredient"/>
    public string[] ingredients;
    /// <summary>
    /// Resource name of the result
    /// </summary>
    public string product;
}
