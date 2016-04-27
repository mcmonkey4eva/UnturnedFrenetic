Complaints
----------

This is a list of things we need to get fixed in Unturned itself to allow full functionality!

- Zombies can't be spawned after the first load-time sending.
    - This is due to the clientside variable "ZombieRegion.isNetworked", which is trivial to remove without trouble.
	- Only workaround is to ask clients to reconnect after spawning a zombie.
- World Objects can't be spawned after the first load-time sending.
    - This is due to the clientside variable "ObjectRegion.isNetworked", which is trivial to remove without trouble.
	- Only workaround is to ask clients to reconnect after spawning a world object.

# Minor

This is a list of things that are minor in scope and can be ignored, but we still wonder about or would like to see a fix for.

- A few things appear to be duplicated in the asset list.
    - This can be seen with the 'minor' warnings at startup of a Frenetic-enabled server.

# Game Concepts

This is a list of things that involve more general changes to the game for a boost in our abilities, but aren't entirely required.

- More physics usage.
    - It would be nice to see things such as items moving around, both client and server side.
- More visibility to in-game chat.
    - The chat in-game is squished into the top corner and isn't very visible or easy to deal with.
    - This can be best improved by moving it to a lower position and widening it.
    - Also, adding features such as pressing the up-arrow to get back the last message typed would be useful.
