Unturned Frenetic
-----------------

A mod for Unturned; adds a full high-power script engine to allow dynamic modding!

# Building notes

- 1. You will need a release build of FreneticUnturned to use as the basis for the injector's first run. (After this, you can use a locally compiled copy.)
- 2. You can invert note 1 if you only need to build the mod itself and not the injector. (Use a release copy of the injection patched result as the basis for building the mod itself.)
- 3. Run the injector via a command line so you can see the output.
- 4. The two sub-projects rely on each other and each cannot function without the other (this causes note 1).
- 5. This project heavily uses complicated internal code... if you can't figure out how to compile it from what's available in this repository, you probably shouldn't be editing the code.

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

Copyright (c) 2015 Frenetic Team

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
