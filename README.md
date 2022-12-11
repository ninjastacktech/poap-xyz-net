
![POAP f74a7300](https://user-images.githubusercontent.com/3955285/206902603-88ef3166-122e-4453-aee0-fd9ea492955a.svg)


---
[![NuGet](https://img.shields.io/nuget/v/Poap)](https://www.nuget.org/packages/Poap/) 
[![GitHub](https://img.shields.io/github/license/ninjastacktech/poap-xyz-net)](https://github.com/ninjastacktech/poap-xyz-net/blob/master/LICENSE)

.NET 6 C# SDK for poap.xyz

The API docs can be found here: https://documentation.poap.tech/reference

# install
```xml
PM> Install-Package Poap
```
# snippets

### Get token details:
```C#
var options = new PoapClientOptions
{
    ApiKey = "<api_key>",
    ClientId = "<client_id>",
    ClientSecret = "<client_secret>",
    Audience = "<audience>",
};

var client = new PoapClient(options);

var token = await client.GetTokenAsync("<token_id>");

```


---

### MIT License


