db = db.getSiblingDB('AIOrchestra');

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