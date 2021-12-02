using System.Collections.Generic;
using System;

[Serializable]
public class UpdateGameStatusResponse : ResponseObject
{
    public Utils.GameStatus gameStatus;
}
