Google Calendar Sample
======================

An ASP.NET MVC 4 applicacition that demonstrates how to obtain &amp; manage authorization to use Google Services, and how to perform CRUD operations on a Google Calendar.


## How to use

You need to update the Web.config file with your Google API Service keys (ClientId, ClientSecret and RedirectUri). You can obtain/find this information in the [Google API Console](https://code.google.com/apis/console) 

  <?xml version="1.0" encoding="utf-8"?>
  <configuration>
    ...
    <appSettings>
      ...
      <!-- GoogleAPI credentials -->
      <add key="ClientId" value="{CLIENT-ID}" />
      <add key="ClientSecret" value="{CLIENT-SECRETD}" />
      <add key="RedirectUri" value="{REDIRECT-URI}" />
    </appSettings>
    <system.web>

Happy coding!
