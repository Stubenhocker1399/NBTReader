# Examples
The level.dat is a gzip compressed nbt file, so to read it with the NBTReader you need to decompress it with the /c argument.

The DecompressedChunk0.0Flatworld.nbt is a chunk file extracted from a Minecraft world, pre-decompressed, so you can just directly pass it into the NBTReader wit the /d argument.

bigtest.nbt is a test file from Notch for the nbt format, compressed, so you need the /c argument.
