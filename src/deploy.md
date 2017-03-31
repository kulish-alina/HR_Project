# Deploy

This simple tutorial is going to take you into the magic of deployment. We will speak about how to build an ASP.NET application, how to host it and how is all of this just possible.

Also, we will talk a little about the frontend building and placing it correctly for the backend to know about routing.

## Breaf Intro

As you know, by default all asp.net apps are hosted with iis. That sounds fine till you get into the deal with IIS.

So why I speak so? - because in that case application relies on `System.Web` and its components, that is place where lives IIS api calls. But that is not all, IIS is dependent from Windows and its syscalls, so administaring such a sing gives you a real trouble.

What is next? - with ASP.NET 4, we received new kind of application infrastructure - lets intoduce you to [OWIN](http://owin.org/) (Open Web Interface for .Net). I will not tell you a lot about OWIN itself (it would be a brilliant topic for separate article/doc).
So read it by yourself, I will only tell about some things it the OWIN topic of this article.

## Breaking Changes to project

First of all, we delete `Global.asax` config as deprecated application structure.

Secondly, the heart of OWIN would be some kind of `Startup.cs` where will put down our application settings and access pipeline. Almostly, there is no need to change your web api config, this must work as it is now.

We should tell OWIN that we use ASP.NET Web API. For such a thing is used extension methond with "telling title" `UseWebApi`.

**Attention**: build your pipeline in a style like `Use + <ActionName>`. As a result, coding brings into particularly new standart, but practically there is no revolution in such a way in its organisation.

OWIN application can run on it's own host, on iis or any other host that implements owin host standart. We are using self-host, because it is rather closer to real-world application host (and it is really lightweight).
To do this create console application with `Program.cs` and do here your stuff, or you can even modify your we api project with adding such a file and changing project configuration to Console one with new startup object.
The best of all, now we can easily change host and in a minutes you can get out of self-host application and use `katana` or smth else.

## How to build daemon

If you want to do building on practycally clear environment you should have .Net SDK installed. Most of all is need .Net Framework 4+, [MSBuild](https://msdn.microsoft.com/en-us/library/dd393574.aspx), [NuGet](https://www.nuget.org/), csc, etc.
Or you can use VisualStudio on your dev environment, just build release and copy-past results of the build to your server. In any case the last thing to start server - just run your application (some kind of executable).

The true-style build is using msbuild (.Net application builder) manually without VisualStudio (under the hood vs does the same, you only need a click).

```PowerShell
msbuild  (<your_solution>|<your_project>)
```

or you need more than just default (like in our case)

```PowerShell
msbuild  (<your_solution>|<your_project>) /t:Build /p:Configuration=Release /nologo /m /v:m
```

By the default we do not need to istall nuget packages manually, but this could be changed in the future for clearness of building.

## Building client-side part of application

Before we begin, every real-world js app has its own dependencies and modules. Every project comes to the problem of modules, it doesn't matter what you use AMD, CommonJs, ES 6 Modules, UMD and different package managers for this modules. As a result it is not really easy to build such a hell into the only application.

So modules are needed to be perfectly organised, all dependencies should be downloaded (your packages from bower, npm, etc), you should use something really cool to create bundle and do some magic with it (browserify, webpack, gulp, grunt). If you want to understand more in how to be in modules, read these articles:

1. [JS module life-cycle](https://habrahabr.ru/post/181536/)
1. [Understanding js modules](https://spring.io/understanding/javascript-modules)

The frontend consists of lots js, html and css files. And even images and other fine support docs. In old world web applications we would use all of them with specified js and css in the html headers. However, real world got out of that with SPA applications - they got their own infrastructure that is much close to the true-style building of application. So, we have the only html file as into for page (site) and one script file **bundle**.

What is bundle? - the file of all content. It consists of js, html, css, json, etc in one. It can be considered as a result of "compilation". We receive such a file due to webpack and its magic of package loading. It causes writing all the js files with their dependencies to one file (or even more, not important).

In our solution there is an npm command to start bundling - _dist-build_. And it is used to do all things fine.
Now this type of build **doesn't** support minification, optimizations and other real-world frontend unglifications.

How to use this commands:

``` PowerShell
   npm run dist-build
```

## Finish line (Hosting and Certification)

After building each part of application, we need to put all of them into one place.

Two important things to notice the setting of the url and port should be equal thought all the parts of application. The second thing - to enable access from anywhere not on the host pc your os should start listening the port and url to redirect it for server.

On Windows there is an utilite that will help us: _netsh_

```PowerShell
netsh http add urlacl url=<url> user=<userName>
```

Execept of specifying every userName we can use some reserved names, like _everyone_.
If you want to grant access to resource to a specific user just use its Domain\userName, like EDU\grym.

Furthermore, if application is deployed in https line - there is a need in the SSL certificate to be attached to application. That is done again using _netsh_. this time you need **_appid_** and **_certhash_**. Appid - is the guid created for your application, you can find it in _AssemblyInfo.cs_ file of project. Certhash is nothing else, but Thumbprint of certificate.

```PowerShell
netsh http add sslcert ippor=0.0.0.0:<port> certhash=<hash> appid="{<guid>}"
```

__Important !!!__ The certificate must be already installed on the localmachine under the __Trusted Root Certification Authorities__. If sslcert fails with _1312_ or smth like that - check you certificate, there is some problem with it. Reed  [Common errors](#what-may-cause-errors) for more info about mistakes and exceptions.

And run the executable "ApiHost.exe"

## Deploy line

1. Create deploy.json in the dir where the script is located, just near it.
   * url - url for hosting server application;
   * port - port of hosting server application;
   * dbInitialCatalog - practically, this is database name;
   * dbDataSource - instance of database type (localdb, sqlexpress, etc);
   * email - the server sender email;
   * password - the server sender password;
   * frAccessUrl - url for frontend to work with;
   * frLogLevel - frontend logging level;
   * frLogPattern - frontend logging pattern;

   Look at this example:

``` json
{
   "url":"https://ihrbot.isd.dp.ua",
   "port": "9000",
   "dbInitialCatalog": "bot-9000",
   "dbDataSource": "(localdb)\\V11.0",
   "email": "sender@isd.dp.ua",
   "password":"******",
   "frAccessUrl": "https://ihrbot.isd.dp.ua",
   "frLogLevel": "ERROR",
   "frLogPattern": "*",
   "certhash" : "ab2aa72898a32bbc89467364c6d1cdf4f20ce412",
   "appid" : "ad219a93-07c5-4ff7-8984-9aff8ec9afae"
}
```

   **Attention**: `http(s)://+` is an alias for all local ip adresses. Is used to start host with httpListeners. Except of it can be used one explicit ip adress;

1. Run script `Deploy-Application` as **Admin**.

   Optional: when you run it is possible to specify two parameters:
   * DestinationPath: the path (string) where will be located fully server build
   * Deploy: switch, aka boolean flag; when is set the script gets the right to host the application - open remote access, attach certifiates, run server, etc.
1. Have a fun.

## What may cause errors

1. Make sure you have got Admin rights to run this script.
1. Make sure you have passed all parameters to script and created an _deploy.json_.
1. Take a look at configs, if any of _local.json_, _deploy.json_ or _SettingsContext_ of backend is in not consistant state, so build or runtime may fail. So look at deploy.json validation, if you add some new settings to application and so on for SettingsContext
1. Modules were not downloaded. Check you policies to grant script and work station access to remote module repositories.
1. Bundle finished with exception. Check you app, maybe some scripts, styles, etc were corrupted. More, check webpack config - maybe smth has changed, so the bundlig became unpredictable.
1. Building of backend unfortunately fall down. Maybe your solution accepted new configurations, so now msbuild gets lost in all of it and fails. Check versions of your libraries to be right the same you need.

   It is known an error when `msbuild` fails with exception of not finding execution or targets or versions on some path, the only solutions are:

   1. To give him what he needs.
   1. To delete from solution some lines, that are responsible for including that thing to your project. (No varanty for this deal, make sure that you ready to do all of that manually)

1. The release folder is not seen to be created. Check rights of creating folders and other ops in the destination folder, check that everythings builds fine - bundle and server files appear in their build folders. Check that you haven't missed with paths.
1. Problems with SSL? - Make sure that you have installed your certificate. Moreover, it is installed along with its private key. Check that you have passed correct _thumbprint_ to build script. Check where you cerificate is located, it must Trusted Root. Doesn't help? - check all params of your certificate, maybe broblems with issuer, CN, country and so on, so on...
1. There is no remote access to server? - Try run `netsh` with correct settings manually (look at its description above). Doesn't help, but seems to be working? - Sorry, contact your server administrator.
1. Some file is not found and so script or server fails with such a kind of exception? - make sure, that you haven't renamed something. so the path can't be resolved.