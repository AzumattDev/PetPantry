# PetPantry

![](https://github.com/AzumattDev/PetPantry/blob/master/Thunderstore/icon.png?raw=true)

## The **PetPantry** mod for Valheim automatically feeds tamed animals from nearby containers, ensuring your pets stay happy and healthy without manual intervention. This mod is optimized for performance, reducing the overhead typically associated with continuous checks for nearby containers and food items.

`Version checks with itself. If installed on the server, it will kick clients who do not have it installed.`

`This mod uses ServerSync, if installed on the server and all clients, it will sync all configs to client`

`This mod uses a file watcher. If the configuration file is not changed with BepInEx Configuration manager, but changed in the file directly on the server, upon file save, it will sync the changes to all clients.`


---

Made this quickly since people asked for it and most of the other alternatives are either broken or have issues.

# PetPantry Mod

## Benefits

### Compared to Counterparts that are usually a named variation of "AutoFeed" or reuploads of Aedenthorn's AutoFeed mod, PetPantry offers the following advantages:

- **Event-Driven Feeding**: Unlike other mods that constantly check for food, PetPantry only checks when an animal
  becomes hungry.
- **Efficient Container Management**: Registers and unregisters containers dynamically, avoiding unnecessary checks on
  non-food containers.
- **Cooldown Mechanism**: Introduces a cooldown for feed checks to prevent frequent and redundant operations.

### Performance:

- **Reduced Overhead**: By leveraging a cooldown mechanism and event-driven checks, the mod minimizes the frequency of
  potentially expensive operations.
- **Optimized Filtering**: Efficiently filters and processes containers and items, ensuring only relevant data is
  handled.
- **Caching Mechanism**: Utilizes caching to store last feed check times, preventing repeated processing for the same
  animal within short intervals.

## How It Works

### Key Mechanisms:

1. **Container Registration**: Containers are registered when they are created and unregistered when destroyed. This
   ensures the list of containers is always up-to-date.
2. **Hunger Check**: The `IsHungry` method is patched to trigger a food search only when an animal becomes hungry,
   ensuring checks are made only when necessary.
3. **Food Search and Feeding**: When an animal is hungry, the mod searches nearby registered containers for suitable
   food and feeds the animal if found.

## Configuration

ContainerRange: Defines the range in meters within which containers are checked for food items.

RequireOnlyFood: Ensures only containers with acceptable food items are considered for feeding.

FeedCheckCooldown: Cooldown in seconds between feed checks. Checks only occur when the pet is hungry.

Objectively shite code here: [PetPantry on GitHub](https://github.com/AzumattDev/PetPantry)

<details>
<summary><b>Installation Instructions</b></summary>

***You must have BepInEx installed correctly! I can not stress this enough.***

### Manual Installation

`Note: (Manual installation is likely how you have to do this on a server, make sure BepInEx is installed on the server correctly)`

1. **Download the latest release of BepInEx.**
2. **Extract the contents of the zip file to your game's root folder.**
3. **Download the latest release of PetPantry from Thunderstore.io.**
4. **Extract the contents of the zip file to the `BepInEx/plugins` folder.**
5. **Launch the game.**

### Installation through r2modman or Thunderstore Mod Manager

1. **Install [r2modman](https://valheim.thunderstore.io/package/ebkr/r2modman/)
   or [Thunderstore Mod Manager](https://www.overwolf.com/app/Thunderstore-Thunderstore_Mod_Manager).**

   > For r2modman, you can also install it through the Thunderstore site.
   ![](https://i.imgur.com/s4X4rEs.png "r2modman Download")

   > For Thunderstore Mod Manager, you can also install it through the Overwolf app store
   ![](https://i.imgur.com/HQLZFp4.png "Thunderstore Mod Manager Download")
2. **Open the Mod Manager and search for "PetPantry" under the Online
   tab. `Note: You can also search for "Azumatt" to find all my mods.`**

   `The image below shows VikingShip as an example, but it was easier to reuse the image.`

   ![](https://i.imgur.com/5CR5XKu.png)

3. **Click the Download button to install the mod.**
4. **Launch the game.**

</details>

<br>
<br>

`Feel free to reach out to me on discord if you need manual download assistance.`

# Author Information

### Azumatt

`DISCORD:` Azumatt#2625

`STEAM:` https://steamcommunity.com/id/azumatt/

For Questions or Comments, find me in the Odin Plus Team Discord or in mine:

[![https://i.imgur.com/XXP6HCU.png](https://i.imgur.com/XXP6HCU.png)](https://discord.gg/qhr2dWNEYq)
<a href="https://discord.gg/pdHgy6Bsng"><img src="https://i.imgur.com/Xlcbmm9.png" href="https://discord.gg/pdHgy6Bsng" width="175" height="175"></a>