# TiledNormalMapper
Quick &amp; dirty tool to convert tilesets with heightmap information into a tileset with normal map information

What this tool does is grabbing each tile of a tilemap/chipset/tileatlas and converts it, and set it back togheter after the progress.
it also adds a "mirrior edge" around each tile to prevent blur edges.

this image explains what exacly it does:

![What it does](https://github.com/Salmakis/TiledNormalMapper/blob/master/readme.png)

the conversion from the heighmap to normal maps is done by using Craig-s-Utility library:
https://github.com/JaCraig/Craig-s-Utility-Library
