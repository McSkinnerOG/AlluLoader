# AlluLoader

> [!CAUTION]
> This repository allows developers to implement mods with an event-based framework. Please note that this is not recommended for all use cases, particularly for mods running on a client (someone's laptop). This framework may work better for plugins intended for dedicated servers. However, there is no official dedicated server software as of now.

## Instructions

1. Get the video game Allumeria. [It's available on Steam.](https://store.steampowered.com/app/3516590/Allumeria/)

1. Download and install .NET 10.0 from [the official website](https://dotnet.microsoft.com/en-us/download/dotnet/10.0)

1.  Run ``git clone`` to copy this repository, or use the download button on GitHub.

1. Go into the folder (directory) that has ``Allumeria.exe``. You can find this by opening up Steam > View Game Library > Click on Allumeria > Click on the settings gear icon > Manage > Browse local files.

1. Copy ``Allumeria.dll`` and ``OpenTK.Mathematics.dll`` and paste both of them into the ``Dependencies`` folder of this repository.

1. Go into the folder where this README lives and run ``dotnet build``.

1. Go back into the folder that has ``Allumeria.exe``. Create a new folder called ``mods``. In this new folder, paste some of the ``.dll`` files you created from the previous step: ``0Harmony.dll`` and ``Loader.dll``.

1. Launch Allumeria and then close it (LOL)

1. If you look in the ``mods`` folder, you should see some new folders: ``Config``, ``Mods``, ``Libraries``, and ``Logs``. You should also see an ``aluloader.log`` file that can help with troubleshooting. If you don't see the new folders, then something went wrong.

1. Copy and paste ``ExampleMod.dll`` into the ``mods/Mods`` folder.

1. Launch Allumeria again and enjoy!

## Authors
- [@McSkinnerOG](https://www.github.com/McSkinnerOG)
- [@rationing](https://github.com/rationing)

## Contributions
Please create [pull requests](https://github.com/McSkinnerOG/AlluLoader/pulls) and [issues](https://github.com/McSkinnerOG/AlluLoader/issues) for suggestions, bug reports, and feedback. Make sure to read other pull requests and issues to prevent duplicates.

## Disclaimer
AlluLoader is an unofficial community project. It is not affiliated with or endorsed by the developer of Allumeria. 