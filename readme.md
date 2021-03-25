# Requirements

1. Dotnet 5 (and VS 2019)

2. This app uses In Memory Database

3. Authentication is via JWT Bearer. Token is generated from `api/account/register` or `api/account/login`. This app promotes strong password usage.

4. For authorization in `PostsController`, token should be put in Header as `Authorization: bearer [TOKEN]`