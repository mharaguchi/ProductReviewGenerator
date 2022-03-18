# Review Generator
This project uses basic Natural Language Processing (NLP) via Markov Chains to generate product reviews. The reviews include a basic sentiment-based star rating generator.

## Prerequisites
Requires .NET 6, npm, Docker, React.

## Setup
- Use included test data
<br>OR
- Download a 5-core Amazon review file (tested with Musical Instruments, Toys and Games, and Electronics) from http://jmcauley.ucsd.edu/data/amazon/ and extract GZ file.
- Add Amazon review JSON file to the \src\ReviewGenerator.Api\data folder.
- Modify \src\ReviewGenerator.Api\appsettings.json > FileSystemReviewRepositoryOptions > FilePath to reflect the correct JSON file. Make sure it has the relative path "data/" at the start.

## Build/Run
- From \ folder (should have .sln and docker-compose.yaml files), run "docker compose up --build".
<br>The console log entries should show when data training is finished.

## API Swagger
Assuming port 8139 was free and the Docker container started correctly, the API's Swagger page should be accessible at http://localhost:8139/Swagger.
The page will not be accessible until the data training has finished.

## React Website
Assuming port 3113 was free and the Docker container started correctly, the website should be accessible at http://localhost:3113. Press the Generate Review button to show a review and its associated star rating. If the API isn't ready yet, the website will throw errors in the JS console.

## Troubleshooting
If the docker compose command isn't working, the API can be started from Visual Studio using IIS Express or Docker. The website can be started from the \web\review-generator\ folder by running "npm start", but the fetch URL will need to be manually updated with whatever port the API is running on.

## Notes
- Has 2 review text generator implementations, one custom built (about 75% done), one based on the Markov NuGet package. Currently set to be the NuGet package version as the custom solution was too slow for large datasets.
- Has 2 different stars generator implementations, one random, one basic sentiment-based. Currently set to be the sentiment-based version.
- API config has a permissive CORS policy to try to avoid issues. Prod would need to be more restrictive.
