using System;

[Serializable]
public struct SizeInt
{
    public int width;
    public int height;

    public SizeInt(int width = 1, int height = 1)
    {
        this.width = width;
        this.height = height;
    }
}