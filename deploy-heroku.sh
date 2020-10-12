#!/bin/bash
echo "Login on Heroku"
heroku login
echo "Login on Heroku Container Registry via Docker"
docker login --username=_ --password=$HEROKU_API_KEY registry.heroku.com
echo "Building the Docker Image"
docker build -t registry.heroku.com/$HEROKU_APP_NAME/web -f src/KanbanBoard.WebApi/dockerfile .
echo "Pushing the image to Heroku Container Registry"
docker push registry.heroku.com/$HEROKU_APP_NAME/web
echo "Release on Heroku"
heroku container:release web --app $HEROKU_APP_NAME
