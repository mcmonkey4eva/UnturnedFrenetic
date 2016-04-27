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
