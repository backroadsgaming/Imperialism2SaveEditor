# Imperialism II Savegame Editor

**An unofficial Windows savegame editor for Imperialism II.**

Developed by **BackRoads Gaming Studio**.

This free utility allows players to modify selected values in their Imperialism II saved games without manually editing hexadecimal data.

## Features

- **Treasury Editing** — View and modify your nation's treasury.
- **Warehouse Resource Editing** — View and modify 23 different warehouse resources.
- **Nation Detection** — Automatically identifies the player's nation in supported savegames.
- **Automatic Backups** — Creates a backup before saving modifications.
- **Savegame Integrity Protection** — Checks for external file changes before writing and preserves the original file length.
- **Simple Windows Interface** — Built with C# and Windows Forms.

## Requirements

- Windows
- Imperialism II
- A valid `.imp` savegame file

The editor is developed using **C# and .NET 10**.

## How to Use

1. Close Imperialism II completely.
2. Launch the Imperialism II Savegame Editor.
3. Open the `.imp` savegame you want to modify.
4. Change the desired treasury amount or warehouse resource quantities.
5. Click the appropriate **Apply** button.
6. The editor automatically creates a backup before saving.
7. Launch Imperialism II and load the modified savegame.

**Important:** Always close the game before editing savegames. Keep your original savegames and backups until you have confirmed the modified save loads correctly.

## Current Limitations

- Warehouse resource quantities are currently limited to **0–255**.
- The editor modifies the detected player's treasury and warehouse resources.
- Not all savegame configurations have been tested.
- Editing unsupported or unexpected savegame structures may cause errors or corrupted saves.

This is an early beta release. Please report any problems through GitHub Issues.

## Download

Precompiled Windows versions will be available in the [Releases](https://github.com/backroadsgaming/Imperialism2SaveEditor/releases) section once the first release is published.

## Development

This project is developed in **Visual Studio 2026** using **C# / .NET 10 Windows Forms**.

Developers can clone the repository and open the included Visual Studio solution.

## Disclaimer

This is an unofficial fan-created utility and is not affiliated with, endorsed by, or sponsored by the creators, publishers, or current rights holders of Imperialism II.

Imperialism II and its associated trademarks and intellectual property belong to their respective owners.

The editor does not include the original game or its proprietary files.

### Windows Security Notice

**Imperialism II Savegame Editor is currently an unsigned application.**

When downloading or launching the editor, Windows Defender SmartScreen may display a warning stating that the publisher cannot be verified or that Windows protected your PC.

This warning may appear because the application has not been digitally signed with a trusted code-signing certificate.

The editor is provided free of charge as an unofficial fan project. You can review its source code in this GitHub repository.

**For your security:**
- Download the editor only from the official GitHub Releases page.
- Verify that the downloaded file comes from this repository.
- Scan the downloaded file with your antivirus software before running it.
- Only proceed past a Windows security warning if you trust the application and its source.

A SmartScreen warning does not automatically indicate malicious software, but an unsigned application is not automatically safe either.

## Credits

**BackRoads Gaming Studio**

Created for the Imperialism II player community.
