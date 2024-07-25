# AIOrchestra
AIOrchestra leverages a scalable and efficient event-driven architecture with microservices to deliver personalized song recommendations. It is built using .NET Core, Kafka, Docker, and a recommendation system based on cosine similarity, making it an ideal project for learning and practicing these technologies.

## Table of Contents
- [Features](#features)
- [Technologies](#technologies)
- [Installation and Running Guide](#installation-and-running-guide)
- [Possible Errors](#possible-errors)

## Features
- **Microservice Architecture**: Independent, scalable services for different functionalities.
- **Event-Driven Architecture**: Utilizes Kafka for asynchronous communication between services.
- **Music Recommendation**: Custom recommendation system using cosine similarity to find similar songs based on user preferences.
- **Docker**: Containerized services for consistent deployment and management.

## Technologies
- **.NET Core**: Framework for building microservices.
- **Kafka**: Event streaming platform for managing communication between services.
- **Docker**: Containerization for deployment consistency.

## Installation and Running Guide
Follow these steps to set up and run AIOrchestra:

- Clone the repository:
  cd AIOrchestra

- Add a folder named `certificates` at the root of the project.

- Add `dotnet-dev-certs` to the `certificates` folder. Check Microsoft Documentation (https://learn.microsoft.com/en-us/dotnet/core/tools/dotnet-dev-certs) for more information.

- Navigate to the `MusicRecommenderService` directory and add a `certificates` folder.

- Add the server certificates (`server.crt` and `server.key`) to the `certificates` folder. Check this guide (https://helpcenter.gsx.com/hc/en-us/articles/115015960428-How-to-Generate-a-Self-Signed-Certificate-and-Private-Key-using-OpenSSL) for more information.

- Stay in the `MusicRecommenderService` directory and add a `data` folder.

- Download the `Music Info.csv` from this Kaggle dataset (https://www.kaggle.com/datasets/undefinenull/million-song-dataset-spotify-lastfm).

- Add the downloaded CSV file to the `data` folder and rename it to `music_data.csv`.

- In the root folder of the project, run the following command to build and start the services:
  docker compose up --build -d

- Go to http://localhost:4200 to enjoy some music recommendations.

  ## Possible Errors
- If `kafka broker-1` does not start automatically, rerun it from Docker Desktop using the ">" button.
- If the website does not render information, navigate directly to http://localhost:4200/explore.
- If after logging in for the first time nothing happens, reload the page as the services and Kafka broker are warming up.
