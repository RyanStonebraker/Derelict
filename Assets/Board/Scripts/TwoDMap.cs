//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class TwoDMap : MonoBehaviour {

//    void Start()
//    {
//        Renderer rend = GetComponent<Renderer>();

//        // duplicate the original texture and assign to the material
//        Texture2D texture = Instantiate(rend.material.mainTexture) as Texture2D;
//        rend.material.mainTexture = texture;

//        // colors used to tint the first 3 mip levels
//        Color[] colors = new Color[3];
//        colors[0] = Color.red;
//        colors[1] = Color.green;
//        colors[2] = Color.blue;
//        int mipCount = Mathf.Min(3, texture.mipmapCount);

//        // tint each mip level
//        for (int mip = 0; mip < mipCount; ++mip)
//        {
//            Color[] cols = texture.GetPixels(mip);
//            for (int i = 0; i < cols.Length; ++i)
//            {
//                cols[i] = Color.Lerp(cols[i], colors[mip], 0.33f);
//            }
//            texture.SetPixels(cols, mip);
//        }
//        // actually apply all SetPixels, don't recalculate mip levels
//        texture.Apply(true);
//    }

//    // Update is called once per frame
//    void Update () {
		
//	}
//}
