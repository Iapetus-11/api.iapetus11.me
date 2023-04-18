# api.iapetus11.me
- A general purpose API designed to be used by any of my miscellaneous projects
- [Originally written in JavaScript using Express.js](https://github.com/Iapetus-11/api.iapetus11.me/tree/4c746d8bc9f6adc993b6dc54fc0a564b92512f73) 

## Technologies
- [Asp.Net](https://dotnet.microsoft.com/en-us/apps/aspnet)
- [Entity Framework](https://docs.microsoft.com/en-us/ef/)
- [ImageSharp](https://sixlabors.com/products/imagesharp/)
- [Serilog](https://serilog.net/)

## Features
- Image generation
- Minecraft Java & Bedrock server status
- Reddit meme endpoint
- GitHub stat shields/cards/badges

## Documentation
### Fractals
- `GET /fractal` - *Returns a png of the generated fractal*
    - `resolution` - *The resolution of the returned image, any number between `512` and `2048`*
    - `fractalVariation` - *The variation of flame fractal, valid values are: `Linear`, `Sine`, `Spherical`, `Horseshoe`, `Cross`, `Tangent`, `Bubble`, `RadTan`, `Tangle`, and `Diamond`*
    - `colorA` - *The first color, in hex format*
    - `colorB` - *The second color, in hex format*
    - `coloring` - *The coloring strategy, valid values are: `Gradient`, `SigmoidGradient`, and `Experimental`*
    - `iterTransformX` - *Iterative X-axis transformation, any value between `0.0` and `5.0`*
    - `iterTransformY` - *Iterative Y-axis tranformation, any value between `0.0` and `5.0`*
    - `xShift` - *How much to shift the X axis by, any value between `-1.0` and `1.0`*
    - `transform` - *Iterative transformation, any value between `0.0` and `5.0`*
    - `iterations` - *How many iterations, any value between `1` and `5000000`*
    - `mirrored` - *Whether or not to mirror the generated fractal, either `true` or `false`*
    - `blur` - *How much to blur the image, any value between `1.0` and `4.0`*
    - `sharpen` - *How much to sharpen the image, any value between `1.0` and `4.0`*
- All generated fractal images are licensed under a [Creative Commons Attribution-ShareAlike 4.0 International License](https://creativecommons.org/licenses/by-sa/4.0/)

### GitHub Stats
- `GET github/stats/{userName}` - *Returns a GitHub user's earned stars, opened issues, and opened PRs which were merged*
  - `userName` - *Must be a valid GitHub username*
- `GET github/stats/{userName}/shield/stars` - *Returns an image/shield/badge showing a user's earned stars*
  - `userName` - *Must be a valid GitHub username*
  - [shields.io](https://shields.io/) is used for image generation, this endpoint supports [these](https://shields.io/#your-badge) query parameters.
- `GET github/stats/{userName}/shield/prs` - *Returns an image/shield/badge showing a user's opened and then merged pull requests*
  - `userName` - *Must be a valid GitHub username*
  - [shields.io](https://shields.io/) is used for image generation, this endpoint supports [these](https://shields.io/#your-badge) query parameters.
- `GET github/stats/{userName}/shield/issues` - *Returns an image/shield/badge showing a user's opened issues*
  - `userName` - *Must be a valid GitHub username*
  - [shields.io](https://shields.io/) is used for image generation, this endpoint supports [these](https://shields.io/#your-badge) query parameters.
- `GET github/stats/{userName}/shield/dependants` - *Returns an image/shield/badge showing the number of repositories which have one of the user's repositories as a dependency*
  - `userName` - *Must be a valid GitHub username*
  - [shields.io](https://shields.io/) is used for image generation, this endpoint supports [these](https://shields.io/#your-badge) query parameters.
### Reddit Posts
- `GET reddit/{subredditGroup}?requesterId={uniquePerSomething}` - *Returns a random image post from the specified subreddit group*
  - `subredditGroup` - *Must be one of `meme`, `cursedMinecraft`, `greentext`, `comic`*
  - `requesterId` (optional) - *Unique ID to prevent duplicate posts from being returned, for example if used in a Discord bot, this could be the channel ID the command was summoned in*
### Minecraft Servers
- `GET mc/server/status/{serverAddress}` - *Returns the status of the specified Minecraft server, this supports all versions of Minecraft except those below 1.8 and realms*
  - `serverAddress` - *Any host, port is optional, an example would be `example.com:25565`*
- `GET mc/server/status/{serverAddress}/image?customName={custom name}` - *Returns an image showing the status of the specified Minecraft server, this supports all version of Minecraft except those below 1.8 and realms*
  - `serverAddress` - *Any host, port is optional, an example would be `example.com:25565`*
  - `customName` (optional) - *Custom name to display on the image instead of the specified server address*
- `GET mc/server/status/{serverAddress}/image/favicon` - *Returns the server's favicon / server icon, supports all Java Edition versions of Minecraft except those below 1.8 and realms*
  - `serverAddress` - *Any host, port is optional, an example would be `example.com:25565`*
### Minecraft Images
- `GET mc/image/achievement/{text}` - *Returns an image saying "Achievement Get" and the specified text*
  - `text` - *Any text, must be within 1-30 characters*
- `GET mc/image/splash/{text}` - *Returns an image of the splash screen with the specified text displayed on it*
  - `text` - *Any text, must be within 1-30 characters*
