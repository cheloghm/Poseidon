#!/bin/bash

# Create the root user for MongoDB in the 'admin' database
mongosh <<EOF
use admin
db.createUser({
  user: "$MONGO_INITDB_ROOT_USERNAME",
  pwd: "$MONGO_INITDB_ROOT_PASSWORD",
  roles: [{ role: "root", db: "admin" }]
})
EOF

# Create the root user for MongoDB in the 'PoseidonDBPod' database if it doesn't exist
mongosh <<EOF
use PoseidonDBPod
db.createUser({
  user: "$MONGO_INITDB_ROOT_USERNAME",
  pwd: "$MONGO_INITDB_ROOT_PASSWORD",
  roles: [{ role: "dbOwner", db: "PoseidonDBPod" }]
})
EOF

# Wait for MongoDB to be ready using mongosh
until mongosh --username "$MONGO_INITDB_ROOT_USERNAME" --password "$MONGO_INITDB_ROOT_PASSWORD" --authenticationDatabase "admin" --eval "print('waiting for connection to MongoDB')" || [ $? -ne 0 ]; do
  sleep 2
done

# Import Titanic data using mongosh
mongosh --username "$MONGO_INITDB_ROOT_USERNAME" --password "$MONGO_INITDB_ROOT_PASSWORD" --authenticationDatabase "admin" --eval "
  var db = connect('mongodb://$MONGO_INITDB_ROOT_USERNAME:$MONGO_INITDB_ROOT_PASSWORD@localhost:27017/$MONGO_DB_NAME');
  db.Passengers.drop();
  db.Passengers.createIndex({ Name: 1 });
  print('Database initialized');
"

# Import Titanic CSV data using mongoimport
mongoimport --username "$MONGO_INITDB_ROOT_USERNAME" --password "$MONGO_INITDB_ROOT_PASSWORD" --authenticationDatabase "admin" --db "$MONGO_DB_NAME" --collection Passengers --type csv --file /mongo-seed/titanic.csv --headerline
