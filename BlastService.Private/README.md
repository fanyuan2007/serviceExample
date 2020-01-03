## HxM.MinePlanBlastPrivateService

Contains service to communicate with the MinePlan Data Service through a private REST API

Docker Launcher:
- Self-signed CA (webserver.pfx) attached for Dev/Test purpose
- The pass code for CA is embedded in Dockerfile for Dev/QA convenience (DO NOT do that in the production)
- The SQL Server name needs to be resolved from the container, if it is not localhost.
  One way to do that is to specify "--dns=10.40.100.67 --dns-search=lgs-net.com" (for our network) with docker run,
  or you can set those properties globally with docker daemon config, on Windows it is %programdata%\docker\config\daemon.json
  {
  "dns": ['10.40.100.67'],
  "dns-search": ['lgs-net.com']
  }