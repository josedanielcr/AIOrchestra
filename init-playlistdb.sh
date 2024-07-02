CONTAINER_NAME="aiorchestra-mongodb-playlist-management"
SCRIPT_PATH="/docker-entrypoint-initdb.d/mongo-init-playlists.js"
docker exec -it $CONTAINER_NAME mongosh $SCRIPT_PATH