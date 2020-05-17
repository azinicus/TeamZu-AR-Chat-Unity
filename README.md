# TeamZu

![TeamZu Chat App on Multiple Platforms](https://take.ms/xBEvn)

TeamZu is a concept app built in Unity that aims to leverage AR foundation and Photon Networking to create a team chat room with AR avatars.

> Note: the master commit currently does not have AR avatars but simply basic 2D images. The AR avatar functionality is in development, but I will likely create a new feature branch with this functionality when I'm able to make some progress.

### Installation

1. Download Unity 2019.3.13f1 - The version of [Unity](https://nodejs.org/) that I'm using is v 2019.3.13f1. It's highly recommended that you use the same version.

2. Get Photon Voice 2: You will also need the following assets from the Asset Store or Package Manager:

| Asset | Link | Notes |
| ------ | ------ | ------ |
| Photon Voice 2 (2.16.1) | https://assetstore.unity.com/packages/tools/audio/photon-voice-2-130518 | This is a free asset up to 20 simultaneous users |

3. Create free Photon account: Sign up for a free account with [Photon Networking](https://www.photonengine.com/) 
4. Create PUN apps: In Photon's Dashboard 'create a new app' for Photon Realtime and another for Photon Voice. You'll need the AppId for each of these in Unity.
5. PUN Wizard: Once you have imported the Photon Voice 2 asset into Unity, it should run you through the PUN wizard where it asks you for the Photon Voice App ID... insert the appId from your Photon Realtime app that you created.
6. Configure Photon Voice: Now go to Window > Photon Unity Networking > Highlight Server Settings. Then insert the App Id for Photon Voice that you created into the 'App Id Voice' field.

### Running
You should be able to run the app in editor, build for Android, iOS, or Mac/PC. This demo makes users join the same default room. So if you run the app any time, it should simply connect you to that room. The limit based on Photon's free tier is set to 20 simultaneous users.

### Todos
- Add ability to create a room instead of simply joining the same room by default 
- Add branch with AR avatar progress + make the AR faces actually networked
- Allow for individuals to talk directly to one another instead of the whole group