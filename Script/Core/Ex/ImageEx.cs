using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class ImageEx 
{
    public static bool SetFillAmount(this Image image, float v, float speed)
    {
        if (image.fillAmount > v)
        {
            var temp = image.fillAmount - GameTime.deltaTime * speed;
            if (temp < v)
            {
                temp = v;
            }
            image.fillAmount = temp;

        }
        else if (image.fillAmount < v)
        {
            var temp = image.fillAmount + GameTime.deltaTime * speed;
            if (temp > v)
            {
                temp = v;
            }
            image.fillAmount = temp;
        }
        return image.fillAmount == v;

    }
}
