#!/bin/bash

echo "Starting MongoDB seeding script..."

# Set MONGO_HOST based on the environment
if [ "$ENVIRONMENT" == "docker-compose" ]; then
  MONGO_HOST=mongodb
  echo "Running in Docker Compose environment. Using database name: $MONGO_DB_NAME"
elif [ "$ENVIRONMENT" == "kubernetes" ]; then
  MONGO_HOST=localhost  # Connect to MongoDB within the same pod
  echo "Running in Kubernetes environment. Using database name: $MONGO_DB_NAME"
else
  echo "Unknown environment: $ENVIRONMENT"
  exit 1
fi

# Wait for MongoDB to be ready with detailed logging
echo "Checking MongoDB readiness on host $MONGO_HOST..."
while ! mongosh --host $MONGO_HOST --username "$MONGO_INITDB_ROOT_USERNAME" --password "$MONGO_INITDB_ROOT_PASSWORD" --authenticationDatabase "admin" --eval "db.runCommand({ ping: 1 })" > /dev/null 2>&1; do
  echo "MongoDB is not ready yet. Retrying in 10 seconds..."
  sleep 10
done

echo "MongoDB is ready!"

# Create the root user in the 'admin' database
mongosh --host $MONGO_HOST <<EOF
use admin
db.createUser({
  user: "$MONGO_INITDB_ROOT_USERNAME",
  pwd: "$MONGO_INITDB_ROOT_PASSWORD",
  roles: [{ role: "root", db: "admin" }]
})
EOF

echo "Admin user created. Creating database user for $MONGO_DB_NAME..."

# Create the user in the specified database
mongosh --host $MONGO_HOST <<EOF
use $MONGO_DB_NAME
db.createUser({
  user: "$MONGO_INITDB_ROOT_USERNAME",
  pwd: "$MONGO_INITDB_ROOT_PASSWORD",
  roles: [{ role: "dbOwner", db: "$MONGO_DB_NAME" }]
})
EOF

echo "User created for $MONGO_DB_NAME."

# Import Titanic CSV data into the database
echo "Seeding Titanic data into $MONGO_DB_NAME..."

mongoimport --host $MONGO_HOST --username "$MONGO_INITDB_ROOT_USERNAME" --password "$MONGO_INITDB_ROOT_PASSWORD" --authenticationDatabase "admin" --db "$MONGO_DB_NAME" --collection Passengers --type csv --file /mongo-seed/titanic.csv --headerline

echo "Data import completed!"
