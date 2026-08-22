# AlluLoader

1. Get the video game Allumeria. [It's available on Steam.](https://store.steampowered.com/app/3516590/Allumeria/)

1. Download and install .NET 10.0 from [the official website](https://dotnet.microsoft.com/en-us/download/dotnet/10.0)

1.  Run ``git clone`` to copy this repository, or use the download button on GitHub.

1. Go into the folder (directory) that has ``Allumeria.exe``.

1. Copy ``Allumeria.dll`` and ``OpenTK.Mathematics.dll`` and paste both of them into the ``Dependencies`` folder of this repository.

1. Go into the folder where this README lives and run ``dotnet build``.

1. Go back into the folder that has ``Allumeria.exe``. Create a new folder called ``mods``. In this new folder, paste the two ``.dll`` files you created from the previous step:  ``Loader.dll`` and ``ExampleMod.dll``.

1. Launch Allumeria and have fun!
