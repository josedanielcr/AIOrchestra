// Authenticate as the root user
db = db.getSiblingDB('admin');
db.auth("root", "AIOrchestra123");

db = db.getSiblingDB('Playlist');

print("Connected to Playlist database");

var user = db.getUser("aiorchestraUser");

if (user === null) {
  print("User 'aiorchestraUser' does not exist. Creating user...");
  db.createUser({
    user: "aiorchestraUser",
    pwd: "AIOrchestra123",
    roles: []
  });
  print("User 'aiorchestraUser' created.");
} else {
  print("User 'aiorchestraUser' already exists.");
}

db.grantRolesToUser(
  "aiorchestraUser",
  [
    {
      role: "readWrite",
      db: "Playlist"
    }
  ]
);

print("Role 'readWrite' on database 'Playlist' granted to user 'aiorchestraUser'.");