# Use official MongoDB image as base
FROM mongo:latest

# Set the working directory in the container
WORKDIR /mongo-seed

# Copy the seeding script and data file into the container
COPY init-mongo.sh /docker-entrypoint-initdb.d/
COPY titanic.csv /mongo-seed/titanic.csv

# Make sure the script is executable
RUN chmod +x /docker-entrypoint-initdb.d/init-mongo.sh

# Expose the MongoDB port
EXPOSE 27017
