# Examples
The level.dat is a gzip compressed nbt file, so to read it with the NBTReader you need to decompress it beforehand. A decompress method is given in Program.cs.

The DecompressedChunk0.0Flatworld.nbt is a chunk file extracted from a Minecraft world, pre-decompressed, so you can just directly pass it into the NBTReader.