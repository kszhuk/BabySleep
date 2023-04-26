# Baby Sleep Tracker

This application includes web and mobile parts, which are using shared cloud and local data for tracking and analyzing sleeps information.

## Project Description
<p>
This project includes web (Asp.Net Mvc Core) and mobile (Xamarin) parts.
</p>
<p>
Web part uses Amazon DynamoDb and AWS Lambda functions for tracking information.
<br/>
Mobile part uses SqlLite for work with data and call Amazon lambda functions for synchronizing local data to cloud.
</p>
<p>
Application uses Firebase for authentication.
</p>
<p>
Project is create based on DDD principles.
</p>
<p>
AWS Lambda functions are located in BabySleep.Api.
<br/>
Separate repositories and shared projects for both parts are injected in BabySleep.Core.
<br/>
Application layer, business processes from infrastracture layer are shared between web and mobile projects. Otherwise repositories for web and mobile parts are completely different because of using different types of databases (local and cloud).
<br/>
Integration and unit tests are located in BabySleep.Tests.
</p>

## License
MIT
