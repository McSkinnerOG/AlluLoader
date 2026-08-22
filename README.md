# AlluLoader
AlluLoader is a .NET mod loader for **Allumeria**. 
It launches the game with a .NET startup hook, applies runtime patches through [Harmony](https://github.com/pardeike/Harmony), and loads a mod assembly that implements a small public API.

> [!WARNING]
> AlluLoader modifies the game process at runtime. Back up your saves before testing mods, and only install mods from authors you trust.

> [!WARNING]
> AlluLoader does NOT use the games exposed mod-loader entrypoint! It is HIGHLY likely this will cause issues with other mods.

## Requirements
- A compatible installation of **Allumeria** whih is currently the **DEMO** version on steam.
- The **.NET10** runtime
- [Harmony](https://github.com/pardeike/Harmony) / `0Harmony.dll`
- **Visual Studio 2026** to build the projects.

## Features
- Light-weight mod-loading
- Harmony patching
- Cross platform mod file loading **`Currently The AlluLauncher is windows only though.`**

## Installation
Place the `AlluLoader.exe` and `AlluLoader.dll` beside `Allumeria.exe`. Put the `AlluLoader.dll` and `0Harmony.dll` in `AlluLoader/Libraries`.

## API Development Status
- [x] Player events
- [x] Chat events
- [x] Inventory events
- [ ] Entity events
- [ ] World events
- [ ] Asset Hooks

## Authors
- [@McSkinnerOG](https://www.github.com/McSkinnerOG)


## Contributing
Contributions are always welcome!
Feel free to make a pull request or open an issue for bug reports.

## Disclaimer

AlluLoader is an unofficial community project and is not affiliated with or endorsed by the developers or publishers of Allumeria. 
