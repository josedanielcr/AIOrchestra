db = db.getSiblingDB('AIOrchestra');

var user = db.getUser("aiorchestraUser");

if (user === null) {
  db.createUser({
    user: "aiorchestraUser",
    pwd: "AIOrchestra123",
    roles: []
  });
}

db.grantRolesToUser(
  "aiorchestraUser",
  [
    {
      role: "readWrite",
      db: "AIOrchestra"
    }
  ]
);