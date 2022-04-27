using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SortTest1: MonoBehaviour
{
    private void Start()
    {
        Create();
    }

    private void Create()
    {
        List<int> list = new List<int>();

        list.Add(0);
        list.Add(3);
        list.Add(-2);
        list.Add(0);
        list.Add(4);
        list.Add(0);
        list.Add(6);
        list.Add(11);
        list.Add(9);


        foreach (int i in list.TestSort<int>((x, y) => x.CompareTo(y)))
        {
            Debug.Log(i);
        }

        
    }



}

public static class Test
{
    public static List<T> TestSort<T>(this IEnumerable list, Func<T,T,int> compare) 
    {
        List<T> li = new List<T>();
        foreach(T item in list)
        {
            li.Add(item);
        }

        for(int i=0; i < li.Count - 1; i++)
        {
            for(int j=i+1; j<li.Count; j++)
            {
                if(compare(li[i], li[j]) == -1)
                {
                    T temp = li[j];
                    li[j] = li[i];
                    li[i] = temp;
                }
            }
        }

        return li;
    }
    
}