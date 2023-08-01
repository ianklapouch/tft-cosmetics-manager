<h3 align="center">
	<img src="https://raw.githubusercontent.com/ianklapouch/tft-cosmetics-manager/main/Assets/icon_256.png" width="100" alt="Logo"/><br/>
	TFT Cosmetics Manager
</h3>

TFT Cosmetics Manager is a .NET application developed using WPF, which allows you to create and manage profiles based on your Teamfight Tactics cosmetics. You can quickly switch between different profiles, select random cosmetics from your collection, and more.

This project utilizes [LCU](https://hextechdocs.dev/getting-started-with-the-lcu-api/) to read your cosmetics and select them in the client, and [CDragon](https://www.communitydragon.org/) to handle the images.

# Setup
Download the latest version from [Releases](https://github.com/ianklapouch/tft-cosmetics-manager/releases) and run the executable. The application is available for Windows only.

# How to use
## Creating a profile
- On the home screen, click ADD ITEM.
- In the create profile screen, give your profile a name, select a Little Legend, an Arena, a Boom, and click SAVE. You can click CANCEL to discard the changes.
- The created profile will be added to the list of profiles on the home page.

## Editing a profile
- On the main page, click the edit icon of the profile you want to change.

## Deleting a profile
- After creating a profile, you can delete it by clicking the trash can icon in the top right corner of the profile.

## Setting random cosmetics
- On the main screen, click the **RANDOMIZE** button to set a random combination of Little Legend, Arena, and Boom in your client.

## Creating favorites
You can create favorites with two different rules: White List and Black List.
### White List
- On the main screen, click the FAVORITES button to access the favorites feature.
- In the Favorites screen, select the "White List" option.
- Mark the Little Legends, Arenas, and Booms that you want to include in the randomization function. If no items are marked in the White List, all items will be considered for randomization.
### Black List
- On the main screen, click the FAVORITES button to access the favorites feature.
- In the Favorites screen, select the "Black List" tab.
- Mark the Little Legends, Arenas, and Booms that you want to exclude from the randomization function. If all items are marked in the Black List, none of them will be considered for randomization.

By using these White List and Black List rules, you can have more control over the cosmetics that will be chosen during the randomization process.

## Acknowledgments
I would like to express my heartfelt appreciation to [@dontdoitadam](https://www.twitch.tv/dontdoitadam) for their contribution to the TFT Cosmetics Manager project. He created the amazing project logo that you can see at the top of this page. Their artistic skill and dedication helped bring the application's visual identity to life.

## Conclusion
TFT Cosmetics Manager is an open-source project developed for educational purposes, as a portfolio project, and to contribute something useful to the community. If you find this project helpful and want to support it, you can show your appreciation by [buying me a coffee](https://www.buymeacoffee.com/ianklapouch), following my [GitHub](https://github.com/ianklapouch) profile, and giving this repository a star to increase its visibility.