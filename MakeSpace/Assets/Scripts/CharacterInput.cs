using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum InputType { Move, Repair, Cutter, Oxygen, Jetpack, Inflatable }

public class CharacterInput
{
    public Direction dir;
    public InputType inputType;
}
