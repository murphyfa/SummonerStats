# SummonerStats
# Work in Progress

The Summoner Stats website allows you to search for a League of Legends player and it displays a profile page for them including information about previous games played, ranking, most played champions and more.

What it does:
When a player is searched for, the player's basic info is retrieved from the API, and their match history is updated in the database. The page currently displays the three most recent matches and the 5 characters they play the most (the banner image at the top reflects the most played character). The details for a match are stored in the database if it hasn't been viewed before. If it has, it is simply pulled from the database to reduce API requests.

This project is built on the ASP.NET framework, following MVC design. Data is pulled from the Riot Games REST API (JSON). 

A demo of current functionality is available here: http://summonerstats.azurewebsites.net/Profile/Search/search-fld?searchName=mount+swolemore

Please note that I am still using a developer API key, so requests are very limited and navigating profiles may throw errors for too many requests. Once completed, a production API key will alleviate this, as well as functionality I plan to implement to minimize API requests.
