#!/bin/bash
echo "Seeding MongoDB with Titanic data..."
mongoimport --host localhost --db PoseidonDB --collection Passengers --type csv --headerline --file /docker-entrypoint-initdb.d/titanic.csv
