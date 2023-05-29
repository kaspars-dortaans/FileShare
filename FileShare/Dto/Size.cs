﻿namespace FileShare.Dto;

public class Size
{
    public int Width { get; set; }
    public int Height { get; set; }

    public Size() { }

    public Size(string size)
    {
        var split = size.Split('*');
        if(split.Length == 2)
        {
            Width = int.Parse(split[0].Trim());
            Height = int.Parse(split[1].Trim());
        }
    }
}
