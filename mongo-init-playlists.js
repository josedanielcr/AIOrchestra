db = db.getSiblingDB('Playlist');

if (db.getUser("aiorchestraUser") === null) {
  db.createUser({
    user: "aiorchestraUser",
    pwd: "AIOrchestra123",
    roles: [
      {
        role: "readWrite",
        db: "AIOrchestra"
      }
    ]
  });
}