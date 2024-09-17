/**
Names: L Kipling (20899932), R Mackintosh (21171466), M Pontague (19126924)
Date: 17 September 2024
Class: musicplayer
Purpose: Implements a singleton class for playing music in the application using the `MediaElement` component. It manages the loading, playing, and stopping of media files.
Notes: The musicplayer class ensures that only one instance of the media player exists throughout the application using the singleton pattern.
*/

using System;
using System.Windows.Controls;

namespace GameClient
{
    internal class musicplayer
    {
        private static musicplayer player;
        public MediaElement media { get; private set; }

        /**
        Method: musicplayer (Constructor)
        Imports: None
        Exports: None
        Notes: Initializes a new MediaElement object with specific settings for manual control of media playback and volume settings.
        Algorithm: The constructor creates a MediaElement and configures its behaviors, such as manual load, stop behavior, and default volume.
        */
        private musicplayer()
        {
            media = new MediaElement();
            {
                media.LoadedBehavior = MediaState.Manual;
                media.UnloadedBehavior = MediaState.Stop;
                media.Volume = 1.0;
            }
        }

        /**
        Method: Instance (Singleton Instance)
        Imports: None
        Exports: musicplayer (Returns the single instance of the musicplayer class)
        Notes: Implements the singleton pattern by returning the single instance of the musicplayer class. Creates the instance if it does not exist.
        Algorithm: If the player instance is null, a new instance of the musicplayer is created. Otherwise, the existing instance is returned.
        */
        public static musicplayer Instance
        {
            get
            {
                if (player == null)
                {
                    player = new musicplayer();
                }
                return player;
            }
        }

        /**
        Method: PlayMusic
        Imports: string filePath (The path to the media file to play)
        Exports: None
        Notes: Plays the media file located at the specified file path. The file path is relative to the application's location.
        Algorithm: Sets the media source using a relative file path and then plays the media using the `Play` method of MediaElement.
        */
        public void PlayMusic(string filePath)
        {
            media.Source = new Uri(filePath, UriKind.Relative);
            media.Play();
        }

        /**
        Method: StopMusic
        Imports: None
        Exports: None
        Notes: Stops the currently playing media.
        Algorithm: Invokes the `Stop` method of the MediaElement to halt playback.
        */
        public void StopMusic()
        {
            media.Stop();
        }
    }
}
