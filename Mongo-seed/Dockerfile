# Use the official MongoDB image as a base
FROM mongo:6.0

# Copy the init script and Titanic CSV to the container
COPY init-mongo.sh /docker-entrypoint-initdb.d/
COPY titanic.csv /docker-entrypoint-initdb.d/

# Make sure the script is executable
RUN chmod +x /docker-entrypoint-initdb.d/init-mongo.sh

# Expose default MongoDB port
EXPOSE 27017

# Use the default entrypoint from MongoDB image
CMD ["mongod"]
