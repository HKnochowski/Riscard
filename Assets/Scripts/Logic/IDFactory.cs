using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class IDFactory {

    private static int Count;

    public static int GetUniqueID()
    {
        
        Count++;
        return Count;
    }
	
	 public static void ResetIDs()
    {
        Count = 0;
    }


}
