using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class ClickableMonoBehaviour : MonoBehaviour {
	abstract public int OnClick (BoardManager ctx);
}
