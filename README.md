# D2RMng

A small Windows tool for launching multiple Diablo II: Resurrected clients at once, each logged into a different Battle.net account, without having to click through the Battle.net launcher and D2R login screen for every single one.

## Features

- Manage a list of "instances" (name, Battle.net account, password, path to `D2R.exe`, region, extractor flag)
- Start a single instance, or mass-restart all of them with one click
- Save/load the instance list to/from a `.txt` file
- Kill all running `D2R.exe` processes
- Auto Kill Handle: periodically frees D2R's single-instance lock so multiple copies can run side by side

## How it works

Diablo II: Resurrected normally only allows one running instance and expects login through the Battle.net launcher. D2RMng works around both:

- It uses D2R's own (undocumented) launch parameters `-username`, `-password` and `-address` to log a client straight into a Battle.net account, bypassing the launcher app.
- It uses Sysinternals' `handle.exe`/`handle64.exe` to find and close the `DiabloII Check For Other Instances` handle that D2R holds, which is what normally prevents a second copy from starting.

## Requirements

- Windows
- .NET Framework 4.8 (already on most Windows 10/11 installs)
- [Sysinternals `handle.exe`/`handle64.exe`](https://learn.microsoft.com/en-us/sysinternals/downloads/handle), available as `handle` on your `PATH`, with its EULA already accepted (run `handle -accepteula` once from a terminal before using the app)
- Administrator rights — the app elevates itself (UAC prompt) to run `D2R.exe` and `handle.exe`

## Building

1. Install [Visual Studio Community](https://visualstudio.microsoft.com/) (free) with the **.NET desktop development** workload.
2. Open `D2RMng.sln`.
3. Press ▶ Start (or Build → Build Solution). The `.exe` ends up in `D2RMng/bin/Debug/` (or `bin/Release/` for a release build).

Command line alternative (with Visual Studio's MSBuild installed):

```
msbuild D2RMng.sln /p:Configuration=Release
```

## Usage

1. Click **New** to add an instance row, fill in name, Battle.net email, password, path to your `D2R.exe`, and region.
2. Click **Encrypt PW** to store the password (obfuscated, see note below) instead of plain text.
3. Click **Start** on a row to launch that instance, or **MassRe** to kill everything and relaunch all instances at once.
4. **Save as** / **Load** persist the current instance list to a `.txt` file so you don't have to re-enter accounts every time.
5. Enable **Auto Kill Handle** to keep the single-instance lock cleared automatically instead of clicking **Kill Handle** by hand.

## Known limitations

- **Battle.net password is capped at 20 characters.** D2R's `-password` launch parameter silently truncates at 23 characters, which causes a failed, hard-to-diagnose "can't authenticate" login. The app rejects longer passwords up front with a clear error instead. If your account's password is longer, shorten it in your Battle.net account settings.
- The stored password is only obfuscated (AES with a key hardcoded in the source), not securely encrypted — good enough to avoid shoulder-surfing a saved file, not a real secret store.
- Multiboxing may be against Blizzard's Battle.net/D2R Terms of Service — use at your own risk.
