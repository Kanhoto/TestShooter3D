# TestShooter3D

![Preview of the project](https://i.postimg.cc/qqgTMhvc/Capture-d-cran-du-2022-07-06-20-45-47.png)

## Current version

### Player controls : ❌ (don't work)

| Key | Action |
|--|--|
| z | Forward |
| q | Left |
| s | Back |
| d | Right |
| left mouse | Shoot |
| space | Jump |

Rotation on axis are not locked and I'm still searching why.

### Bullets : ❌ (don't work)

Bullets are just sphere mesh with a rigidbody, I make them spawn at a spawn point placed in front of the gun and I give them some inertia.
Currently, they spawn but they don't have any power, they just drop in front of the player.

## Next steps

 - Repair the motion axis lock
 - Trying to shoot some bullets
 - add some more type of bullets (curved trajectory type, auto aim type, ...)
 - add a shield system (front shield, sphere shield, ...)
 - add life system (no mana/ammo since bullet will be shooted with life)
 - add swords
 - add walk on wall
 - add multiple jump (cost energy)
 - add multiplayer
