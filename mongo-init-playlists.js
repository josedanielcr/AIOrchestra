db = db.getSiblingDB('Playlist');

db.createUser({
    user: "aiorchestraUser",
    pwd: "AIOrchestra123",
    roles: [
      {
        role: "readWrite",
        db: "Playlist"
      }
    ]
});