using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CalculateShanten : MonoBehaviour
{

    public int[] hand;
    public int shanten;


    private int[] hc;

    // acts like Hash table
    void init_hash()
    {
        hc = new int[889];
        
        for (int i1 = 0; i1 < 9; i1++)
        {
            for (int i2 = 0; i2 < 9; i2++)
            {
                for (int i3 = 0; i3 < 9; i3++)
                {
                    int ind = i1 * 100 + i2 * 10 + i3;
                    if (i1 == i2 && i1 == i3) // Same 3
                        hc[ind] = 2;
                    else if (i1 + 1 == i2 && i2 + 1 == i3) // Consecutive
                        hc[ind] = 2;
                    else if (i1 - i2 < 3 && i1 - i2 > -3)
                        hc[ind] = 1;
                    else if (i2 - i3 < 3 && i2 - i3 > -3)
                        hc[ind] = 1;
                    else
                        hc[ind] = 0;
                }
            }
        }
        
    }

    // GetMaxBody : Returns max body count of hand
    // hand is composed of values 0 to 9.
    // Suppose that list is sorted.
    int GetMaxBody(List<int> h)
    {
        if (h.Count <= 1) // One cards, doesn't have to count
        {
            return 0;
        }
        else if (h.Count == 2) // if just two cards, there's possibility to have body candidate
        {
            return (h[0] - h[1] < 3 && h[0] - h[1] > -3) ? 1 : 0;
        }
        else if (h.Count == 3) // Basic body composition starts
        {
            return hc[h[0] * 100 + h[1] * 10 + h[2]];
        }
        else // More than 5, just do the recursive method to find best hand posibility
        {
            // Divide into two group if the value is changing a lot            
            for (int i = 1; i < h.Count; i++)
            {
                if (h[i] - h[i-1] > 2)
                {
                    List<int> h1 = new List<int>(h.GetRange(0,i));
                    List<int> h2 = new List<int>(h.GetRange(i, h.Count - i));
                    return GetMaxBody(h1) + GetMaxBody(h2);
                }
            }

            // Or, pick 3 values
            int max_r = 0;
            for (int i1 = 0; i1 < h.Count-2; i1++)
            {
                for (int i2 = 1; i2 < h.Count-1; i2++)
                {
                    if (i1 >= i2)
                        continue;
                    for (int i3 = 2; i3 < h.Count; i3++)
                    {
                        if (i1 >= i3 || i2 >= i3)
                            continue;
                        // Debug.Log(i1.ToString() + " " + i2.ToString() + " " + i3.ToString() + " " );
                        List<int> h1 = new List<int>(h); // Remove from here
                        h1.RemoveAt(i3); h1.RemoveAt(i2); h1.RemoveAt(i1);
                        int r_ = hc[h[i1] * 100 + h[i2] * 10 + h[i3]] + GetMaxBody(h1);
                        if (max_r < r_)
                            max_r = r_;
                    }
                }
            }
            // Debug.Log(h.Count.ToString() + " " + max_r.ToString());
            return max_r;
        }
    }
    
    // CalculateShanten : Calculate shanten count
    // 0 means tenpai, 1~ means shanten, -1 should means agari, but it's not sure
    // Based on http://ara.moo.jp/mjhmr/shanten.htm
    int CalculateShanten(int[] hand)
    {
        int r = 0;
        /// Card value
        // 0~8:1m~9m, 9~17:1p~9p, 18~26:1s~9s, 27~33:tou, nan, sha, be, shiro, hatsu, chu

        List<int> t_ = new List<int>();

        /// Check Chitoitsu
        short chi_count = 0;
        for (int i = 0; i < hand.Length - 1; i++)
            if (hand[i] == hand[i + 1])
            {
                chi_count++; i++;
            }
        r = 6 - chi_count;

        /// Check Kokushi
        List<int> k_ = new List<int>();
        short k_head = 0;
        for (int i = 0; i < hand.Length; i++)
            if (hand[i] > 26 || hand[i] % 9 == 0 || hand[i] % 9 == 8) { 
                if (!k_.Contains(hand[i]))
                {
                    k_.Add(hand[i]); 
                }
                if (i < hand.Length - 1 && hand[i] == hand[i + 1])
                {
                    k_head = 1;
                }
            }
        int r_k = 13 - k_.Count - k_head;
        if (r > r_k)
            r = r_k;

        /// Check Otherwise
        /// This is quite complicated
        /// Basically, 8 - 2 * [body] - [body candidate]
        // I know this is dumb method, but it works fine so... lol
        List<int> m_ = new List<int>(); // man
        List<int> p_ = new List<int>(); // pin
        List<int> s_ = new List<int>(); // sou
       int[] y_ = new int[7]{0,0,0,0,0,0,0};
        
        for (int i = 0; i < hand.Length; i++)
        {
            if (hand[i] <= 8) // man
                m_.Add(hand[i]);
            else if (hand[i] <= 17) // pin
                p_.Add(hand[i] - 9);
            else if (hand[i] <= 26) // sou
                s_.Add(hand[i] - 18);
            else if (hand[i] <= 33) // mozi
                y_[hand[i] - 27]++;
        }
        // Sort each lists
        m_.Sort(); p_.Sort(); s_.Sort();

        // Handle body shanten
        int r_n = 8 - GetMaxBody(m_) - GetMaxBody(p_) - GetMaxBody(s_);

        // Handling mozi cards, which can only handled as 3 same cards or head
        for (int i = 0; i < 7; i++)
            if (y_[i] == 2)
                r_n -= 1; // Body Candidate
            else if (y_[i] == 3 || y_[i] == 4)
                r_n -= 2; // Body
        Debug.Log(r_n);
        // Applying the shanten
        if (r > r_n)
            r = r_n;
        return r;
    }

    void Start()
    {
        init_hash();
        Debug.Log("Start");
        shanten = CalculateShanten(hand);
        Debug.Log("Finish!");
    }

}
