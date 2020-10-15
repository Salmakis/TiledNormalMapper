# TiledNormalMapper
Quick &amp; dirty tool in net.core 3.1 to convert tilesets with heightmap information into a tileset with normal map information

## What is it?

What this tool does is grabbing each tile of a tilemap/chipset/tileatlas and converts it, and set it back togheter after the progress.
it also adds a "mirrior edge" around each tile to prevent blur edges.

this image explains what exacly it does:

![What it does](https://github.com/Salmakis/TiledNormalMapper/blob/master/readme.png)

## how to use
dotnet run -- path tilewidth tileheight
expample:  
dotnet run -- tiles.png 16 16  
this would create a tiles_normal.png and assumes that a tile is 16x16 pixels in size

## credits

the code conversion from the heighmap to normal maps was originated here:  
https://gamedev.stackexchange.com/questions/106703/create-a-normal-map-using-a-script-unity
i just changed it to be usabele with normal .net Windows.System.Drawing instead of unity3d stuff.
