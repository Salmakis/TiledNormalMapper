# TiledNormalMapper
Quick &amp; dirty tool to convert tilesets with heightmap information into a tileset with normal map information

## What is it?

What this tool does is grabbing each tile of a tilemap/chipset/tileatlas and converts it, and set it back togheter after the progress.
it also adds a "mirrior edge" around each tile to prevent blur edges.

this image explains what exacly it does:

![What it does](https://github.com/Salmakis/TiledNormalMapper/blob/master/readme.png)

## how to use
dotnet run -- <path> <tilewidth> <tileheight>  
expample:  
dotnet run -- tiles.png 16 16  
this would create a tiles_normal.png and assumes that a tile is 16x16 pixels in size

## credits

the conversion from the heighmap to normal maps is done by using Craig-s-Utility.
used via nuget:
https://github.com/JaCraig/Craig-s-Utility-Library
