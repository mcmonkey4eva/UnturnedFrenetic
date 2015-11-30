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
