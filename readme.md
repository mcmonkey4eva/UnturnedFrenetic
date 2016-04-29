Unturned Frenetic
-----------------

A mod for Unturned; adds a full high-power script engine to allow dynamic modding!

# Building

- 1. Build UnturnedFreneticInjector using Visual Studio 2015.
- 2. include in its directory the base Unturned files (Assembly-CSharp, and Assembly-CSharp-firstpass).
- 3. Run the injector via a command line.
- 4. Compile UnturnedFrenetic against the files you now have, also using Visual Studio 2015.
- 5. Use this new file to re-run the injector with.
- 6. Copy the resultant patched file and replace the original Assembly file with it, in your Unturned directory.
- 7. Launch your server :)

# Warnings

- Your local client may have trouble playing on other servers until you revert Assembly-CSharp.dll to its original form. Make sure you are running off a server box!
- Avast (and possibly other AV's) don't currently seem to like the injector much, and may freeze up. Disable them before running the injector!

### Licensing pre-note:

This is an open source project, provided entirely freely, for everyone to use and contribute to.

If you make any changes that could benefit the community as a whole, please contribute upstream.

### The short of the license is:

You can do basically whatever you want, except you may not hold any developer liable for what you do with the software.

### The long version of the license follows:

The MIT License (MIT)

Copyright (c) 2015-2016 Frenetic XYZ

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
